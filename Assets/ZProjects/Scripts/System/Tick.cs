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

    //input events
    public delegate void OnKeyboardEvent();
    public event OnKeyboardEvent OnDoubleClickA, OnDoubleClickD;

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


    private float? A_clicktime_0 = null, D_clicktime_0 = null;
    private void Update()
    {
        if(OnUpdate != null && !GamePause)
        {
            OnUpdate();
        }
        deltaTime = Time.deltaTime * TimeScale;

        //input event
        if(Input.GetKeyDown(KeyCode.A))
        {
            if(A_clicktime_0 == null)
            {
                A_clicktime_0 = Time.timeSinceLevelLoad;
            }
            else
            {
                if(Time.timeSinceLevelLoad - A_clicktime_0 < 0.3f)
                {
                    if (OnDoubleClickA != null)
                        OnDoubleClickA();
                }
                A_clicktime_0 = null;
                
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (D_clicktime_0 == null)
            {
                D_clicktime_0 = Time.timeSinceLevelLoad;
            }
            else
            {
                if (Time.timeSinceLevelLoad - D_clicktime_0 < 0.3f)
                {
                    if (OnDoubleClickD != null)
                        OnDoubleClickD();
                }
                D_clicktime_0 = null;
                
            }
        }
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
