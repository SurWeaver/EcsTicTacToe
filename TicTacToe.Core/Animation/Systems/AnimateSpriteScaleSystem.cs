using Leopotam.EcsLite;
using Microsoft.Xna.Framework;
using Surtility.Drawing.Components;
using Surtility.Extensions;
using Surtility.Tweening.Components;

namespace TicTacToe.Core.Animation.Systems;

public class AnimateSpriteScaleSystem
    : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private EcsFilter _filter;
    private EcsPool<SpriteScale> _spriteScalePool;
    private EcsPool<TweenCurrentValue<Vector2>> _tweenPool;
    private EcsPool<Target> _targetPool;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();

        _filter = _world.Filter<SpriteScale>()
            .Inc<TweenCurrentValue<Vector2>>()
            .Inc<Target>()
            .End();

        _tweenPool = _world.GetPool<TweenCurrentValue<Vector2>>();
        _targetPool = _world.GetPool<Target>();
        _spriteScalePool = _world.GetPool<SpriteScale>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _filter)
        {
            ref var tween = ref _tweenPool.Get(entity);
            ref var target = ref _targetPool.Get(entity).Entity;

            if (target.Unpack(_world, out int targetEntity))
            {
                ref var scale = ref _spriteScalePool.GetOrAdd(targetEntity);

                scale.Vector = tween.Value;
            }
        }
    }
}
