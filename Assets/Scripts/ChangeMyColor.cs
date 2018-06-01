using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMyColor : MonoBehaviour {


    public float low = float.MinValue;
    public float high = float.MaxValue;
    public Color color;
    public InputField lowInput;
    public InputField highInput;
    public bool usable = false;
    public GameObject button;
    public Material mat;
    public bool isMaterial;



    public void PickColor()
    {
        Manager.instance.colorPanel.SetActive(true);
        Manager.instance.cPicker.GetComponent<ColorPicker>().SetCurrentGO(this.gameObject);
    }
    
    public void Create(float lo, float hi, Color c)
    {
        usable = false;
        SetColor(c);
        lowInput.text = lo.ToString();
        highInput.text = hi.ToString();
        SetRange();
    }
    
    public void SetColor(Color c)
    {
        color = new Color(c.r,c.g,c.b);
        if(isMaterial){
            mat.color = c;
        }
        else {
            button.GetComponent<Image>().color = c;
        }
    }

    public bool CanUse(float t)
    {
        
        return (t >= low && t <= high && usable) ? true : false;
    }

    public void SetRange()
    {

        float _low = low;
        low = Convert.ToSingle(lowInput.text);
        float _high = high;
        high = Convert.ToSingle(highInput.text);

        usable = this.transform.parent.GetComponent<ValueSetter>().SetValues(this);

        if (!usable)
        {
            highInput.text = _high.ToString();
            high = _high;
            lowInput.text = _low.ToString();
            low = _low;
        }
        
    }

    public void ResetValues()
    {
        highInput.text = high.ToString();
        lowInput.text = low.ToString();
    }
    
}
