namespace Lunfardle.Game;

public class Game
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string Answer { get; private set; }

    public List<GuessResult[]> Results { get; private set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private int MaxAttempt { get; set; }

    /// <summary>
    /// Keyboard letters state
    /// </summary>
    public readonly Dictionary<char, MatchResult> Distribution = new();

    public event Action? GameUpdated;

    public bool IsWin { get; private set; }

    public bool IsLose => !IsWin && Results.Count >= MaxAttempt && !Answer.Equals(string.Empty);

    public void Init(string answer, int maxAttempt, List<GuessResult[]> results)
    {
        var lastGuessResults = results.LastOrDefault();
        var lastGuessWord = lastGuessResults != null ? string.Concat(lastGuessResults.Select(t => t.Letter)).ToUpper() : null;
        IsWin = lastGuessWord == answer;
        Answer = answer;
        MaxAttempt = maxAttempt;
        Results = results;
        Distribution.Clear();
        foreach (var result in results)
            MatchDistribution(result);
        GameUpdated?.Invoke();
    }

    public void Guess(string input)
    {
        if (IsWin || IsLose) return;
        if (Answer.Equals(string.Empty)) return;
        if (Results.Count == MaxAttempt) return;

        var guess = new Guess(Answer);
        var guessResults = guess.Match(input);

        MatchDistribution(guessResults);

        Results.Add(guessResults);
        IsWin = guess.IsWin();
        GameUpdated?.Invoke();
    }

    private void MatchDistribution(IEnumerable<GuessResult> results)
    {
        foreach ((var letter, var matchResult) in results)
        {
            if (!Distribution.ContainsKey(letter))
            {
                Distribution[letter] = matchResult;
                continue;
            }

            var matchType = Distribution[letter];

            if (matchType == MatchResult.NoneHit && matchResult != MatchResult.NoneHit ||
                matchType == MatchResult.CharHit && matchResult == MatchResult.FullHit)
            {
                Distribution[letter] = matchResult;
            }
        }
    }
}
