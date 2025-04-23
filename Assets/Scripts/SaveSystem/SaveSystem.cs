using UnityEngine;
using Leopotam.EcsLite;

public static class SaveSystem
{
    private const string SaveKey = "BusinessGameSave";

    public static void SaveGame(EcsWorld world, GameConfig config)
    {
        var saveData = new SaveData();
        var businessPool = world.GetPool<Business>();
        
        var playerFilter = world.Filter<Player>().End();
        foreach (var entity in playerFilter)
        {
            saveData.playerBalance = world.GetPool<Player>().Get(entity).balance;
            break;
        }

        var businessFilter = world.Filter<Business>().End();
        foreach (var entity in businessFilter)
        {
            ref var business = ref businessPool.Get(entity);
            saveData.businesses.Add(new BusinessState
            {
                level = business.level,
                progress = business.progress,
                upgrade1Purchased = business.upgrade1Purchased,
                upgrade2Purchased = business.upgrade2Purchased
            });
        }

        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public static void LoadGame(EcsWorld world, GameConfig config)
    {
        if (!PlayerPrefs.HasKey(SaveKey)) return;

        string json = PlayerPrefs.GetString(SaveKey);
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);

        var playerEntity = world.NewEntity();
        ref var player = ref world.GetPool<Player>().Add(playerEntity);
        player.balance = saveData.playerBalance;

        for (int i = 0; i < config.businesses.Count; i++)
        {
            var entity = world.NewEntity();
            ref var business = ref world.GetPool<Business>().Add(entity);
            
            if (i < saveData.businesses.Count)
            {
                var savedState = saveData.businesses[i];
                business.configIndex = i;
                business.level = savedState.level;
                business.progress = savedState.progress;
                business.upgrade1Purchased = savedState.upgrade1Purchased;
                business.upgrade2Purchased = savedState.upgrade2Purchased;
                business.isUnlocked = savedState.level > 0;
            }
            else
            {
                business.configIndex = i;
                business.level = i == 0 ? 1 : 0;
                business.isUnlocked = i == 0;
            }
        }
    }
}