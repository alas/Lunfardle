namespace Lunfardle.Pages;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Lunfardle.Game;

public partial class Index
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    [Inject]
    private ILocalStorageService LocalStorageService { get; set; }

    [Inject]
    private WordsLists WordsLists { get; set; }

    [Inject]
    private Game Game { get; set; }

    [Inject]
    private GameInput GameInput { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    protected override void OnInitialized()
    {
        Game.GameUpdated +=
            () =>
            {
                InvokeAsync(StateHasChanged);
                SetLastSavedResults();
            };
        GameInput.InputChanged += () => InvokeAsync(StateHasChanged);

        var results = GetLastSavedResults() ?? new();
        var answer = WordsLists.GetWordOfTheDay();
        Game.Init(answer, 6, results);
        GameInput.Init(answer.Length);
    }

    private List<GuessResult[]>? GetLastSavedResults()
    {
        var lastSavedDate = LocalStorageService.GetItem<DateTime?>("LastSavedDate");
        return lastSavedDate != null && lastSavedDate == DateTime.Today
            ? LocalStorageService.GetItem<List<GuessResult[]>>("LastGuessResults")
            : null;
    }

    private void SetLastSavedResults()
    {
        LocalStorageService.SetItem("LastGuessResults", Game.Results);
        LocalStorageService.SetItem("LastSavedDate", DateTime.Today);
    }
}
