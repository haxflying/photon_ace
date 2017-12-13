using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : WeapObjectBase {

    public float translateSpeed = 30f;

    public override void Initilize(GearBase parent, TargetObjectBase target, float deltaTime, float timeScale = 1f)
    {
        base.Initilize(parent, target, deltaTime, timeScale);

        StartCoroutine(AutoDestroy(10f));
        Tick.OnUpdate += ProjectileUpdate;
    }

    protected virtual void ProjectileUpdate()
    {
        transform.position += (transform.forward * translateSpeed * deltaTime);
    }

    protected IEnumerator AutoDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Tick.OnUpdate -= ProjectileUpdate;
        PhotonNetwork.Destroy(photonView);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ProjectileBase>())
            return;

        TargetObjectBase gear = other.transform.root.GetComponent<TargetObjectBase>();
        if (gear != null && !photonView.isMine)
        {
            print("hited gear " + gear.name);
           
        }
        else if(gear == null)
        {
            print("hited " + other.name);
            Tick.OnUpdate -= ProjectileUpdate;
            StartCoroutine(AutoDestroy(1f));
        }
    }

}
