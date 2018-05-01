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
        usable = false;
        if (lowInput.text.Length == 0 || lowInput.text == String.Empty || highInput.text.Length == 0 || highInput.text == String.Empty) {
            lowInput.text = low.ToString(); highInput.text = high.ToString(); return;
        }

        float _low = Convert.ToSingle(lowInput.text);
        float _high = Convert.ToSingle(highInput.text);

        if (_low > _high)
        {
            lowInput.text = low.ToString(); highInput.text = high.ToString(); return;
        }
       

        int listCnt = Manager.instance.colorList.Count;

        for (int i=0; i< listCnt; ++i)
        {
            if (Manager.instance.colorList[i] != this.gameObject && Manager.instance.colorList[i].GetComponent<ChangeMyColor>().usable == true)
            {
                float lowCheck = Manager.instance.colorList[i].GetComponent<ChangeMyColor>().low;
                float highCheck = Manager.instance.colorList[i].GetComponent<ChangeMyColor>().high;
                if (_low == lowCheck)
                {
                    lowInput.text = low.ToString(); return;
                }
                if (_low < lowCheck && high > lowCheck)
                {
                    lowInput.text = low.ToString(); return;
                }
                if (_low > lowCheck && _low < highCheck)
                {
                    lowInput.text = low.ToString(); return;
                }

                // Check High
                if (_high == highCheck)
                {
                    highInput.text = high.ToString(); return;
                }

                if (_high > highCheck && _low < highCheck)
                {
                    highInput.text = high.ToString(); return;
                }
                if (_high < highCheck && _high > lowCheck)
                {
                    highInput.text = high.ToString(); return;
                }
            }
        }
        
        
        highInput.text = _high.ToString();
        high = _high;
        lowInput.text = _low.ToString();
        low = _low;

        usable = true;

        // Change colors in the shell as well
        
    }

    
}
