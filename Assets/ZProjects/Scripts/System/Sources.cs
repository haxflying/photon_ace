using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sources : MonoBehaviour {

    public static Sources instance;

    public List<TargetObject> targets = new List<TargetObject>();
    public WeapStorage weaps;

    private void Awake()
    {
        instance = this;
    }
}
