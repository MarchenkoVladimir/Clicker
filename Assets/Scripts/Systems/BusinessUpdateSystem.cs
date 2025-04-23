using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BusinessUpdateSystem : IEcsRunSystem
{ 
    private readonly GameConfig _config;
    
    public BusinessUpdateSystem(GameConfig config)
    {
        _config = config;
    }
    
    public void Run(EcsSystems systems)
    {
        var world = systems.GetWorld();
        var filter = world.Filter<Business>().Inc<BusinessViewComponent>().End();

        foreach (var entity in filter)
        {
            ref var business = ref world.GetPool<Business>().Get(entity);
            ref var view = ref world.GetPool<BusinessViewComponent>().Get(entity);
            var businessConfig = _config.businesses[business.configIndex];
            UpdateView(ref view, ref business, businessConfig);
        }
    }

    private void UpdateView(ref BusinessViewComponent view, ref Business business, BusinessConfig config)
    {
        view.title.text = config.businessName;
        view.level.text = $"{business.level}";
        view.income.text = $"{IncomeCalculator.CalculateIncome(business, config)}$";
        view.levelCost.text = $"Цена: {IncomeCalculator.CalculateLevelUp(business, config)}$";
    
        UpdateUpgradeButton(ref view.upgrade1Button, ref view.upgrade1Label, ref view.upgrade1Income, ref view.upgrade1Cost,
            config.upgrade1, business.upgrade1Purchased);
    
        UpdateUpgradeButton(ref view.upgrade2Button, ref view.upgrade2Label, ref view.upgrade2Income, ref view.upgrade2Cost,
            config.upgrade2, business.upgrade2Purchased);
    }

    private void UpdateUpgradeButton(ref Button button, ref TextMeshProUGUI label, ref TextMeshProUGUI income, ref TextMeshProUGUI cost, 
        UpgradeConfig upgrade, bool isPurchased)
    {
        label.text = $"{upgrade.upgradeName}";
        income.text = $"Доход + {upgrade.multiplier}%";
        cost.text = isPurchased ? "Куплено" : $"Цена: {upgrade.cost}$";
        button.interactable = !isPurchased;
    }
}
