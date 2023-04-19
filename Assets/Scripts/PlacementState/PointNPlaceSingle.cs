using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointNPlaceSingle : MonoBehaviour
{
    public Camera cam;
    [SerializeField]
    GameObject objModel = null, something = null;
    [SerializeField]
    bool scaled = true, scaling = false, busy = false;

    [SerializeField]
    LayerMask mask;

    private void Start()
    {
        if (cam == null)
            cam = Camera.main;
        this.enabled = false;
    }

    void Update()
    {
        if (!scaled)
        {
            scaled = Updating();
        }
        else
            this.enabled = false;
    }

    public void PlaceOut()
    {
        this.enabled = true;
        objModel.transform.localScale = Vector3.one;
        scaled = false;
        scaling = false;
        something = Instantiate(objModel);
    }


    public bool Updating()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray,out hit, 20f,mask))
        {
            Vector3 point = hit.point;

            if (!scaling)
                something.transform.position = point;
        }

        if (scaling)
            something.transform.localScale += new Vector3(0f, Input.GetAxis("Mouse Y") * 0.5f, 0f);


        if (Input.GetButtonDown("Fire1"))
        {
            if (!scaling)
            {
                scaling = true;
                return false;
            }
            something = null;

            return true;
        }
        return false;
    }
}