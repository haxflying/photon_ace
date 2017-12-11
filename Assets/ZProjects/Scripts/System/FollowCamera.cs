using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    public float rotfollowSpeed = 15, posfollowSpeed = 8f;
    private Transform target;
	public void Initilize(Transform target)
    {
        this.target = target;
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
        transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, Tick.deltaTime * rotfollowSpeed);

        
        transform.position = Vector3.Lerp(transform.position, target.position, Tick.deltaTime * posfollowSpeed);        
    }
}
