using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public float playerBalance;
    public List<BusinessState> businesses = new List<BusinessState>();
}

[System.Serializable]
public class BusinessState
{
    public int level;
    public float progress;
    public bool upgrade1Purchased;
    public bool upgrade2Purchased;
}
