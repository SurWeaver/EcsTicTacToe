using Leopotam.EcsLite;
using Microsoft.Xna.Framework;
using Surtility.Drawing.Components;
using Surtility.Input;
using TicTacToe.Core.Input.Components;
using TicTacToe.Core.Input.Enums;
using TicTacToe.Core.User.Components;
using TicTacToe.Core.Utils;

namespace TicTacToe.Core.Input.Systems;

public class UpdateCursorPositionSystem(InputDevices device, Point windowSize)
    : IEcsInitSystem, IEcsRunSystem
{
    private readonly Vector2 WindowCenter = windowSize.ToVector2() / 2;
    private readonly float WindowRadius = windowSize.X / 2;

    private EcsFilter _filter;
    private EcsPool<Control> _controlPool;
    private EcsPool<SpritePosition> _positionPool;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        _filter = world.Filter<Cursor>()
            .Inc<Control>()
            .Inc<SpritePosition>()
            .End();

        _controlPool = world.GetPool<Control>();
        _positionPool = world.GetPool<SpritePosition>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _filter)
        {
            var controlMethod = _controlPool.Get(entity).Method;
            ref var position = ref _positionPool.Get(entity).Vector;

            switch (controlMethod)
            {
                case ControlMethod.Mouse:
                    position = device.Mouse.MousePosition.ToVector2();
                    break;

                case ControlMethod.GamePad1:
                    position = GetPositionFromGamePad(device.Pad1);
                    break;

                case ControlMethod.GamePad2:
                    position = GetPositionFromGamePad(device.Pad2);
                    break;
            }
        }
    }

    public Vector2 GetPositionFromGamePad(GamePadController pad)
    {
        var stickValue = pad.GetStickValue(ControllerSide.Left);
        var normalizedStickPosition = new Vector2(stickValue.X, -stickValue.Y);


        return WindowCenter + normalizedStickPosition * WindowRadius;
    }
}
