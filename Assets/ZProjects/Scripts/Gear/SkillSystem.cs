using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillSystem : Photon.MonoBehaviour
{
    
    GearBase parent;
    Camera cam;
    List<Material> mats;
    Vector3 bulletTimeOriginal;
    public virtual void Initilize(GearBase parent, Camera cam, List<Material> mats)
    {
        this.parent = parent;
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

        if (Input.GetKeyDown(KeyCode.C))
        {
            //tmp
            bulletTimeOriginal = transform.position;
            //ActiveBulletTime();
            photonView.RPC("ActiveBulletTime", PhotonTargets.AllViaServer, "hello");
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
    private void ActiveBulletTime(Vector3 pos)
    {
        foreach (Material mat in mats)
        {
            mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        }
        cam.GetComponent<BulletTimeEffect>().ActiveBulletTime(pos, true);
    }
}
