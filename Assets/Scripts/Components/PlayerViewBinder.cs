using Leopotam.EcsLite;
using TMPro;
using UnityEngine;

public class PlayerViewBinder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _income;

    public void Initialize(EcsWorld world, float income)
    {
        var entity = world.NewEntity();
        ref var view = ref world.GetPool<PlayerViewComponent>().Add(entity);
        view.balance = _income;
        
        ref var balance = ref world.GetPool<Player>().Add(entity);
        balance.balance = income;
    }
}
