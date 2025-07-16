using Leopotam.EcsLite;
using Surtility.Drawing.Components;
using Surtility.Timing.Components;
using TicTacToe.Core.Components;

namespace TicTacToe.Core.Systems;

public class RandomizeExistingFrameSystem
    : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _timerFilter;
    private EcsFilter _filter;
    private EcsPool<DeltaTime> _deltaTimePool;
    private EcsPool<RandomFrameTimer> _randomTimePool;
    private EcsPool<CurrentFrame> _framePool;
    private EcsPool<SpriteBorder> _borderPool;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        _timerFilter = world.Filter<RandomFrameTimer>()
            .Inc<DeltaTime>()
            .End();

        _filter = world.Filter<RandomFrame>()
            .Inc<CurrentFrame>()
            .Inc<FrameCount>()
            .Inc<SpriteBorder>()
            .End();

        _deltaTimePool = world.GetPool<DeltaTime>();
        _randomTimePool = world.GetPool<RandomFrameTimer>();

        _framePool = world.GetPool<CurrentFrame>();
        _borderPool = world.GetPool<SpriteBorder>();
    }

    public void Run(IEcsSystems systems)
    {
        var randomize = false;
        foreach (var entity in _timerFilter)
        {
            ref var deltaTime = ref _deltaTimePool.Get(entity);
            ref var randomTime = ref _randomTimePool.Get(entity);

            randomTime.CurrentSeconds += deltaTime.Seconds;
            if (randomTime.CurrentSeconds >= randomTime.Seconds)
            {
                randomize = true;
                randomTime.CurrentSeconds = 0;
            }
            break;
        }

        if (!randomize)
            return;

        foreach (var entity in _filter)
        {
            _framePool.Del(entity);
            _borderPool.Del(entity);
        }
    }
}
