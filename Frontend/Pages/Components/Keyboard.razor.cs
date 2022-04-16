namespace Lunfardle.Pages.Components;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Lunfardle.Game;

public partial class Keyboard
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    [Inject]
    private WordsLists WordsLists { get; set; }

    [Inject]
    private Game Game { get; set; }

    [Inject]
    private GameInput GameInput { get; set; }

    [Inject]
    ISnackbar Snackbar { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    private ElementReference _container;

    private readonly char[][] _keyboardRows =
    {
        new[] { 'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p' },
        new[] { 'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'ñ' },
        new[] { 'z', 'x', 'c', 'v', 'b', 'n', 'm' },
    };

    protected override async Task OnAfterRenderAsync(bool firstRender) => await CatchFocus();

    private async Task CatchFocus() => await _container.FocusAsync();

    private void OnKeyDown(KeyboardEventArgs e)
    {
        if (e.Repeat) return;

        var value = e.Key.Trim().ToLower();
        switch (value)
        {
            case "enter":
                Guess();
                break;
            case "backspace":
                Back();
                break;
            default:
                if (value.Length == 1 && char.IsLetter(value[0]))
                    GameInput.Input(value[0]);
                break;
        }
    }

    private void Back() => GameInput.Back();

    private void Guess()
    {
        var str = GameInput.GetString();
        if (String.IsNullOrWhiteSpace(str) || !WordsLists.WordExists(str.ToUpper()))
        {
            Snackbar.Add("No está en la lista!", Severity.Normal,
                config =>
                {
                    config.VisibleStateDuration = 500;
                    config.ShowCloseIcon = false;
                });
            return;
        }

        Game.Guess(GameInput.Flush());
    }
}
