using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    public float rotfollowSpeed = 15, posfollowSpeed = 8f;
    private Transform target;
    private float camDeltaTime, camTimeScale;
	public void Initilize(Transform target, float deltaTime, float timeScale = 1f)
    {
        this.target = target;
        this.camTimeScale = timeScale;
        this.camDeltaTime = deltaTime * timeScale;        
        Tick.OnUpdate += followUpdate;
    }

    void followUpdate()
    {
        if(target == null)
        {
            Tick.OnUpdate -= followUpdate;
            Destroy(gameObject);
            return;
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, camDeltaTime * rotfollowSpeed);

        
        transform.position = Vector3.Lerp(transform.position, target.position, camDeltaTime * posfollowSpeed);        
    }
}
