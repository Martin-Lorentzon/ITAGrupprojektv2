using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateGizmo : MonoBehaviour
{

    public enum translateGizmoState { idle, translateX, translateY, translateZ, rotateY};
    public translateGizmoState gizmoState;

    private int arrowGizmoLayerMask;
    private int mouseTrapLayerMask;

    private Vector3 currentPos;
    private Vector3 previousPos;

    private Vector3 lastMousePos = Vector3.zero;

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