using Leopotam.EcsLite;
using Surtility.Drawing.Components;
using Surtility.Extensions;
using TicTacToe.Core.Animation.Components;
using TicTacToe.Core.Utils;

namespace TicTacToe.Core.Animation.Systems;

public class AssignRandomFrameSystem
    : IEcsInitSystem, IEcsRunSystem
{
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

            _framePool.Add(entity, new(Randomizer.GetInt(count)));
        }
    }
}
