using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GearSystemBase : Photon.MonoBehaviour, IDeltaTime
{
    public GearBase parent { get; protected set; }
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

    public virtual void Initilize(GearBase parent, Transform mesh)
    {
        this.parent = parent;
    }

    public virtual void Initilize(GearBase parent, Camera cam, List<Material> mats)
    {
        this.parent = parent;
    }

    protected virtual void Update()
    {
        deltaTime = Tick.deltaTime;
    }
}
