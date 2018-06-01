using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueSetter : MonoBehaviour {

	public bool SetValues(ChangeMyColor current)
    {

        // Check to see if self values are usable
        float currLow = current.low;
        float currHigh = current.high;
        if (currHigh < currLow) return false;

        // Now check against the other options
        // Obtain all of the child game objects to make checking simpler
        int childCount = transform.childCount;
        ChangeMyColor[] options = new ChangeMyColor[childCount];


        for (int i=0; i < childCount; ++i){
            options[i] = transform.GetChild(i).GetComponent<ChangeMyColor>();
        }


        bool usable = true;

        // Since we are setting a new value, we need to check the value ranges to make sure that they do not overlap
        for (int i = 0; i < childCount; ++i)
        {
            if (options[i] != current)
            {
                ChangeMyColor c = options[i];
                c.ResetValues();
                if (currLow < c.high && currHigh >= c.high) {
                    //Debug.Log("1. False in " + i + " " + currLow + " " + c.low + " " + currHigh + " " + c.high);
                    usable = false;
                }
                else if (currLow < c.low && currHigh > c.low)
                {
                    //Debug.Log("2. False in " + i + " " + currLow + " " + c.low + " " + currHigh + " " + c.high);
                    usable = false;
                }
                else if (currLow >= c.low && currHigh <= c.low)
                {
                    //Debug.Log("3. False in " + i + " " + currLow + " " + c.low + " " + currHigh + " " + c.high);
                    usable = false;
                }
            }
        }

        return usable;
    }
}
