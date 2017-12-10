using UnityEngine;
using System.Collections;

public class LockSystem : Photon.MonoBehaviour
{
    public TargetObject currentTarget { get; private set; }

    bool locked;
    Transform mesh;

    public void Initlize(Transform mesh)
    {
        locked = false;
        this.mesh = mesh;
        Tick.OnUpdate += LockUpdate;
    }

    protected void LockUpdate()
    {
        if (!photonView.isMine)
            return;

        if (locked)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                locked = false;
                currentTarget = null;
                print("Unlocked");
            }
        }
        else
        {
            foreach (TargetObject target in Sources.instance.targets)
            {
                //print("scaning " + target.name);
                if (target == this.GetComponent<TargetObject>())
                    continue;

                Vector3 target2me = target.transform.position - transform.position;
                target2me = Vector3.Normalize(target2me);
                //print(target.name + "'s cos value is " + Vector3.Dot(target2me, mesh.forward));
                float dotResult = Vector3.Dot(target2me, mesh.forward);
                if (dotResult > 0f && Mathf.Abs(dotResult) >= 0.966f)
                {
                    //inside the lock cone
                    currentTarget = target;
                    locked = true;

                    print("Locked target " + target.name);
                    break;
                }
            }
        }
    }
}
