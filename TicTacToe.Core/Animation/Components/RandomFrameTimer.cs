namespace TicTacToe.Core.Animation.Components;

public struct RandomFrameTimer(double seconds)
{
    public double Seconds = seconds;
    public double CurrentSeconds;
}
