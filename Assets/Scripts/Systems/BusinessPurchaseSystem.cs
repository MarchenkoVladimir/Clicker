using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public class BusinessPurchaseSystem : IEcsRunSystem
{
    private readonly GameConfig _config;
    
    public BusinessPurchaseSystem(GameConfig config)
    {
        _config = config;
    }
    
    public void Run(EcsSystems systems)
    {
        var world = systems.GetWorld();
        var playerFilter = world.Filter<Player>().End();

        ProcessEvents<LevelUpEvent>(world, _config, playerFilter);
        ProcessEvents<Upgrade1Event>(world, _config, playerFilter);
        ProcessEvents<Upgrade2Event>(world, _config, playerFilter);
    }

    private void ProcessEvents<T>(
        EcsWorld world, 
        GameConfig config, 
        EcsFilter playerFilter) where T : struct, IBusinessEvent
    {
        var eventPool = world.GetPool<T>();
        var businessPool = world.GetPool<Business>();
    
        var events = new List<int>();
        foreach (var eventEntity in world.Filter<T>().End())
        {
            events.Add(eventEntity);
        }

        foreach (var eventEntity in events)
        {
            ref var evt = ref eventPool.Get(eventEntity);

            if (!businessPool.Has(evt.BusinessEntity))
            {
                world.DelEntity(eventEntity);
                continue;
            }

            ref var business = ref businessPool.Get(evt.BusinessEntity);
            var businessConfig = config.businesses[business.configIndex];

            foreach (var playerEntity in playerFilter)
            {
                ref var player = ref world.GetPool<Player>().Get(playerEntity);
                
                if (typeof(T) == typeof(LevelUpEvent))
                {
                    float cost = IncomeCalculator.CalculateLevelUp(business, businessConfig);
                    if (player.balance >= cost)
                    {
                        player.balance -= cost;
                        business.level++;
                    }
                }
                else if (typeof(T) == typeof(Upgrade1Event))
                {
                    if (player.balance >= businessConfig.upgrade1.cost)
                    {
                        player.balance -= businessConfig.upgrade1.cost;
                        business.upgrade1Purchased = true;
                    }
                }
                else if (typeof(T) == typeof(Upgrade2Event))
                {
                    if (player.balance >= businessConfig.upgrade2.cost)
                    {
                        player.balance -= businessConfig.upgrade2.cost;
                        business.upgrade2Purchased = true;
                    }
                }
                break; 
            }

            world.DelEntity(eventEntity);
        }
    }
}

