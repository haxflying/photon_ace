using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GearSystemBase : Photon.MonoBehaviour, IDeltaTime
{
    public GearBase parent { get; protected set; }
    protected float deltaTime, timeScale;
    public virtual void SetTimeScale(float timeScale)
    {
        this.timeScale = timeScale;
        deltaTime *= timeScale;
    }

    public virtual void Initilize(GearBase parent, Transform mesh, float deltaTime, float timeScale = 1f)
    {
        this.timeScale = timeScale;
        this.deltaTime = deltaTime * timeScale;
        this.parent = parent;
    }

    public virtual void Initilize(GearBase parent, Camera cam, List<Material> mats)
    {
        this.parent = parent;
    }
}
