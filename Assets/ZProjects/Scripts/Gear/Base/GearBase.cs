using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearBase : TargetObjectBase {


    public float minSpeed = 0, maxSpeed = 30f, acclerate = 10f, rotScale = 1f;
    protected float currentSpeed;

    protected LockSystem lockSystem;
    protected SkillSystem skillSystem;
    protected WeapSystem weapSystem;

	public bool Inited { get; private set; }
    public Transform camPos { get; private set; }
    public PhotonPlayer localPlayer { get; private set; }
    public Camera Cam
    {
        get
        {
            return cam;
        }
    }

    private TextMesh nickName;
    private Transform Trans, rollTrans;
    private Camera cam;
    private List<Material> GearMats = new List<Material>();
    

    private bool isStop;
    private float rotZSpeed = 250f, horizontalSpeed = 3f;
    private float currentRotZ = 0f, currentHorizontalSpeed = 0f;
    protected override void Awake()
    {
        base.Awake();
        Inited = false;
        Initilize();
    }

    public void Initilize()
    {
        if (Inited)
            return;

        //player init
        if (photonView.isMine)
        {
            localPlayer = PhotonNetwork.player;
            localPlayer.TagObject = this;
        }

        //name init
        nickName = GetComponentInChildren<TextMesh>();
        nickName.text = "ID : " + photonView.viewID;
        if (photonView.isMine && PhotonNetwork.isMasterClient)
            nickName.text += "(Master)";

        gameObject.name = "Gear " + nickName.text;

        //print(photonView.ownerId + " " + localPlayer.ID);

        //transform init
        Trans = transform.Find("MeshTransform");
        camPos = Trans.Find("CamPos");
        rollTrans = Trans.Find("RollTransform");

        cam = GetComponentInChildren<Camera>();
        if(cam != null)
        {
            cam.gameObject.name = "Cam:" + nickName.text;
            cam.transform.SetParent(null);
            cam.gameObject.AddComponent<FollowCamera>().Initilize(camPos, deltaTime);
        }
        isStop = true;

        //system init
        weapSystem = gameObject.AddComponent<WeapSystem>();
        weapSystem.Initilize(this, rollTrans, deltaTime);

        foreach (Renderer rd in GetComponentsInChildren<Renderer>())
        {
            GearMats.Add(rd.material);
        }
        skillSystem = gameObject.AddComponent<SkillSystem>();
        skillSystem.Initilize(this, cam, GearMats);

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

    public override void SetTimeScale(float timeScale)
    {
        base.SetTimeScale(timeScale);
        //Bullet time just effects weap system now
        weapSystem.SetTimeScale(timeScale);
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
            currentSpeed += acclerate * deltaTime;
        }
        else
        {
            currentSpeed -= acclerate * deltaTime * 0.3f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            currentSpeed -= acclerate * deltaTime;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);
        transform.position += Trans.forward * currentSpeed * deltaTime;

        Vector3 screenSize = new Vector3(Screen.width, Screen.height, 0);
        Vector3 mousePos = Input.mousePosition;

        mousePos = mousePos - screenSize / 2f;
        Trans.Rotate(new Vector3(-mousePos.y, mousePos.x, 0) * deltaTime * rotScale);

        float roll = rollTrans.localRotation.eulerAngles.z > 180 ?
            360f - rollTrans.localRotation.eulerAngles.z : rollTrans.localRotation.eulerAngles.z;


        if (Input.GetKey(KeyCode.A))
        {
            if (roll <= 80f)
            {
                currentRotZ = rotZSpeed * deltaTime;
                currentHorizontalSpeed = Mathf.Lerp(currentHorizontalSpeed, -horizontalSpeed, 5f * deltaTime);
                rollTrans.localRotation *= Quaternion.Euler(new Vector3(0, 0, currentRotZ));
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (roll <= 80f)
            {
                currentRotZ = -rotZSpeed * deltaTime;
                currentHorizontalSpeed = Mathf.Lerp(currentHorizontalSpeed, horizontalSpeed, 5f * deltaTime);
                rollTrans.localRotation *= Quaternion.Euler(new Vector3(0, 0, currentRotZ));
            }
        }
        else
        {
            rollTrans.localRotation = Quaternion.Lerp(rollTrans.localRotation, Quaternion.identity, 3f * deltaTime);
            currentHorizontalSpeed = Mathf.Lerp(currentHorizontalSpeed, 0f, deltaTime);
        }          

        //print("roll " + roll);       
        transform.position += Trans.right * currentHorizontalSpeed * deltaTime;
        //print(Mathf.Abs(currentHorizontalSpeed) / horizontalSpeed);
        //print(camPos.localPosition);
        rollTrans.localPosition = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(currentHorizontalSpeed > 0 ? 2.5f : -2.5f, 0, 0),
            Mathf.Abs(currentHorizontalSpeed) / horizontalSpeed);
    }
}
