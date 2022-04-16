namespace Lunfardle.Pages.Components;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Lunfardle.Game;

public partial class KeyboardButton
{
    /*
    * The generated tag data from MudBlazor components will change, have to put the styles in app.css instead of
    * KeyboardButton.razor.css.Components that use HTML tag like LetterBox can use LetterBox.razor.css.
    * We choose <div> over putting the styles to app.css
    * <MudButton Variant = "Variant.Filled" Class=@("key-button "+ GetCssStyle()) @onclick="OnClick">@Text</MudButton>
    */

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    [Inject]
    private Game Game { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    [Parameter]
    public char Text { get; set; }

    [Parameter]
    public Action<MouseEventArgs> OnClick { get; set; } = null!;

    private string GetCssStyle()
    {
        if (!Game.Distribution.ContainsKey(Text)) return "black-gold-container";

        var matchResult = Game.Distribution[Text];
        return matchResult switch
        {
            MatchResult.NoneHit => "btn-none-hit",
            MatchResult.CharHit => "btn-char-hit",
            MatchResult.FullHit => "btn-full-hit",
            _ => string.Empty,
        };
    }
}
