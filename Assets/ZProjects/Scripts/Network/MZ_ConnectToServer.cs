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

        //test
        if(Input.GetKeyDown(KeyCode.P))
        {
            foreach(PhotonPlayer player in PhotonNetwork.playerList)
            {
                print(player.ID + " " +  player.TagObject);
            }
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
        if (Sources.instance != null)
        {
            Sources.instance.players.Clear();
            print("inited players source");
        }
    }

    public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.LogError("Cause : " + cause);
    }

    public void OnJoinedRoom()
    {
        print("Joined");
        //print("cos 15 " + Mathf.Cos( 15f));
        GameObject gobject = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(PhotonNetwork.player.ID * 10f, 3f, 0), Quaternion.identity, 0);
        //TargetObject tobject = gobject.GetComponent<TargetObject>();
        //if (tobject != null)
        //    Sources.instance.targets.Add(tobject);
    }

    public void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        if(Sources.instance != null)
        {
            Sources.instance.players.Add(player);
            print(player.ID + " added ");
        }       
    }

    public void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        if (Sources.instance != null)
        {
            Sources.instance.players.Remove(player);
            print(player.ID + " leaved ");
        }
    }
}
