using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateGizmo : MonoBehaviour
{

    // For Visuals
    public MeshFilter[] arrowMeshFilters;
    public Mesh[] gizmoMeshes;


    // For Functionality
    public enum translateGizmoState {idle, translateX, translateY, translateZ, rotateY};
    public translateGizmoState gizmoState;

    public enum translateGizmoMode { move, scale};
    public translateGizmoMode gizmoMode;

    private int arrowGizmoLayerMask;
    private int mouseTrapLayerMask;

    private Vector3 currentPos;
    private Vector3 previousPos;

    private Vector3 lastMousePos = Vector3.zero;

    void Awake()
    {
        gizmoState = translateGizmoState.idle;
        gizmoMode = translateGizmoMode.move;

        // Arrow gizmo Layer mask
        arrowGizmoLayerMask = LayerMask.GetMask("Arrow Gizmo");

        // Mouse trap Layer mask
        mouseTrapLayerMask = LayerMask.GetMask("Mouse Trap");

        

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (gizmoMode == translateGizmoMode.move)
                gizmoMode = translateGizmoMode.scale;
            else
                gizmoMode = translateGizmoMode.move;
        }


        // Screen to World space Ray
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        // Hover over arrow
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, arrowGizmoLayerMask))
        {
            // Mouse trap Hits
            RaycastHit[] hits;
            hits = Physics.RaycastAll(ray, Mathf.Infinity, mouseTrapLayerMask);

            // Arrow Axis
            string axis = hit.transform.tag;

            switch (axis)   // Enter Translate states
            {
                case "X":
                    if (Input.GetMouseButtonDown(0))
                    {
                        foreach (RaycastHit mouseTrapHit in hits)
                            if (mouseTrapHit.transform.tag == "X")
                                previousPos = mouseTrapHit.point;

                        gizmoState = translateGizmoState.translateX;
                    }
                    break;

                case "Y":
                    if (Input.GetMouseButtonDown(0))
                    {
                        foreach (RaycastHit mouseTrapHit in hits)
                            if (mouseTrapHit.transform.tag == "Y")
                                previousPos = mouseTrapHit.point;

                        gizmoState = translateGizmoState.translateY;
                    }
                    break;

                case "Z":
                    if (Input.GetMouseButtonDown(0))
                    {
                        foreach (RaycastHit mouseTrapHit in hits)
                            if (mouseTrapHit.transform.tag == "Z")
                                previousPos = mouseTrapHit.point;

                        gizmoState = translateGizmoState.translateZ;
                    }
                    break;
            }
        }

        if (Input.GetKey(KeyCode.R))
            gizmoState = translateGizmoState.rotateY;


        switch (gizmoState)     // Translate gizmo States
        {
            case translateGizmoState.translateX:
                Translate("X", new Vector3(1f, 0f, 0f));

                if (Input.GetMouseButtonUp(0))
                    gizmoState = translateGizmoState.idle;
                break;

            case translateGizmoState.translateY:
                Translate("Y", new Vector3(0f, 1f, 0f));

                if (Input.GetMouseButtonUp(0))
                    gizmoState = translateGizmoState.idle;
                break;

            case translateGizmoState.translateZ:
                Translate("Z", new Vector3(0f, 0f, 1f));

                if (Input.GetMouseButtonUp(0))
                    gizmoState = translateGizmoState.idle;
                break;

            case translateGizmoState.rotateY:
                foreach (GameObject obj in SceneInformation.selectedObjects)
                    obj.transform.localEulerAngles += Vector3.up * ((lastMousePos.x - Input.mousePosition.x)/Screen.width) * 200f;

                if (Input.GetKeyUp(KeyCode.R))
                    gizmoState = translateGizmoState.idle;
                break;

            case translateGizmoState.idle:

                break;
        }
    }

    private void LateUpdate()
    {
        lastMousePos = Input.mousePosition;

        //For Visuals
        foreach (MeshFilter filt in arrowMeshFilters)
        {
            if (gizmoMode == translateGizmoMode.move)
                filt.sharedMesh = gizmoMeshes[0];
            else
                filt.sharedMesh = gizmoMeshes[1];
        }
    }

    void Translate(string tag, Vector3 axis)
    {
        
        // Screen to World space Ray
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Mouse trap Hits
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray, Mathf.Infinity, mouseTrapLayerMask);

        foreach (RaycastHit mouseTrapHit in hits)
        {
            if (mouseTrapHit.transform.tag == tag)
            {
                

                currentPos = mouseTrapHit.point;
                Vector3 distance = currentPos - previousPos;            // Get Distance

                if (gizmoMode == translateGizmoMode.move)
                    transform.position += Vector3.Scale(distance, axis);    // Move Gizmo by Distance

                foreach (GameObject obj in SceneInformation.selectedObjects)
                {

                    switch (gizmoMode)
                    {
                        case translateGizmoMode.move:
                            obj.transform.position += Vector3.Scale(distance, axis);    // Move Selected objects by Distance

                            RaycastHit hit;
                            if (Physics.Raycast(obj.transform.position + new Vector3(0, 5000, 0), Vector3.down, out hit, 10000, LayerMask.GetMask("Ground", "Water")))
                            {
                                if (tag != "Y")
                                {
                                    obj.transform.position = new Vector3(obj.transform.position.x, hit.point.y, obj.transform.position.z);
                                }
                            }
                            break;
                        case translateGizmoMode.scale:


                            //axis = Quaternion.Euler(0, , 0), 0) * Vector3.forward;
                            Vector3 v = Quaternion.AngleAxis(Mathf.DeltaAngle(obj.transform.eulerAngles.y, 0f), Vector3.up) * axis;

                            obj.transform.localScale += Vector3.Scale(Vector3.Scale(distance, axis) * 1f, new Vector3(-1f, 1f, -1f));
                            break;
                    }

                    
                }

                previousPos = mouseTrapHit.point;
                return;
            }
        }
        Debug.LogError("Mouse trap did not catch mouse. Please report to developer.");
        gizmoState = translateGizmoState.idle;
        return;
    }
}