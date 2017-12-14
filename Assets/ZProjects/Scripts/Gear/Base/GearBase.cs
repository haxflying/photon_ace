using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
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
    

    private bool isStop, isRolling;
    private float rotZSpeed = 400f, horizontalSpeed = 3f;
    private float currentRotZ = 0f, currentHorizontalSpeed = 0f;
    protected override void Awake()
    {
        base.Awake();
        Inited = false;
        isRolling = false;
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
            print("cam deltaTime " + deltaTime);
        }
        isStop = true;

        //system init
        weapSystem = gameObject.AddComponent<WeapSystem>();
        weapSystem.Initilize(this, rollTrans);

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

        //event register
        Tick.instance.OnDoubleClickA += DoubleClickAEvent;
        Tick.instance.OnDoubleClickD += DoubleClickDEvent;
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

        if (isRolling) // can't rotate during rolling
            return;

        Vector3 screenSize = new Vector3(Screen.width, Screen.height, 0);
        Vector3 mousePos = Input.mousePosition;

        mousePos = mousePos - screenSize / 2f;
        Trans.Rotate(new Vector3(-mousePos.y, mousePos.x, 0) * deltaTime * rotScale);

        float roll = rollTrans.localRotation.eulerAngles.z > 180 ?
            360f - rollTrans.localRotation.eulerAngles.z : rollTrans.localRotation.eulerAngles.z;


        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            if (roll <= 80f)
            {
                currentRotZ = rotZSpeed * deltaTime;
                currentHorizontalSpeed = Mathf.Lerp(currentHorizontalSpeed, -horizontalSpeed, 5f * deltaTime);
                rollTrans.localRotation *= Quaternion.Euler(new Vector3(0, 0, currentRotZ));
            }
        }
        else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
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

    protected virtual void DoubleClickAEvent()
    {
        print("double click A");
        Vector3 target = Vector3.forward * 360f;
        //rollTrans.DOLocalRotate(target, 1f, RotateMode.LocalAxisAdd);
        //StartCoroutine(rollTest());
    }

    protected virtual void DoubleClickDEvent()
    {
        print("double click D");
        Vector3 target = rollTrans.localRotation.eulerAngles + Vector3.forward * -360f;
        rollTrans.DOLocalRotate(target, 1f, RotateMode.FastBeyond360);
    }

    IEnumerator rollTest()
    {
        WaitForSeconds wait = new WaitForSeconds(0.001f);
        isRolling = true;
        for (int i = 0; i < 72; i++)
        {
            rollTrans.Rotate(new Vector3(0,0,5), Space.Self);
            yield return wait;
        }
        isRolling = false;
        yield return null;
    }
}
