using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoxelToggle : MonoBehaviour {


    public Toggle toggle;
    public Text text;
    public GameObject mesh;
    public Image image;

    public void SetData(GameObject _mesh, Color color, string _text)
    {
        mesh = _mesh;
        image.color = color;
        text.text = _text;
    }

    public void ToggleSwitched()
    {
        mesh.SetActive(toggle.isOn);
    }
}
