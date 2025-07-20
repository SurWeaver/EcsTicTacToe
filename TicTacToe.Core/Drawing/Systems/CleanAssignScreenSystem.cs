using Leopotam.EcsLite;
using TicTacToe.Core.Drawing.Components;
using TicTacToe.Core.Input.Components;

namespace TicTacToe.Core.Drawing.Systems;

public class CleanAssignScreenSystem
    : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private EcsFilter _eventFilter;
    private EcsFilter _uiFilter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();

        _eventFilter = _world.Filter<PlayerSelectedInput>()
            .End();

        _uiFilter = _world.Filter<AssignInputUI>()
            .End();
    }

    public void Run(IEcsSystems systems)
    {
        if (_eventFilter.GetEntitiesCount() > 0)
            foreach (var uiEntity in _uiFilter)
                _world.DelEntity(uiEntity);
    }
}
