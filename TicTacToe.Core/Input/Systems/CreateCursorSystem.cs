using Leopotam.EcsLite;
using Surtility.Drawing.Components;
using Surtility.Tools;
using TicTacToe.Core.Drawing.Components;
using TicTacToe.Core.Input.Components;
using TicTacToe.Core.User.Components;

namespace TicTacToe.Core.Input.Systems;

public class CreateCursorSystem
    : IEcsInitSystem, IEcsRunSystem
{
    private EcsFilter _filter;
    private EcsPool<CreateCursor> _createCursorPool;
    private EcsPool<CursorIcon> _iconPool;
    private EcsPool<Control> _controlPool;
    private EcsPool<SpriteColor> _colorPool;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        _filter = world.Filter<Player>()
            .Inc<CreateCursor>()
            .Inc<Control>()
            .Inc<CursorIcon>()
            .End();

        _createCursorPool = world.GetPool<CreateCursor>();
        _iconPool = world.GetPool<CursorIcon>();
        _controlPool = world.GetPool<Control>();
        _colorPool = world.GetPool<SpriteColor>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _filter)
        {
            _createCursorPool.Del(entity);

            ref var icon = ref _iconPool.Get(entity);
            var controlMethod = _controlPool.Get(entity).Method;
            var color = _colorPool.Get(entity).Color;
            EntityGenerator.NewEntity()
                .With(new Cursor())
                .With(new Control(controlMethod))

                .With(new Sprite(icon.Texture))
                .With(new SpriteColor(color))
                .With(new SpriteOrigin(icon.Origin))
                .With(new SpritePosition());
        }
    }
}
