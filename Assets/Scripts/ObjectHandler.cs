using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectHandler : MonoBehaviour
{

    public List<GameObject> selectedObjects;


    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (Input.GetKey(KeyCode.LeftShift))            // Add/Remove from selection
                {
                    if (hit.transform.tag != "Immovable")
                    {
                        if (selectedObjects.Contains(hit.transform.gameObject))
                            selectedObjects.Remove(hit.transform.gameObject);
                        else
                            selectedObjects.Add(hit.transform.gameObject);
                    }
                }
                else
                {
                    if (hit.transform.tag != "Immovable")       // Select/Deselect single object
                    {
                        if (selectedObjects.Contains(hit.transform.gameObject))
                            selectedObjects.Remove(hit.transform.gameObject);
                        else
                        {
                            selectedObjects.Clear();
                            selectedObjects.Add(hit.transform.gameObject);
                        }
                    }
                }
            }
        }
    }
}