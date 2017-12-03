using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MaximumSlider : Slider
{
	public MinimumSlider other;

	protected override void Set(float input, bool sendCallback)
	{
		float newValue = input;

		if (wholeNumbers)
		{
			newValue = Mathf.Round(newValue);
		}
        if (other != null)
        {
            if (newValue <= other.value)
            {
                return;
            }
        }
		base.Set(input, sendCallback);
	}
}
