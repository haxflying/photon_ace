using UnityEngine;
using System.Collections;
using System;

public class WeapObjectBase : Photon.MonoBehaviour, IDeltaTime
{
    public GearBase parent { get; protected set; }
    public TargetObjectBase target { get; protected set; }

    protected float deltaTime, timeScale;

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

    public virtual void Initilize(GearBase parent, TargetObjectBase target)
    {
        this.parent = parent;
        this.target = target;
    }

    protected virtual void Update()
    {
        deltaTime = Tick.deltaTime;
    }
}
