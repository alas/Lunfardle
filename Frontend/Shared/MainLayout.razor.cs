namespace Lunfardle.Shared;

using Lunfardle.Game;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text;

public partial class MainLayout
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    [Inject]
    private ILocalStorageService LocalStorageService { get; set; }

    [Inject]
    private IJSRuntime JSRuntime { get; set; }

    [Inject]
    private Game Game { get; set; }

    [Inject]
    private NavigationManager UriHelper { get; set; }

    private bool ShowStatistics { get; set; }

    private int[] Statistics { get; set; }

    private double[] StatisticsSeries { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private int CountPlays { get; set; }

    private int WinsPercent { get; set; }

    protected override void OnInitialized()
    {
        Game.GameUpdated +=
            () =>
            {
                InvokeAsync(StateHasChanged);
                SetSavedStatistics();
            };
        GetSavedStatistics();
    }

    private void GetSavedStatistics()
    {
        Statistics = LocalStorageService.GetItem<int[]?>("Statistics") ?? new int[9];
        /*
        Statistics[0] == countPlays1
        Statistics[1] == countPlays2
        Statistics[2] == countPlays3
        Statistics[3] == countPlays4
        Statistics[4] == countPlays5
        Statistics[5] == countPlays6
        Statistics[6] == countLost;
        Statistics[7] == currentStreak;
        Statistics[8] == MaxStreak;*/
        var countWins = Statistics.Take(6).Sum();
        CountPlays = countWins + Statistics[6];
        if (CountPlays > 0) WinsPercent = countWins * 100 / CountPlays;

        StatisticsSeries = Statistics.Take(6).Select(t => (double)t).ToArray();
    }

    private void SetSavedStatistics()
    {
        if (Game.IsWin)
        {
            Statistics[Game.Results.Count - 1]++;
            Statistics[7]++;
            if (Statistics[7] > Statistics[8])
                Statistics[8] = Statistics[7];
            LocalStorageService.SetItem("Statistics", Statistics);
        }
        else if (Game.IsLose)
        {
            Statistics[6]++;
            Statistics[7] = 0;
            LocalStorageService.SetItem("Statistics", Statistics);
        }
    }

    private string GetSquares(bool putBreaks)
    {
        StringBuilder sb = new();
        foreach (var guessResult in Game.Results)
        {
            foreach (var (_, matchResult) in guessResult)
            {
                sb.Append(matchResult switch
                {
                    MatchResult.FullHit => "🟩",
                    MatchResult.CharHit => "🟨",
                    _ => "⬜",
                });
            }
            sb.AppendLine(putBreaks ? "<br />" : "");
        }
        return sb.ToString();
    }

    private ValueTask CopyToClipboard()
    {
        return JSRuntime.InvokeVoidAsync("navigator.clipboard.writeText", GetSquares(false));
    }

    private void Reset()
    {
        LocalStorageService.SetItem("Statistics", default(int[]));
        LocalStorageService.SetItem("LastGuessResults", default(List<GuessResult[]>));
        LocalStorageService.SetItem("LastSavedDate", default(DateTime));
        UriHelper.NavigateTo(UriHelper.Uri, forceLoad: true);
    }
}
