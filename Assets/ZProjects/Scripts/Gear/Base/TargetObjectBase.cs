using UnityEngine;
using System.Collections;
using System;

public class TargetObjectBase : Photon.MonoBehaviour, IDeltaTime
{
    public float timeScale;
    public float MaxHealth = 100;
    
    public float CurrentHealth { get; private set; }

    protected float deltaTime
    {
        get
        {
            return Tick.deltaTime * timeScale;
        }
    }    

    protected virtual void Awake()
    {
        timeScale = 1f;
    }

    public virtual void SetTimeScale(float timeScale)
    {
        this.timeScale = timeScale;
    }
}
