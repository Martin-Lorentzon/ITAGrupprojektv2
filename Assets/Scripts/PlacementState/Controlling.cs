using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controlling : MonoBehaviour
{
    public Camera cam;
    public GameObject planeObj;
    [SerializeField]
    bool busy = false;
    PointNPlace placing;

    [SerializeField]
    LayerMask objLayer;

    [Space]

    [SerializeField]
    PlaceObjList objList;

    public ref bool Busy { get { return ref busy; } }

    void Start()
    {
        if (cam == null)
            cam = Camera.main;
        //placing = new PointNPlace(placeObj, cam, planeObj.transform);
        objList = GetComponent<PlaceObjList>();
    }

    void Update()
    {
        if (busy) return;

        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit, 100f,objLayer))
            {
                if (hit.transform.tag == "Object")
                {
                    ChangeOptions opt = hit.collider.gameObject.GetComponentInParent<ChangeOptions>();
                    opt.Obj = opt.gameObject;
                     busy = opt.StartChoices();
                }
            }
        }

    }
}