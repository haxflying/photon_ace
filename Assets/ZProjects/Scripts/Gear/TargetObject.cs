using UnityEngine;
using System.Collections;

public class TargetObject : Photon.MonoBehaviour
{

    public float MaxHealth = 100;
    
    public float CurrentHealth { get; private set; }
}
