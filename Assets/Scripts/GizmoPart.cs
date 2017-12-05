using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoPart : MonoBehaviour {


    private MeshRenderer rend;
    private Material originalMat;
    private Material selectedMat;
    private Gizmo pg;
    public int selection;


	// Use this for initialization
	void Start () {
        rend = GetComponent<MeshRenderer>();
        originalMat = rend.material;
        pg = transform.parent.GetComponent<Gizmo>();
        selectedMat = pg.selectedMat;

    }

    void OnMouseDown()
    {
        pg.SetRotation(selection);
    }

    void OnMouseEnter()
    {
        rend.material = selectedMat;
    }
   
    void OnMouseExit()
    {
        rend.material = originalMat;
    }
}
