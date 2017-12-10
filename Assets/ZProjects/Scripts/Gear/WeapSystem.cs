using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeapSystem : Photon.MonoBehaviour
{

    protected bool initilized = false;
    protected Transform std_trans, adv_trans_left, adv_trans_right;
    protected GearBase parent;
    protected LockSystem lockSys;
    protected Transform mesh;

    public virtual void Initilize(GearBase parent, Transform mesh)
    {
        this.parent = parent;
        this.mesh = mesh;
        std_trans = transform.ZFindChild("Std_Trans");
        adv_trans_left = transform.ZFindChild("Adv_left");
        adv_trans_right = transform.ZFindChild("Adv_right");

        Tick.OnUpdate += WeapFireUpdate;

        //init locksystem
        lockSys = gameObject.AddComponent<LockSystem>();
        lockSys.Initlize(mesh);
    }

    private float stdColdDownTime = 0f, advColdDownTime = 0f, currentTime = 0f;

	public virtual void WeapFireUpdate()
    {
        if (!photonView.isMine)
            return;

        currentTime += Tick.deltaTime;

        if(Input.GetMouseButton(0))
        {
            //std
            SingleWeapData weap = Sources.instance.weaps.GetWeap(WeapType.std_Test);
            if (weap != null)
            {
                if (currentTime >= stdColdDownTime)
                {
                    PhotonNetwork.Instantiate(weap.prefab.name, std_trans.position, std_trans.rotation, 0);
                    stdColdDownTime = currentTime + weap.reAttackTime;
                }
            }
            else
            {
                Debug.Log("std weap prefab is null");
            }

        }
        if (Input.GetMouseButton(1))
        {
            //adv
            SingleWeapData weap = Sources.instance.weaps.GetWeap(WeapType.adv_Test);
            if (weap != null)
            {
                if (currentTime >= advColdDownTime)
                {
                    MissileBase missile_l = PhotonNetwork.Instantiate(weap.prefab.name, adv_trans_left.position, adv_trans_left.rotation, 0).GetComponent<MissileBase>();
                    MissileBase missile_r = PhotonNetwork.Instantiate(weap.prefab.name, adv_trans_right.position, adv_trans_right.rotation, 0).GetComponent<MissileBase>();

                    missile_l.Initilize(parent, lockSys.currentTarget);
                    missile_r.Initilize(parent, lockSys.currentTarget);

                    advColdDownTime = currentTime + weap.reAttackTime;
                }
            }
            else
            {
                Debug.Log("std weap prefab is null");
            }
        }
    }
}
