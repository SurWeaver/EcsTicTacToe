using Leopotam.EcsLite;
using Microsoft.Xna.Framework.Graphics;

namespace TicTacToe.Core.Drawing.Systems;

public class BeginDrawSystem(SpriteBatch spriteBatch, SamplerState samplerState)
    : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        spriteBatch.Begin(samplerState: samplerState);
    }
}
