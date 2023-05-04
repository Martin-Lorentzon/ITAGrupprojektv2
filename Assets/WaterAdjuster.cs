using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterAdjuster : MonoBehaviour
{
    float ad;
    float st;
    ImpostTerrain ip;
    private void Start()
    {
        ip = FindObjectOfType<ImpostTerrain>();
        st = ip.strength;
    }
    public void Readjust()
    {
        st = ip.strength;
        transform.position = new Vector3(0, (ad * st * 200) - 1.1f, 0);
    }

    public void SetHeight(float adjust)
    {
        ad = adjust;
        transform.position = new Vector3(0, (ad * st *200) - 1.1f, 0);
    }
}
