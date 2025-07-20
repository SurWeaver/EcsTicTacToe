using Leopotam.EcsLite;
using TicTacToe.Core.Events;

namespace TicTacToe.Core.Cleaning.Systems;

public class CleanEventSystem
    : IEcsInitSystem, IEcsRunSystem
{
    private EcsWorld _world;
    private EcsFilter _filter;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();

        _filter = _world.Filter<EventMarker>()
            .End();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _filter)
            _world.DelEntity(entity);
    }
}
