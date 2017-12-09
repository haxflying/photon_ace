using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tick : MonoBehaviour {

    public delegate void OnUpdateDele();

    public static Tick instance;
    public static OnUpdateDele OnUpdate, OnFixedUpdate;
    public static bool GamePause;
    public static float TimeScale
    {
        get
        {
            return TimeScale;
        }
        set
        {
            TimeScale = Mathf.Min(1, value);
        }
    }

    private void Awake()
    {
        instance = this;
        GamePause = false;
    }

    private float count = 0;
    private void Update()
    {
        if(OnUpdate != null && !GamePause)
        {
            OnUpdate();
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
