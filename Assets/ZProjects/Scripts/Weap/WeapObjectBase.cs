using UnityEngine;
using System.Collections;
using System;

public class WeapObjectBase : Photon.MonoBehaviour, IDeltaTime
{
    public GearBase parent { get; protected set; }
    public TargetObjectBase target { get; protected set; }

    protected float deltaTime, timeScale;
    public virtual void SetTimeScale(float timeScale)
    {
        this.timeScale = timeScale;
        deltaTime *= timeScale;
    }

    public virtual void Initilize(GearBase parent, TargetObjectBase target, float deltaTime, float timeScale = 1f)
    {
        this.parent = parent;
        this.target = target;
        this.timeScale = timeScale;
        this.deltaTime = timeScale * deltaTime;
    }
}
