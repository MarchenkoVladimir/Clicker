using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Game Config")]
public class GameConfig : ScriptableObject
{
    public List<BusinessConfig> businesses;
}
