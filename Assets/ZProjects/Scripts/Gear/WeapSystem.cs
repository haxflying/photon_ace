using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeapSystem : GearSystemBase
{
    public LockSystem lockSystem { get; protected set; }

    protected bool initilized = false;
    protected Transform std_trans, adv_trans_left, adv_trans_right;    
    protected Transform mesh;

    private float stdColdDownTime = 0f, advColdDownTime = 0f, currentTime = 0f;

	public virtual void WeapFireUpdate()
    {
        if (photonView != null && !photonView.isMine)
            return;

        currentTime += deltaTime;

        if(Input.GetMouseButton(0))
        {
            //std
            SingleWeapData weap = Sources.instance.weapsDatas.GetWeap(WeapType.std_Test);
            if (weap != null)
            {
                if (currentTime >= stdColdDownTime)
                {
                    ProjectileBase proj = NetworkTools.ZInstantiate<ProjectileBase>(weap.prefab.name, std_trans.position, std_trans.rotation, 0)
                        .GetComponent<ProjectileBase>();
                    proj.Initilize(parent, null, deltaTime);
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
            SingleWeapData weap = Sources.instance.weapsDatas.GetWeap(WeapType.adv_Test);
            if (weap != null)
            {
                if (currentTime >= advColdDownTime)
                {
                    MissileBase missile_l = NetworkTools.ZInstantiate<MissileBase>(weap.prefab.name, adv_trans_left.position, adv_trans_left.rotation, 0).GetComponent<MissileBase>();
                    MissileBase missile_r = NetworkTools.ZInstantiate<MissileBase>(weap.prefab.name, adv_trans_right.position, adv_trans_right.rotation, 0).GetComponent<MissileBase>();

                    missile_l.Initilize(parent, lockSystem.currentTarget, deltaTime);
                    missile_r.Initilize(parent, lockSystem.currentTarget, deltaTime);

                    advColdDownTime = currentTime + weap.reAttackTime;
                }
            }
            else
            {
                Debug.Log("std weap prefab is null");
            }
        }
    }

    public override void Initilize(GearBase parent, Transform mesh, float deltaTime, float timeScale = 1)
    {
        base.Initilize(parent, mesh, deltaTime, timeScale);
        this.mesh = mesh;
        std_trans = transform.ZFindChild("Std_Trans");
        adv_trans_left = transform.ZFindChild("Adv_left");
        adv_trans_right = transform.ZFindChild("Adv_right");

        Tick.OnUpdate += WeapFireUpdate;

        //init locksystem
        lockSystem = gameObject.AddComponent<LockSystem>();
        lockSystem.Initilize(parent, mesh, deltaTime);
        
    }

}
