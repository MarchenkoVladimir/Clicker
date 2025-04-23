using Leopotam.EcsLite;
public class PlayerUpdateSystem : IEcsRunSystem
{
    public void Run(EcsSystems systems)
    {
        var world = systems.GetWorld();
        var playerFilter = world.Filter<Player>().End();
        var filter = world.Filter<Player>().Inc<PlayerViewComponent>().End();

        foreach (var entity in filter)
        {
            ref var player = ref world.GetPool<Player>().Get(playerFilter.GetRawEntities()[0]);
            UpdateBalance(ref world.GetPool<PlayerViewComponent>().Get(entity), player.balance);
        }
    }

    private void UpdateBalance(ref PlayerViewComponent view, float balance)
    {
        view.balance.text = balance.ToString();
    }
}
