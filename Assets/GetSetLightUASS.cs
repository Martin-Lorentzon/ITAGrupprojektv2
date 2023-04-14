using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GetSetLightUASS : MonoBehaviour
{
    
    void Update()
    {
        UASS UASSComponent = gameObject.GetComponent<UASS>();
        UASSComponent.light = GameObject.Find("Directional Light").GetComponent<Light>();
    }
}
