using UnityEngine;
using System.Collections;
using System;

public class MissileBase : WeapObjectBase
{
    //test
    public GameObject meshMine, meshOther, psMine, psOther;


    public float translateSpeed = 50f, rotScale = 2f;

    public override void Initilize(GearBase parent, TargetObjectBase target, float deltaTime, float timeScale = 1f)
    {
        base.Initilize(parent, target, deltaTime, timeScale);

        Tick.OnUpdate += MissileUpdate;
        StartCoroutine(AutoDestroy(10f));

        if(photonView.isMine)
        {
            meshOther.SetActive(false);
        }
        else
        {
            meshMine.SetActive(false);
        }
    }

    protected IEnumerator AutoDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Tick.OnUpdate -= MissileUpdate;
        if(photonView.isMine)
            PhotonNetwork.Destroy(photonView);
    }

    protected virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            int index_parent = parent.photonView.viewID;
            int index_target = target.photonView.viewID;
            stream.Serialize(ref index_parent);
            stream.Serialize(ref index_target);
        }
        else
        {
            int index_parent = 0;
            int index_target = 0;
            stream.Serialize(ref index_parent);
            stream.Serialize(ref index_target);
            if (index_parent != -1)
            {
                parent = Sources.instance.GetObject(index_parent) as GearBase;
            }
            if (index_target != -1)
            {
                target = Sources.instance.GetObject(index_target);
            }
        }
    }

    protected virtual void MissileUpdate()
    {
        if (target == null)
        {
            transform.position += (transform.forward * translateSpeed * deltaTime);
        }
        else
        {
            transform.position += transform.forward * translateSpeed * deltaTime;
            Quaternion lookRot = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, rotScale * deltaTime);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MissileBase>())
            return;

        TargetObjectBase gear = other.transform.root.GetComponent<TargetObjectBase>();
        if (gear != null)
        {
            if (gear != parent)
            {
                //print(parent.photonView.viewID + " hited gear ");
                print(parent.gameObject.name + " hited gear ");
                print(PhotonView.Get(gear.gameObject).viewID);
            }
            else
                return;
        }
        else if (gear == null)
        {
            print("hited normal obj " + other.name);            
        }

        Tick.OnUpdate -= MissileUpdate;
        StartCoroutine(AutoDestroy(1f));

        photonView.RPC("Explode", PhotonTargets.All);
    } 

    [PunRPC]
    protected void Explode()
    {
        if(photonView.isMine)
        {
            psMine.SetActive(true);
        }
        else
        {
            psOther.SetActive(true);
        }
    }
}
