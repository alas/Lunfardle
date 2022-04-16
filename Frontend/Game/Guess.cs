namespace Lunfardle.Game;

public class Guess
{
    private readonly GuessResult[] _result;
    private readonly string _answer;

    public Guess(string answer)
    {
        _answer = Cleanse(answer);
        _result = new GuessResult[answer.Length];
    }

    public bool IsWin() => _result.All(g => g.Result == MatchResult.FullHit);

    public GuessResult[] Match(string input)
    {
        if (input.Length != _answer.Length)
            throw new Exception("Should have same length.");

        input = Cleanse(input);
        var answerLetters = _answer.Select(t => (char?)t).ToList();

        // first pass: mark the correct ones
        for (var i = 0; i < input.Length; i++)
        {
            if (!char.IsLetter(input[i]) || !char.IsLetter(answerLetters[i]!.Value))
                throw new Exception("Should contains letter only.");

            if (input[i].Equals(answerLetters[i])) {
                _result[i] = new GuessResult(input[i], MatchResult.FullHit);
                answerLetters[i] = null;
            }
        }

        // second pass: mark the ones that are present
        for (var i = 0; i < input.Length; i++)
        {
            if (_result[i] is null) {
                for (var j = 0; j < input.Length; j++)
                {
                    if (answerLetters[j].Equals(input[i]))
                    {
                        _result[i] = new GuessResult(input[i], MatchResult.CharHit);
                        answerLetters[j] = null;
                        break;
                    }
                }
            }
        }

        // 3rd pass: mark the rest
        for (var i = 0; i < input.Length; i++)
        {
            if (_result[i] is null) {
                _result[i] = new GuessResult(input[i], MatchResult.NoneHit);
            }
        }

        return _result;
    }

    private static string Cleanse(string input) => input.Trim().ToLower();
}
