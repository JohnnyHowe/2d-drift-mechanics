using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrackSection : MonoBehaviour
{

    public Curve[] curves;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Validate() {
        OnValidate();
    }

    void OnValidate() {
        for (int i = 0; i < curves.Length - 1; i++) {
            curves[i].p3 = curves[i + 1].p0;
        }
    }
}


[Serializable]
public struct Curve {
    public Vector3 p0;
    public Vector3 p1;
    public Vector3 p2;
    public Vector3 p3;
}