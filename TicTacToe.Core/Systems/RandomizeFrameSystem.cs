using Leopotam.EcsLite;
using Surtility.Drawing.Components;
using Surtility.Extensions;
using TicTacToe.Core.Components;

namespace TicTacToe.Core.Systems;

public class RandomizeFrameSystem
    : IEcsInitSystem, IEcsRunSystem
{
    private readonly Random random = new(DateTime.Now.GetHashCode());

    private EcsFilter _filter;
    private EcsPool<FrameCount> _countPool;
    private EcsPool<CurrentFrame> _framePool;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        _filter = world.Filter<Sprite>()
            .Inc<FrameCount>()
            .Inc<RandomFrame>()
            .Exc<CurrentFrame>()
            .End();

        _countPool = world.GetPool<FrameCount>();
        _framePool = world.GetPool<CurrentFrame>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _filter)
        {
            ref var count = ref _countPool.Get(entity).Count;

            _framePool.Add(entity, new(random.Next(count)));
        }
    }
}
