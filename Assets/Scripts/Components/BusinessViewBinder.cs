using Leopotam.EcsLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BusinessViewBinder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private TextMeshProUGUI _income;
    [SerializeField] private Slider _progressBar;
    [SerializeField] private TextMeshProUGUI _levelCost;
    [SerializeField] private Button _levelUpButton;
    [SerializeField] private TextMeshProUGUI _upgrade1Label;
    [SerializeField] private TextMeshProUGUI _upgrade1Income;
    [SerializeField] private TextMeshProUGUI _upgrade1Cost;
    [SerializeField] private Button _upgrade1Button;
    [SerializeField] private TextMeshProUGUI _upgrade2Label;
    [SerializeField] private TextMeshProUGUI _upgrade2Income;
    [SerializeField] private TextMeshProUGUI _upgrade2Cost;
    [SerializeField] private Button _upgrade2Button;

    public void Initialize(EcsWorld world, int configIndex)
    {
        var businessPool = world.GetPool<Business>();
        var businessFilter = world.Filter<Business>().End();
        foreach (var entity in businessFilter)
        {
            ref var business = ref businessPool.Get(entity);
            if (business.configIndex == configIndex)
            {
                ref var view = ref world.GetPool<BusinessViewComponent>().Add(entity);
                view.viewObject = gameObject;
                view.title = _title;
                view.level = _level;
                view.income = _income;
                view.progressBar = _progressBar;
                view.levelCost = _levelCost;
                view.levelUpButton = _levelUpButton;
                view.upgrade1Label = _upgrade1Label;
                view.upgrade1Income = _upgrade1Income;
                view.upgrade1Cost = _upgrade1Cost;
                view.upgrade1Button = _upgrade1Button;
                view.upgrade2Label = _upgrade2Label;
                view.upgrade2Income = _upgrade2Income;
                view.upgrade2Cost = _upgrade2Cost;
                view.upgrade2Button = _upgrade2Button;
                
                _levelUpButton.onClick.AddListener(() => 
                {
                    var eventEntity = world.NewEntity();
                    ref var evt = ref world.GetPool<LevelUpEvent>().Add(eventEntity);
                    evt.BusinessEntity = entity;
                });

                _upgrade1Button.onClick.AddListener(() => 
                {
                    var eventEntity = world.NewEntity();
                    ref var evt = ref world.GetPool<Upgrade1Event>().Add(eventEntity);
                    evt.BusinessEntity = entity;
                });

                _upgrade2Button.onClick.AddListener(() => 
                {
                    var eventEntity = world.NewEntity();
                    ref var evt = ref world.GetPool<Upgrade2Event>().Add(eventEntity);
                    evt.BusinessEntity = entity;
                });
            }
        }
    }
}
