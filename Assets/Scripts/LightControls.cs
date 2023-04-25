using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;



public class LightControls : MonoBehaviour
{
    public GameObject sun;
    float sunAngle;
    public Material skyBox;
    float dayExposure;
    float nightExposure;
    Vector3 north;
    float sunOffset;
    public GameObject cam;
    public GameObject northArrow;
    RectTransform arrowTransform;
    public Slider sunCycleSlider;
    public Slider northSlider;

    void Start()
    {
        sunAngle = sun.transform.eulerAngles.x;
        dayExposure = 1f;
        nightExposure = 0.2f;
        north = sun.transform.right;
        sunOffset = sun.transform.eulerAngles.y;
        sun.transform.eulerAngles = new Vector3(sunAngle, sunOffset + 180, 0f);
        sunOffset = sun.transform.eulerAngles.y;
        sunCycleSlider.onValueChanged.AddListener(delegate { CycleSun(); });
        northSlider.onValueChanged.AddListener(delegate { OffsetNorth(); });
        
        northSlider.value = sunOffset;
        
        arrowTransform = northArrow.GetComponent<RectTransform>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(sun.transform.position, north);
    }

    void Update()
    {      

        if(Input.GetKey(KeyCode.X))
        {
            CycleSun();
        }       
          
        if (Input.GetKey(KeyCode.Y))
        {
            OffsetNorth();
        }
     
        PointNorth();
    }

    // rotates UI arrow to always point north like a compass needle
    void PointNorth()
    {
        float relAngle = Vector3.SignedAngle(north.normalized, cam.transform.right.normalized, Vector3.up);

        arrowTransform.eulerAngles = new Vector3(0f, 0f, relAngle + 90);
    }

    // Rotates sun around y, sets new north direction 
    void OffsetNorth()
    {
        

        sunOffset = northSlider.value;
        sun.transform.eulerAngles = new Vector3(sunAngle, sunOffset + 180 , 0f);

        //if (sunOffset > 359)
        //{
        //    sunOffset = 0;
        //    sun.transform.eulerAngles = new Vector3(sunAngle, sunOffset + 180f, 0f);
        //}

        north =  sun.transform.right;
    }

    // Sets directional light to slider value

    void CycleSun()
    {
        sunAngle = sunCycleSlider.value;
        sun.transform.eulerAngles = new Vector3(sunAngle, sunOffset, 0f);

        if (sunAngle < -15f || sunAngle > 185f)
        {
            RenderSettings.skybox.SetFloat("_Exposure", nightExposure);
            sun.SetActive(false);

        }
        else
        {
            RenderSettings.skybox.SetFloat("_Exposure", dayExposure);
            sun.SetActive(true);
        }
    }


}
