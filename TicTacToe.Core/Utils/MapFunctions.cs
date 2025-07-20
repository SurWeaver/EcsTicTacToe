using Microsoft.Xna.Framework;
using Surtility.Drawing.Components;

namespace TicTacToe.Core.Utils;

public static class MapFunctions
{
    public static void SaveSpritePosition(ref SpritePosition position, Vector2 vector) => position.Vector = vector;
    public static void SaveSpriteScale(ref SpriteScale scale, Vector2 vector) => scale.Vector = vector;
    public static void SaveSpriteColor(ref SpriteColor sprite, Color color) => sprite.Color = color;
}
