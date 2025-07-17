using TicTacToe.Core.Input.Enums;

namespace TicTacToe.Core.Input.Components;

public struct Control(ControlMethod method)
{
    public ControlMethod Method = method;
}
