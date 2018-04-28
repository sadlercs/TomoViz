using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReshade : MonoBehaviour {



    private void OnDisable()
    {
        StartCoroutine(Manager.instance.Reshade(false));
    }
}
