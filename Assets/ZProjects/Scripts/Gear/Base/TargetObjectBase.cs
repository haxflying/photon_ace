using UnityEngine;
using System.Collections;
using System;

public class TargetObjectBase : Photon.MonoBehaviour, IDeltaTime
{
    
    public float MaxHealth = 100;
    
    public float CurrentHealth { get; private set; }

    protected float timeScale;
    protected float deltaTime;

    protected virtual void Awake()
    {
        timeScale = 1f;
        deltaTime = Tick.deltaTime;
    }

    public virtual void SetTimeScale(float timeScale)
    {
        this.timeScale = timeScale;
        deltaTime *= timeScale;
    }

    protected virtual void Update()
    {
        deltaTime = Tick.deltaTime;
    }
}
