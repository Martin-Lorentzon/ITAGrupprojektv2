using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateGizmo : MonoBehaviour
{

    public enum translateGizmoState { idle, translateX, translateY, translateZ };
    public translateGizmoState gizmoState;

    private int arrowGizmoLayerMask;
    private int mouseTrapLayerMask;

    private Vector3 currentPos;
    private Vector3 previousPos;

    void Awake()
    {
        gizmoState = translateGizmoState.idle;

        // Arrow gizmo Layer mask
        arrowGizmoLayerMask = LayerMask.GetMask("Arrow Gizmo");

        // Mouse trap Layer mask
        mouseTrapLayerMask = LayerMask.GetMask("Mouse Trap");
    }

    void Update()
    {
        // Screen to World space Ray
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        // Hover over arrow
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, arrowGizmoLayerMask))
        {
            string axis = hit.transform.tag;
            

            switch (axis)       // Enter Translate states
            {
                case "X":
                    if (Input.GetMouseButtonDown(0))
                    {
                        // Mouse trap Hits
                        RaycastHit[] mouseTrapHits;
                        mouseTrapHits = Physics.RaycastAll(ray, Mathf.Infinity, mouseTrapLayerMask);

                        gizmoState = translateGizmoState.translateX;
                        foreach(RaycastHit mouseTrapHit in mouseTrapHits)
                        {
                            if (mouseTrapHit.transform.tag == "X")
                                previousPos = mouseTrapHit.point;
                        }   
                    }
                    break;

                case "Y":
                    if (Input.GetMouseButtonDown(0))
                        gizmoState = translateGizmoState.translateY;
                    break;

                case "Z":
                    if (Input.GetMouseButtonDown(0))
                        gizmoState = translateGizmoState.translateZ;
                    break;
            }
        }


        switch (gizmoState)     // Translate gizmo States
        {
            case translateGizmoState.translateX:

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, mouseTrapLayerMask))  // Check for Mouse trap
                {
                    currentPos = hit.point;
                    float distanceX = currentPos.x - previousPos.x;         // Get X Distance
                    transform.position += new Vector3(distanceX, 0f, 0f);   // Move Translate gizmo
                    previousPos = hit.point;
                }
                else
                {
                    Debug.LogError("Mouse trap did not catch mouse. Please report to developer.");
                    gizmoState = translateGizmoState.idle;
                }

                if (Input.GetMouseButtonUp(0))
                gizmoState = translateGizmoState.idle;
                break;

            case translateGizmoState.translateY:

                if (Input.GetMouseButtonUp(0))
                    gizmoState = translateGizmoState.idle;
                break;

            case translateGizmoState.translateZ:

                if (Input.GetMouseButtonUp(0))
                    gizmoState = translateGizmoState.idle;
                break;

            case translateGizmoState.idle:

                break;
        }
    }
}