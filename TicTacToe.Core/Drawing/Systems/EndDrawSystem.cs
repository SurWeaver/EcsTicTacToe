using Leopotam.EcsLite;
using Microsoft.Xna.Framework.Graphics;

namespace TicTacToe.Core.Drawing.Systems;

public class EndDrawSystem(SpriteBatch spriteBatch)
    : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        spriteBatch.End();
    }
}
