using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearBase : Photon.MonoBehaviour {


    public float minSpeed = 0, maxSpeed = 30f, acclerate = 10f, rotScale = 1f;
    protected float currentSpeed;

	public bool Inited { get; private set; }
    public Transform camPos { get; private set; }

    private TextMesh nickName;
    private Transform mesh;
    private Camera cam;

    private bool isStop;
    private float rotThreshold = 10, rotZSpeed = 10f, horizontalSpeed = 3f;
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

        //transform init
        mesh = transform.Find("MeshTransform");
        camPos = mesh.Find("CamPos");
        cam = GetComponentInChildren<Camera>();
        if(cam != null)
        {
            cam.gameObject.name = "Cam:" + nickName.text;
            cam.transform.SetParent(null);
            cam.gameObject.AddComponent<FollowCamera>().Initilize(camPos);
        }
        isStop = true;

        //system init
        gameObject.AddComponent<WeapSystem>().Initilize(this, mesh);

        //network init
        if (photonView.isMine)
        {
            Tick.OnUpdate += MovementUpdate;
        }
        else
        {
            cam.enabled = false;
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
            currentSpeed += acclerate * Time.deltaTime;
        }
        else
        {
            currentSpeed -= acclerate * Time.deltaTime * 0.3f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            currentSpeed -= acclerate * Time.deltaTime;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);
        transform.position += mesh.forward * currentSpeed * Time.deltaTime;

        Vector3 screenSize = new Vector3(Screen.width, Screen.height, 0);
        Vector3 mousePos = Input.mousePosition;

        mousePos = mousePos - screenSize / 2f;
        mesh.Rotate(new Vector3(-mousePos.y, mousePos.x, 0) * Time.deltaTime * rotScale);

        if(Input.GetKey(KeyCode.A))
        {
            currentRotZ += rotZSpeed * Time.deltaTime;
            currentHorizontalSpeed = Mathf.Lerp(currentHorizontalSpeed, -horizontalSpeed, Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            currentRotZ -= rotZSpeed * Time.deltaTime;
            currentHorizontalSpeed = Mathf.Lerp(currentHorizontalSpeed, horizontalSpeed, Time.deltaTime);
        }
        else
        {
            currentRotZ = Mathf.Lerp(currentRotZ, 0f, rotZSpeed * Time.deltaTime);
            currentHorizontalSpeed = Mathf.Lerp(currentHorizontalSpeed, 0f, Time.deltaTime);
        }
        
        currentRotZ = Mathf.Clamp(currentRotZ, -rotThreshold, rotThreshold);
        mesh.rotation *= Quaternion.Euler(new Vector3(0, 0, currentRotZ));
        transform.position += mesh.up * currentHorizontalSpeed * Time.deltaTime;
    }
}
