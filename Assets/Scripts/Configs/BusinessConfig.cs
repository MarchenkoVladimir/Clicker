using UnityEngine;

[CreateAssetMenu(menuName = "Game/Business Config")]
public class BusinessConfig : ScriptableObject
{
    public string businessName;
    public float baseIncome;
    public float baseCost;
    public float incomeDelay;
    public UpgradeConfig upgrade1;
    public UpgradeConfig upgrade2;
    public bool isPurchased;
}

