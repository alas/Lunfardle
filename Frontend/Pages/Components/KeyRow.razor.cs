namespace Lunfardle.Pages.Components;

using Microsoft.AspNetCore.Components;
using Lunfardle.Game;

public partial class KeyRow
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    [Inject]
    private GameInput GameInput { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    [Parameter]
    public char[]? KeysToGenerate { get; set; }
}
