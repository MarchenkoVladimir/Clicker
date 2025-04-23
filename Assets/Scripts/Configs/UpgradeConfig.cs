using UnityEngine;

[CreateAssetMenu(menuName = "Game/Upgrade Config")]
public class UpgradeConfig : ScriptableObject
{
    public string upgradeName;
    public float cost;
    public float multiplier;
}
