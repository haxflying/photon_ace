using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sources : MonoBehaviour {

    public static Sources instance;

    public List<TargetObjectBase> targets = new List<TargetObjectBase>();
    public List<PhotonPlayer> players = new List<PhotonPlayer>();
    public List<WeapObjectBase> weapObjects = new List<WeapObjectBase>();
    public WeapStorage weapsDatas;
    private void Awake()
    {
        instance = this;
    }    
    
    public TargetObjectBase GetObject(int viewID)
    {
        return targets.Find((e) => e.photonView.viewID == viewID);
    }

}
