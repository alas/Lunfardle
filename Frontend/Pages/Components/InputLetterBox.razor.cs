namespace Lunfardle.Pages.Components;

using Microsoft.AspNetCore.Components;

public partial class InputLetterBox
{
    [Parameter]
    public char Letter { get; set; }

    private string GetCssClass()
    {
        return Letter switch
        {
            ' ' => "tile-empty",
            _ => "tile-input"
        };
    }
}
