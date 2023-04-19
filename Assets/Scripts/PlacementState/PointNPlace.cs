using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointNPlace:Updater
{
    public Camera cam;
    public Transform planePos;
    GameObject something = null;
    bool scaling = false;
    
    public PointNPlace(GameObject _model, Camera _cam)
    {
        something = _model;
        something.transform.localScale = Vector3.one;
        cam = _cam;
        //cam.orthographic = true;
    }

    public PointNPlace(GameObject _model, Camera _cam, Transform _transform)
    {
        something = _model;
        something.transform.localScale = Vector3.one;
        something.tag = "Object";
        cam = _cam;
        planePos = _transform;
        cam.orthographic = true;
    }


    public override bool Updating()
    {
        Vector3 planPos = cam.transform.position - Vector3.up;
        if (planePos != null)
            planPos = planePos.position;

        Plane plan = new Plane(Vector3.up, planPos);
        
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        float enter = 0f;
        if(plan.Raycast(ray,out enter))
        {
            Vector3 point = ray.GetPoint(enter);

            if(!scaling)
            something.transform.position = point;
        }

        if(scaling)
                something.transform.localScale += new Vector3 (0f,Input.GetAxis("Mouse Y")*0.5f,0f);


        if (Input.GetButtonDown("Fire1"))
        {
            if (!scaling)
            {
                scaling = true;
                return false;
            }
            something = null;
            cam.orthographic = false;

            return true;
        }
            Debug.Log("fire");
        return false;
    }
}