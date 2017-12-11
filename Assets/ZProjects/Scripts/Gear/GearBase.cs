using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearBase : TargetObject {


    public float minSpeed = 0, maxSpeed = 30f, acclerate = 10f, rotScale = 1f;
    protected float currentSpeed;

	public bool Inited { get; private set; }
    public Transform camPos { get; private set; }

    private TextMesh nickName;
    private Transform Trans, rollTrans;
    private Camera cam;
    private List<Material> GearMats = new List<Material>();

    private bool isStop;
    private float rotZSpeed = 250f, horizontalSpeed = 3f;
    private float currentRotZ = 0f, currentHorizontalSpeed = 0f;
    protected virtual void Awake()
    {
        Inited = false;
        Initilize();
    }

    public void Initilize()
    {
        if (Inited)
            return;

        //name init
        nickName = GetComponentInChildren<TextMesh>();
        nickName.text = "ID : " + photonView.viewID;
        if (photonView.isMine && PhotonNetwork.isMasterClient)
            nickName.text += "(Master)";

        gameObject.name = "Gear " + nickName.text;

        //transform init
        Trans = transform.Find("MeshTransform");
        camPos = Trans.Find("CamPos");
        rollTrans = Trans.Find("RollTransform");

        cam = GetComponentInChildren<Camera>();
        if(cam != null)
        {
            cam.gameObject.name = "Cam:" + nickName.text;
            cam.transform.SetParent(null);
            cam.gameObject.AddComponent<FollowCamera>().Initilize(camPos);
        }
        isStop = true;

        //system init
        gameObject.AddComponent<WeapSystem>().Initilize(this, rollTrans);

        foreach(Renderer rd in GetComponentsInChildren<Renderer>())
        {
            GearMats.Add(rd.material);
        }
        gameObject.AddComponent<SkillSystem>().Initilize(this, cam, GearMats);

        //network init
        if (photonView.isMine)
        {
            Tick.OnUpdate += MovementUpdate;
        }
        else
        {
            cam.gameObject.SetActive(false);
        }

        //add to source
        if(!Sources.instance.targets.Contains(this))
        {
            Sources.instance.targets.Add(this);
        }
        print("gear inited");
        Inited = true;
    }

    public virtual void MovementUpdate()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            isStop = !isStop;
        }

        if (isStop)
            return;

        if(Input.GetKey(KeyCode.Space))
        {
            currentSpeed += acclerate * Tick.deltaTime;
        }
        else
        {
            currentSpeed -= acclerate * Tick.deltaTime * 0.3f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            currentSpeed -= acclerate * Tick.deltaTime;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);
        transform.position += Trans.forward * currentSpeed * Tick.deltaTime;

        Vector3 screenSize = new Vector3(Screen.width, Screen.height, 0);
        Vector3 mousePos = Input.mousePosition;

        mousePos = mousePos - screenSize / 2f;
        Trans.Rotate(new Vector3(-mousePos.y, mousePos.x, 0) * Tick.deltaTime * rotScale);

        float roll = rollTrans.localRotation.eulerAngles.z > 180 ?
            360f - rollTrans.localRotation.eulerAngles.z : rollTrans.localRotation.eulerAngles.z;


        if (Input.GetKey(KeyCode.A))
        {
            if (roll <= 80f)
            {
                currentRotZ = rotZSpeed * Tick.deltaTime;
                currentHorizontalSpeed = Mathf.Lerp(currentHorizontalSpeed, -horizontalSpeed, 5f * Tick.deltaTime);
                rollTrans.localRotation *= Quaternion.Euler(new Vector3(0, 0, currentRotZ));
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (roll <= 80f)
            {
                currentRotZ = -rotZSpeed * Tick.deltaTime;
                currentHorizontalSpeed = Mathf.Lerp(currentHorizontalSpeed, horizontalSpeed, 5f * Tick.deltaTime);
                rollTrans.localRotation *= Quaternion.Euler(new Vector3(0, 0, currentRotZ));
            }
        }
        else
        {
            rollTrans.localRotation = Quaternion.Lerp(rollTrans.localRotation, Quaternion.identity, 3f * Tick.deltaTime);
            currentHorizontalSpeed = Mathf.Lerp(currentHorizontalSpeed, 0f, Tick.deltaTime);
        }          

        //print("roll " + roll);       
        transform.position += Trans.right * currentHorizontalSpeed * Tick.deltaTime;
        //print(Mathf.Abs(currentHorizontalSpeed) / horizontalSpeed);
        //print(camPos.localPosition);
        rollTrans.localPosition = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(currentHorizontalSpeed > 0 ? 2.5f : -2.5f, 0, 0),
            Mathf.Abs(currentHorizontalSpeed) / horizontalSpeed);
    }
}
