using Microsoft.Xna.Framework;
using Surtility.Input;

namespace TicTacToe.Core.Utils;

public class InputDevices
{
    public readonly MouseController Mouse = new();
    public readonly GamePadController Pad1 = new(PlayerIndex.One);
    public readonly GamePadController Pad2 = new(PlayerIndex.Two);

    public void Update()
    {
        Mouse.Update();
        Pad1.Update();
        Pad2.Update();
    }
}
