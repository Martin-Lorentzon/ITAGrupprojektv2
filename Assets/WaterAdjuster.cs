using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAdjuster : MonoBehaviour
{

    ImpostTerrain ip;
    private void Start()
    {
        ip = FindObjectOfType<ImpostTerrain>();
    }

    public void setHeight(float adjust)
    {
        transform.position = new Vector3(0, (adjust * ip.strength *200) - 1.01f, 0);
    }
}
