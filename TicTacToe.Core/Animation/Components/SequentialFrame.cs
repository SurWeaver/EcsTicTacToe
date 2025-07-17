namespace TicTacToe.Core.Animation.Components;

public struct SequentialFrame(double secondsToSwitch)
{
    public readonly double SecondsToSwitch = secondsToSwitch;
    public double CurrentSeconds;
}
