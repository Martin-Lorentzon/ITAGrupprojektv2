using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class ObjectHandler : MonoBehaviour
{
    public Transform cameraTransform;

    public GameObject translateGizmoPrefab;
    private GameObject translateGizmoInstance;


    public GameObject cliboardObjectPrefab;

    private LayerMask sceneLayer;
    private LayerMask blockLayer;

    public TimeSlider timeSlider;

    void Awake()
    {
        if (SceneInformation.selectedObjects == null)
            SceneInformation.selectedObjects = new List<GameObject>();

        // Scene Layer contains what is selectable
        sceneLayer = LayerMask.GetMask("Scene Asset", "Outlined");

        // Block Layer contains what the user expects to block selections, usually user interface
        blockLayer = LayerMask.GetMask("Arrow Gizmo");

        translateGizmoInstance = Instantiate(translateGizmoPrefab, Vector3.zero, Quaternion.identity);
        translateGizmoInstance.SetActive(SceneInformation.selectedObjects.Count > 0);
    }

    private void Start()
    {
        SceneInformation.moveSnapIncrement = 0f;
        SceneInformation.rotationSnapIncrement = 11.25f;
        SceneInformation.snapSpeed = 60f;

        timeSlider = GameObject.Find("TimeSliderControl").GetComponent<TimeSlider>();
    }
    public void ClearSelected()
    {
        Debug.Log("clear selected");
        SceneInformation.selectedObjects.Clear();
        OnSelectionChangedHit(new Vector3(0,0,0));
    }
    public void Paste()
    {
        string clipboard = GUIUtility.systemCopyBuffer;
        GameObject newObj = Instantiate(cliboardObjectPrefab, cameraTransform.position, Quaternion.identity);

        MeshFilter meshFilter = newObj.GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = newObj.GetComponent<MeshRenderer>();

        Mesh mesh = new Mesh();      // Mesh
        string name;    // Not used for anything


        // Paste Mesh Data
        CL3D.PasteModel(clipboard, true, out mesh, out name);

        // Update Mesh Filter
        meshFilter.mesh = mesh;
        meshFilter.mesh.RecalculateNormals();
        meshFilter.mesh.RecalculateBounds();

        // Add Mesh Collider
        MeshCollider meshCollider = newObj.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = mesh;
    }

    void Update()
    {
        SceneInformation.focusPoint = cameraTransform.position + (Vector3.up * 0.2f);

        bool paste = Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.V);

        if (paste)
        {


            string clipboard = GUIUtility.systemCopyBuffer;
            if (clipboard.StartsWith("CL3D_KEY"))
            {
                Paste();
            }
        }

        if (Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace))
        {
            List<GameObject> removeList = new List<GameObject>();
            foreach (GameObject removeObj in SceneInformation.selectedObjects)
            {
                switch (removeObj.tag)
                {
                    case ("Road Point"):
                        break;
                    default:
                        removeList.Add(removeObj);
                        break;
                }
            }
            foreach (GameObject destroyObj in removeList)
                Destroy(destroyObj);
            SceneInformation.selectedObjects.Clear();
            translateGizmoInstance.SetActive(SceneInformation.selectedObjects.Count > 0);
        }

            

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneInformation.selectedObjects.Clear();
            OnSelectionChangedHit(hits[0].point);
        }

        bool inSelectState = SceneInformation.ApplicationState == SceneInformation.AppState.Select;

        foreach (var hit in hits)
            if (blockLayer == (blockLayer | (1 << hit.transform.gameObject.layer)))
                return;

        try
        {
            if (inSelectState && Physics.Raycast(ray, out hits[0], Mathf.Infinity, sceneLayer))
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

                    OnSelectionChangedHit(hits[0].point);
                }
            }
        }
        catch(Exception ex)
        {
            //Debug.LogException(ex);
        }
    }

    private void LateUpdate()
    {
        bool inSelectState = SceneInformation.ApplicationState == SceneInformation.AppState.Select;

        if (!inSelectState)
            translateGizmoInstance.SetActive(false);
        else
            translateGizmoInstance.SetActive(SceneInformation.selectedObjects.Count > 0);
    }

    void ToggleSelectedObject(GameObject obj)
    {
        switch (obj.tag)
        {
            case ("Road"):
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (SceneInformation.selectedObjects.Contains(obj))
                        SceneInformation.selectedObjects.Remove(obj);
                    else
                        SceneInformation.selectedObjects.Add(obj);
                    return;
                }
                else
                {
                    Transform parent = obj.transform.parent;
                    Transform[] children = parent.GetComponentsInChildren<Transform>();     // Get all road prefab instances

                    bool allSelected = true;
                    foreach (Transform child in children)
                        if (!SceneInformation.selectedObjects.Contains(child.gameObject) && child.tag == "Road")
                            allSelected = false;


                    if (SceneInformation.selectedObjects.Contains(obj))
                    {
                        if (allSelected)
                            foreach (Transform child in children)
                            {
                                if (child.tag == "Road")
                                    SceneInformation.selectedObjects.Remove(child.gameObject);
                            }
                                
                        else
                            foreach (Transform child in children)
                            {
                                if (child.tag == "Road")
                                    SceneInformation.selectedObjects.Add(child.gameObject);
                            }
                    }
                    else
                        foreach (Transform child in children)
                        {
                            if (child.tag == "Road")
                                SceneInformation.selectedObjects.Add(child.gameObject);
                        }
                }
                
                break;
            default:
                if (SceneInformation.selectedObjects.Contains(obj))
                    SceneInformation.selectedObjects.Remove(obj);
                else
                    SceneInformation.selectedObjects.Add(obj);
                    timeSlider.printCurrentNumber(obj);
                break;

                
        }
    }

    void OnSelectionChangedHit(Vector3 hitpoint)
    {
        // Get all game objects in Scene Layer
        
            Collider[] colliders = Physics.OverlapSphere(hitpoint, 20000.0f, sceneLayer);
        

        // Iterate over the game objects
        foreach (Collider collider in colliders)
        {
            GameObject obj = collider.gameObject;

            if (SceneInformation.selectedObjects.Contains(obj))
            {
                obj.layer = 28;
                //try { obj.GetComponent<MeshRenderer>().materials[0].SetFloat("_Selected", 1f); }
                //catch (Exception ex) { /*Debug.LogException(ex);*/ }
            }
            else
            {
                obj.layer = 25;
                //try { obj.GetComponent<MeshRenderer>().materials[0].SetFloat("_Selected", 0f); }
                //catch (Exception ex) { /*Debug.LogException(ex);*/ }
            }
        }

        translateGizmoInstance.transform.position = hitpoint;
        translateGizmoInstance.SetActive(SceneInformation.selectedObjects.Count > 0);
    }
}