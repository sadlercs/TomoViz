using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo : MonoBehaviour {



    public Transform camTrans;
    public Material selectedMat;


    public void SetRotation(int s)
    {
        float x = 0f, y = 0f;

        switch (s)
        {
            case 0: x = -270; y = 0; break; // y-up
            case 1: x = -90; y = 0; break; // y-down
            case 2: x = 0; y = -90; break; // x-right
            case 3: x = 0; y = -270; break; // x-left
            case 4: x = 0; y = 180; break; // z-forward
            case 5: x = 0; y = 0; break; // z-back
        }

        camTrans.GetComponent<DragMouseOrbit>().SetRotation(x, y);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newRot = -camTrans.localRotation.eulerAngles;
        this.transform.localRotation = Quaternion.Euler(newRot.x, newRot.y, newRot.z);
    }
}
