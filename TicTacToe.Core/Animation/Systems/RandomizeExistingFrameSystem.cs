using Leopotam.EcsLite;
using Surtility.Drawing.Components;
using Surtility.Timing;
using Surtility.Utils;
using TicTacToe.Core.Animation.Components;

namespace TicTacToe.Core.Animation.Systems;

public class RandomizeExistingFrameSystem
    : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _timerFilter;
    private EcsFilter _filter;
    private EcsPool<RandomFrameTimer> _randomTimePool;
    private EcsPool<CurrentFrame> _framePool;
    private EcsPool<FrameCount> _countPool;
    private EcsPool<SpriteBorder> _borderPool;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        _timerFilter = world.Filter<RandomFrameTimer>()
            .End();

        _filter = world.Filter<RandomFrame>()
            .Inc<CurrentFrame>()
            .Inc<FrameCount>()
            .Inc<SpriteBorder>()
            .End();

        _randomTimePool = world.GetPool<RandomFrameTimer>();

        _framePool = world.GetPool<CurrentFrame>();
        _countPool = world.GetPool<FrameCount>();
        _borderPool = world.GetPool<SpriteBorder>();
    }

    public void Run(IEcsSystems systems)
    {
        var randomize = false;
        foreach (var entity in _timerFilter)
        {
            ref var randomTime = ref _randomTimePool.Get(entity);

            randomTime.CurrentSeconds += DeltaTime.Seconds;
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
            var count = _countPool.Get(entity).Count;
            ref var currentFrameIndex = ref _framePool.Get(entity).Index;
            var newFrameIndex = currentFrameIndex;

            while (newFrameIndex == currentFrameIndex)
                newFrameIndex = Randomizer.GetInt(count);

            currentFrameIndex = newFrameIndex;

            _borderPool.Del(entity);
        }
    }
}
