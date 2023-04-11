using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectHandler : MonoBehaviour
{

    public GameObject translateGizmoPrefab;
    private GameObject translateGizmoInstance;

    private LayerMask sceneLayer;
    private LayerMask blockLayer;

    void Awake()
    {
        if (SceneInformation.selectedObjects == null)
            SceneInformation.selectedObjects = new List<GameObject>();

        // Scene Layer contains what is selectable
        sceneLayer = LayerMask.GetMask("Scene Asset");

        // Block Layer contains what the user expects to block selections, usually user interface
        blockLayer = LayerMask.GetMask("Arrow Gizmo");
    }

    private void Start()
    {
        translateGizmoInstance = Instantiate(translateGizmoPrefab, Vector3.zero, Quaternion.identity);
        translateGizmoInstance.SetActive(SceneInformation.selectedObjects.Count > 0);

        SceneInformation.moveSnapIncrement = 0.5f;
        SceneInformation.rotationSnapIncrement = 11.25f;
        SceneInformation.snapSpeed = 60f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace))
        {
            for (int t = 0; t < SceneInformation.selectedObjects.Count; t++)
                Destroy(SceneInformation.selectedObjects[t]);

            SceneInformation.selectedObjects.Clear();
            translateGizmoInstance.SetActive(SceneInformation.selectedObjects.Count > 0);

            translateGizmoInstance.SetActive(SceneInformation.selectedObjects.Count > 0);
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        foreach (var hit in hits)
            if (blockLayer == (blockLayer | (1 << hit.transform.gameObject.layer)))
                return;

        try
        {
            if (Physics.Raycast(ray, out hits[0], Mathf.Infinity, sceneLayer))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    GameObject obj = hits[0].transform.gameObject;

                    if (Input.GetKey(KeyCode.LeftShift))
                        ToggleSelectedObject(obj);
                    else
                    {
                        if (!SceneInformation.selectedObjects.Contains(obj) || SceneInformation.selectedObjects.Count > 1)
                            SceneInformation.selectedObjects.Clear();
                        ToggleSelectedObject(obj);
                    }

                    OnSelectionChangedHit(hits[0]);
                }
            }
        }
        catch(Exception ex) { Debug.LogException(ex); }
    }

    void ToggleSelectedObject(GameObject obj)
    {
        if (SceneInformation.selectedObjects.Contains(obj))
            SceneInformation.selectedObjects.Remove(obj);
        else
            SceneInformation.selectedObjects.Add(obj);
    }

    void OnSelectionChangedHit(RaycastHit hit)
    {
        // Get all game objects in Scene Layer
        Collider[] colliders = Physics.OverlapSphere(hit.point, 20000.0f, sceneLayer);

        // Iterate over the game objects
        foreach (Collider collider in colliders)
        {
            GameObject obj = collider.gameObject;

            if (SceneInformation.selectedObjects.Contains(obj))
            {
                try { obj.GetComponent<MeshRenderer>().materials[0].SetFloat("_Selected", 1f); }
                catch (Exception ex) { Debug.LogException(ex); }
            }
            else
            {
                try { obj.GetComponent<MeshRenderer>().materials[0].SetFloat("_Selected", 0f); }
                catch (Exception ex) { Debug.LogException(ex); }
            }
        }

        translateGizmoInstance.transform.position = hit.transform.position;
        translateGizmoInstance.SetActive(SceneInformation.selectedObjects.Count > 0);
    }

    void OnSelectionChanged()
    {

    }
}