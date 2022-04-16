namespace Lunfardle.Pages.Components;

using Microsoft.AspNetCore.Components;
using Lunfardle.Game;

public partial class LetterBox
{
    [Parameter]
    public char Letter { get; set; }

    [Parameter]
    public MatchResult MatchResult { get; set; }

    [Parameter]
    public bool TileShake { get; set; }

    private string GetCssClass()
    {
        return MatchResult switch
        {
            MatchResult.FullHit => "tile-full-hit",
            MatchResult.CharHit => "tile-partial-hit",
            MatchResult.NoneHit => "tile-not-hit",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
