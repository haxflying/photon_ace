using UnityEngine;
using System.Collections;

public class MissileBase : Photon.MonoBehaviour
{
    //test
    public GameObject meshMine, meshOther, psMine, psOther;


    public float translateSpeed = 50f, rotScale = 2f;
    GearBase parent;
    TargetObject target;

    public virtual void Initilize(GearBase parent, TargetObject target)
    {
        this.parent = parent;
        this.target = target;
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

    protected virtual void MissileUpdate()
    {
        if (target == null)
        {
            transform.position += (transform.forward * translateSpeed * Tick.deltaTime);
        }
        else
        {
            transform.position += transform.forward * translateSpeed * Tick.deltaTime;
            Quaternion lookRot = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, rotScale * Tick.deltaTime);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MissileBase>())
            return;

        TargetObject gear = other.transform.root.GetComponent<TargetObject>();
        if (gear != null)
        {
            if (gear != parent)
                print("hited gear " + PhotonView.Get(gear.gameObject).viewID);
            else
                return;
        }
        else if (gear == null)
        {
            print("hited " + other.name);            
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
