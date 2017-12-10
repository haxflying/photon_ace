using UnityEngine;
using System.Collections;


[CreateAssetMenu(fileName = "WeapData", menuName = "ACE/WeapData", order = 2)]
public class SingleWeapData : ScriptableObject
{
    public GameObject prefab;
    public WeapType type;

    public float reAttackTime = 0.2f;
}
