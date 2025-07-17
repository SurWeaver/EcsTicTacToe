using Leopotam.EcsLite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Surtility.Drawing.Components;
using Surtility.Extensions;
using Surtility.Tools;
using Surtility.Tweening;
using Surtility.Tweening.Components;
using TicTacToe.Core.Animation.Components;
using TicTacToe.Core.Drawing.Components;
using TicTacToe.Core.Input.Components;

namespace TicTacToe.Core.Drawing.Systems;

public class CreateInputAssignScreenSystem(Texture2D inputIcons)
    : IEcsInitSystem, IEcsRunSystem
{
    private const double IconHoverOneDirectionDuration = 2;

    private const int TopY = 48;
    private const int BottomY = 80;

    private EcsWorld _world;
    private EcsFilter _filter;
    private EcsPool<ShowingInputAssignScreen> _showPool;
    private EcsPool<PlayerIcon> _playerIconPool;
    private EcsPool<SpriteColor> _colorPool;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();

        _filter = _world.Filter<WaitForInput>()
            .Inc<PlayerIcon>()
            .Exc<ShowingInputAssignScreen>()
            .End();

        _showPool = _world.GetPool<ShowingInputAssignScreen>();
        _playerIconPool = _world.GetPool<PlayerIcon>();

        _colorPool = _world.GetPool<SpriteColor>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _filter)
        {
            _showPool.Add(entity);

            ref var playerIcon = ref _playerIconPool.Get(entity).Texture;

            var playerIconEntity = EntityGenerator.NewEntity()
                .With(new AssignInputUI())
                .With(new Sprite(playerIcon))
                .With(new SpritePosition())
                .With(new FrameCount(4))
                .With(new RandomFrame())
                .End();

            if (_colorPool.Has(entity))
            {
                var color = _colorPool.Get(entity);
                _colorPool.Add(playerIconEntity, color);
            }

            AnimateEntityHover(playerIconEntity,
                topPosition: new(32, TopY), bottomPosition: new(32, BottomY));

            var inputIconEntity = EntityGenerator.NewEntity()
                .With(new AssignInputUI())
                .With(new Sprite(inputIcons))
                .With(new SpritePosition())
                .With(new FrameCount(3))
                .With(new SequentialFrame(secondsToSwitch: 1))
                .End();

            AnimateEntityHover(inputIconEntity,
                topPosition: new(96, TopY), bottomPosition: new(96, BottomY));
        }
    }

    private void AnimateEntityHover(int entity, Vector2 topPosition, Vector2 bottomPosition)
    {
        EntityGenerator.NewEntity()
            .With(new Tween(IconHoverOneDirectionDuration, percent: 0.5f))
            .With(new Looping())
            .With(new Easing(EasingType.SineInOut))
            .With(new SpritePosition())
            .With(new TweenValuePair<Vector2>(bottomPosition, topPosition))
            .With(new Target(_world.PackEntity(entity)))
            .With(new AssignInputUI());
    }
}
