using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon;
public class NetworkTools
{
    public static GameObject ZInstantiate<T>(string prefabName, Vector3 position, Quaternion rotation, byte group = 0) where T : WeapObjectBase
    {
        GameObject go = PhotonNetwork.Instantiate(prefabName, position, rotation, group);
        Sources.instance.weapObjects.Add(go.GetComponent<T>());
        return go;
    }

    public static void ZDestroy<T>(GameObject go) where T : WeapObjectBase
    {
        if (Sources.instance.weapObjects.Contains(go.GetComponent<T>()))
            Sources.instance.weapObjects.Remove(go.GetComponent<T>());
        PhotonNetwork.Destroy(go);
    }
}
