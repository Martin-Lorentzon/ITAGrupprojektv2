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

            case translateGizmoState.idle:

                break;
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
                transform.position += Vector3.Scale(distance, axis);    // Move Gizmo by Distance

                foreach (GameObject obj in SceneInformation.selectedObjects)
                    obj.transform.position += Vector3.Scale(distance, axis);    // Move Selected objects by Distance

                previousPos = mouseTrapHit.point;
                return;
            }
        }
        Debug.LogError("Mouse trap did not catch mouse. Please report to developer.");
        gizmoState = translateGizmoState.idle;
        return;
    }
}