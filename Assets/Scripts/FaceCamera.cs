using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {

    private Transform cam;
	// Use this for initialization
	void Start () {
		
        if(Manager.instance != null)
        {
            cam = Manager.instance.cam.transform;
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (cam != null)
        {
            transform.LookAt(cam);
        }
        else
        {
            if (Manager.instance != null)
            {
                cam = Manager.instance.cam.transform;
            }
        }


	}
}
