using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SkillSystem : GearSystemBase, IPunObservable
{    
    Camera cam;
    List<Material> mats;
    Vector3 bulletTimeOriginal;

    public override void Initilize(GearBase parent, Camera cam, List<Material> mats)
    {
        base.Initilize(parent, cam, mats);
        this.cam = cam;
        this.mats = mats;
        parent.photonView.ObservedComponents.Add(this);
        bulletTimeOriginal = Vector3.zero;
        Tick.OnUpdate += skillUpdate;
    }

    void skillUpdate()
    {
        if (!photonView.isMine)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //tmp
            bulletTimeOriginal = transform.position;
            cam.GetComponent<BulletTimeEffect>().ActiveBulletTime(bulletTimeOriginal, true);

            photonView.RPC("ActiveBulletTime", PhotonTargets.AllViaServer, bulletTimeOriginal);
            //Tick.instance.SlowDown();
        }
    }

    protected virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting)
        {
            Vector3 btOrigin = bulletTimeOriginal;
            stream.Serialize(ref btOrigin);
        }
        else
        {
            Vector3 btOrigin = Vector3.zero;
            stream.Serialize(ref btOrigin);
            bulletTimeOriginal = btOrigin;
        }
    }

    [PunRPC]
    private void ActiveBulletTime(Vector3 pos, PhotonMessageInfo info)
    {
        foreach (Material mat in mats)
        {
            mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        }
        foreach (TargetObjectBase target in Sources.instance.targets)
        {
            if (target.GetComponent<GearBase>() && target.photonView != parent.photonView)
            {
                target.GetComponent<GearBase>().Cam.GetComponent<BulletTimeEffect>().ActiveBulletTime(bulletTimeOriginal, true);
                Tick.instance.SlowDownAllTargetExcept(info.sender);
                print(target.photonView.viewID + " at " + pos + " by " + info.sender);
            }
        }            
    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

}
