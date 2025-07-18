using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TicTacToe.Core.Drawing.Components;

public struct CursorIcon(Texture2D texture, Vector2 origin)
{
    public Texture2D Texture = texture;
    public Vector2 Origin = origin;
}
