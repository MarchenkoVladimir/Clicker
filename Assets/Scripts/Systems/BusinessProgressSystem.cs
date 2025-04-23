using Leopotam.EcsLite;
using UnityEngine;

public class BusinessProgressSystem : IEcsRunSystem
{
    private readonly GameConfig _config;
    
    public BusinessProgressSystem(GameConfig config)
    {
        _config = config;
    }
    
    public void Run(EcsSystems systems)
    {
        var world = systems.GetWorld();
        var filter = world.Filter<Business>().Inc<BusinessViewComponent>().End();
        var playerFilter = world.Filter<Player>().End();

        foreach (var entity in filter)
        {
            ref var business = ref world.GetPool<Business>().Get(entity); 
            if (business.level == 0) continue;

            var businessConfig = _config.businesses[business.configIndex];
            business.progress += Time.deltaTime / businessConfig.incomeDelay;
            
            if (business.progress >= 1f)
            {
                business.progress = 0f;
                ref var player = ref world.GetPool<Player>().Get(playerFilter.GetRawEntities()[0]);
                player.balance += IncomeCalculator.CalculateIncome(business, businessConfig);

            }

            UpdateProgressBar(ref world.GetPool<BusinessViewComponent>().Get(entity), business.progress);
        }
    }

    

    private void UpdateProgressBar(ref BusinessViewComponent view, float progress)
    {
        view.progressBar.value = progress;
    }
}
