using Leopotam.EcsLite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Surtility.Extensions;
using Surtility.Input;
using Surtility.Tools;
using TicTacToe.Core.Events;
using TicTacToe.Core.Input.Components;
using TicTacToe.Core.Input.Enums;
using TicTacToe.Core.User.Components;
using TicTacToe.Core.Utils;

namespace TicTacToe.Core.Input.Systems;

public class AssignInputSystem(InputDevices device)
    : IEcsInitSystem, IEcsRunSystem
{
    private readonly Rectangle WindowBounds = new(0, 0, 192, 192);

    private EcsFilter _filter;
    private EcsFilter _noInputPlayerFilter;
    private EcsPool<WaitForInput> _waitPool;
    private EcsPool<Control> _controlPool;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();

        _filter = world.Filter<Player>()
            .Inc<WaitForInput>()
            .Exc<Control>()
            .End();

        _noInputPlayerFilter = world.Filter<Player>()
            .Exc<Control>()
            .Exc<WaitForInput>()
            .End();

        _waitPool = world.GetPool<WaitForInput>();
        _controlPool = world.GetPool<Control>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _filter)
        {
            ControlMethod controlMethod = ControlMethod.Nothing;

            if (device.Mouse.IsJustPressed(MouseButton.Left) && WindowBounds.Contains(device.Mouse.MousePosition))
                controlMethod = ControlMethod.Mouse;
            else if (device.Pad1.IsJustPressed(Buttons.A))
                controlMethod = ControlMethod.GamePad1;
            else if (device.Pad2.IsJustPressed(Buttons.A))
                controlMethod = ControlMethod.GamePad2;

            if (controlMethod != ControlMethod.Nothing)
            {
                _waitPool.Del(entity);
                _controlPool.Add(entity, new(controlMethod));

                EntityGenerator.NewEntity()
                    .With(new EventMarker())
                    .With(new PlayerSelectedInput());

                SetOtherPlayerIfExists();
            }
        }
    }

    private void SetOtherPlayerIfExists()
    {
        foreach (var entity in _noInputPlayerFilter)
        {
            _waitPool.Add(entity);
            break;
        }
    }
}
