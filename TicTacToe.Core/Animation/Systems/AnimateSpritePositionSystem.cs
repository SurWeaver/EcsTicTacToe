using Leopotam.EcsLite;
using Microsoft.Xna.Framework;
using Surtility.Drawing.Components;
using Surtility.Tweening.Components;

namespace TicTacToe.Core.Animation.Systems;

public class AnimateSpritePositionSystem
    : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private EcsFilter _filter;
    private EcsPool<TweenCurrentValue<Vector2>> _tweenPool;
    private EcsPool<Target> _targetPool;
    private EcsPool<SpritePosition> _positionPool;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();

        _filter = _world.Filter<Target>()
            .Inc<TweenCurrentValue<Vector2>>()
            .Inc<SpritePosition>()
            .End();

        _tweenPool = _world.GetPool<TweenCurrentValue<Vector2>>();
        _targetPool = _world.GetPool<Target>();
        _positionPool = _world.GetPool<SpritePosition>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var tweenEntity in _filter)
        {
            ref var target = ref _targetPool.Get(tweenEntity).Entity;

            if (!target.Unpack(_world, out int targetEntity))
                continue;

            var newPosition = _tweenPool.Get(tweenEntity).Value;

            ref var currentPosition = ref _positionPool.Get(targetEntity);
            currentPosition.Vector = newPosition;
        }
    }
}
