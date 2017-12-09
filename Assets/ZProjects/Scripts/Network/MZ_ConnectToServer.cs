using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MZ_ConnectToServer : Photon.MonoBehaviour {

    public GameObject playerPrefab;

    public bool AutoConnect = true;

    public byte Version = 1;

    private bool ConnectInUpdate = true;

    public virtual void Start()
    {
        PhotonNetwork.autoJoinLobby = false;
    }

    public virtual void Update()
    {
        if(ConnectInUpdate && AutoConnect && !PhotonNetwork.connected)
        {
            ConnectInUpdate = false;
            PhotonNetwork.ConnectUsingSettings(Version + "." + SceneManagerHelper.ActiveSceneBuildIndex);
        }
    }

    //callbacks
    public virtual void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public virtual void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public virtual void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 4 }, null);
    }

    public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.LogError("Cause : " + cause);
    }

    public void OnJoinedRoom()
    {
        print("Joined");
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(PhotonNetwork.player.ID * 2f, 2f, 0), Quaternion.identity, 0);
    }
}
