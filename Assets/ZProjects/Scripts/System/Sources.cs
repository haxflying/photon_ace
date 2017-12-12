using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sources : MonoBehaviour {

    public static Sources instance;

    public List<TargetObject> targets = new List<TargetObject>();
    public List<PhotonPlayer> players = new List<PhotonPlayer>();
    public WeapStorage weaps;
    private void Awake()
    {
        instance = this;
    }    
    
    public TargetObject GetObject(int viewID)
    {
        return targets.Find((e) => e.photonView.viewID == viewID);
    }

}
