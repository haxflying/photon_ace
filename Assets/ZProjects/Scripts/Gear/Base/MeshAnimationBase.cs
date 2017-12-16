using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshAnimationBase : Photon.MonoBehaviour, IDeltaTime
{
    public void SetTimeScale(float timeScale)
    {
        throw new NotImplementedException();
    }

    public virtual void SetAnimationBySpeed(float speed)
    {

    }
}
