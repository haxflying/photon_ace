using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : Photon.MonoBehaviour {

    public float translateSpeed = 30f;

    protected virtual void Awake()
    {
        StartCoroutine(AutoDestroy(10f));
        Tick.OnUpdate += ProjectileUpdate;
    }

    protected virtual void ProjectileUpdate()
    {
        transform.position += (transform.forward * translateSpeed * Tick.deltaTime);
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

        TargetObject gear = other.transform.root.GetComponent<TargetObject>();
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
