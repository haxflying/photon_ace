using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tick : Photon.MonoBehaviour {

    public delegate void OnUpdateDele();

    public static Tick instance;
    public static OnUpdateDele OnUpdate, OnFixedUpdate;
    public static bool GamePause;
    public static float TimeScale;
    public static float deltaTime;

    public void SlowDownAllTargetExcept(PhotonPlayer target, float timeScale = 0.1f, bool includingOthersWeap = true)
    {
        foreach(TargetObjectBase t in Sources.instance.targets)
        {
            if(t.photonView.ownerId != target.ID)
            {
                t.SetTimeScale(timeScale);
                print("Slowed " + t.gameObject.name);
            }
        }

        if (includingOthersWeap)
        {
            foreach (WeapObjectBase wo in Sources.instance.weapObjects)
            {
                if (wo.parent.photonView.ownerId != target.ID)
                {
                    wo.SetTimeScale(timeScale);
                }
            }
        }
    }

    private void Awake()
    {
        instance = this;
        TimeScale = 1f;
    }

    private void Update()
    {
        if(OnUpdate != null && !GamePause)
        {
            OnUpdate();
        }
        deltaTime = Time.deltaTime * TimeScale;
    }

    private void FixedUpdate()
    {
        if (OnFixedUpdate != null)
            OnFixedUpdate();
    }

    private void OnDestroy()
    {
        if (OnUpdate != null)
        {
            System.Delegate[] ar = OnUpdate.GetInvocationList();
            foreach (System.Delegate a in ar)
            {
                OnUpdate -= a as OnUpdateDele;
            }
        }

        if (OnFixedUpdate != null)
        {
            System.Delegate[] ar = OnFixedUpdate.GetInvocationList();
            foreach (System.Delegate a in ar)
            {
                OnFixedUpdate -= a as OnUpdateDele;
            }
        }
    }
}
