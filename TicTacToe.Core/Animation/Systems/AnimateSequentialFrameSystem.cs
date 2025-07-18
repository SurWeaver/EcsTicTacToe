using Leopotam.EcsLite;
using Surtility.Drawing.Components;
using Surtility.Timing;
using TicTacToe.Core.Animation.Components;

namespace TicTacToe.Core.Animation.Systems;

public class AnimateSequentialFrameSystem
    : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _filter;
    private EcsPool<SequentialFrame> _sequentialFramePool;
    private EcsPool<CurrentFrame> _framePool;
    private EcsPool<FrameCount> _frameCountPool;
    private EcsPool<SpriteBorder> _borderPool;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        _filter = world.Filter<SequentialFrame>()
            .Inc<FrameCount>()
            .Inc<CurrentFrame>()
            .End();

        _sequentialFramePool = world.GetPool<SequentialFrame>();

        _framePool = world.GetPool<CurrentFrame>();
        _frameCountPool = world.GetPool<FrameCount>();
        _borderPool = world.GetPool<SpriteBorder>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _filter)
        {
            ref var sequentialFrame = ref _sequentialFramePool.Get(entity);
            sequentialFrame.CurrentSeconds += DeltaTime.Seconds;

            if (sequentialFrame.CurrentSeconds >= sequentialFrame.SecondsToSwitch)
            {
                sequentialFrame.CurrentSeconds = 0;
                SwitchEntityFrame(entity);
            }
        }
    }

    private void SwitchEntityFrame(int entity)
    {
        ref var frameIndex = ref _framePool.Get(entity).Index;
        ref var frameCount = ref _frameCountPool.Get(entity).Count;

        frameIndex = (frameIndex + 1) % frameCount;

        if (_borderPool.Has(entity))
            _borderPool.Del(entity);
    }
}
