using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeapList", menuName = "ACE/WeapList", order = 1)]
public class WeapStorage : ScriptableObject {

    public List<SingleWeapData> weaps = new List<SingleWeapData>();

    public SingleWeapData GetWeap(WeapType type)
    {
        return weaps.Find((a) => a.type == type);
    }
}

public enum WeapType
{
    std_Test,adv_Test
}
