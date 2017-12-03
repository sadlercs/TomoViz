using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MinimumSlider : Slider
{
    public MaximumSlider other;

    protected override void Set(float input, bool sendCallback)
	{
		float newValue = input;

		if (wholeNumbers)
		{
			newValue = Mathf.Round(newValue);
		}
        if (other != null)
        {
            if (newValue >= other.value)
            {
                return;
            }
        }
		base.Set(input, sendCallback);
	}
}
