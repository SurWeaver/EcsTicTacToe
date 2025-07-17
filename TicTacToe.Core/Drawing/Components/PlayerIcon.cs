using Microsoft.Xna.Framework.Graphics;

namespace TicTacToe.Core.Drawing.Components;

public struct PlayerIcon(Texture2D texture)
{
    public Texture2D Texture = texture;
}
