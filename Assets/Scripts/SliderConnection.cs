using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderConnection : MonoBehaviour {

    public MinimumSlider min;
    public MaximumSlider max;

    // Use this for initialization
    void Start () {
        min.other = max;
        max.other = min;
    }
	
}
