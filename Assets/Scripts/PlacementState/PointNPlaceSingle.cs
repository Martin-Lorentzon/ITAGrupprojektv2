using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointNPlaceSingle : MonoBehaviour
{
    MeshButton buttonMesh;
    public Camera cam;
    [SerializeField]
    GameObject objModel = null, something = null;
    [SerializeField]
    bool scaled = true, scaling = false, busy = false;

    float posOffset = 0f;

    [SerializeField]
    LayerMask mask,maskTwo;

    private void Start()
    {
        if (cam == null)
            cam = Camera.main;
        buttonMesh = GetComponent<MeshButton>();
        gameObject.GetComponent<Button>().onClick.AddListener(Listening);
    }

    public void Listening()
    {
        //SceneInformation.ApplicationState = SceneInformation.AppState.Placement;
        objModel = buttonMesh.meshLibrary.prefabs[buttonMesh.GetButtonIdx];
        posOffset = 0f;
        PlaceOut();
        this.enabled = true;
    }

    void Update()
    {
        if (!scaled)
        {
            scaled = Updating();
        }
    }

    public void PlaceOut()
    {
        this.enabled = true;
        //objModel.transform.localScale = Vector3.one;
        //objModel.GetComponent<MeshRenderer>().materials[0].SetFloat("_Selected", 0f);
        scaled = false;
        scaling = false;
        something = Instantiate(objModel);
        something.layer = 0;
        for (int i = 0; i < something.transform.childCount; i++)
        {
            GameObject child = something.transform.GetChild(i).gameObject;
            child.layer = 0;
        }
    }


    public bool Updating()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray,out hit, 20f,mask))
        {
            Vector3 point = hit.point;

            if (!scaling)
            {
                if(Input.GetKeyDown(KeyCode.UpArrow)|| Input.GetKeyDown(KeyCode.DownArrow))
                posOffset += Input.GetAxisRaw("Vertical")*.5f;
                something.transform.position = point+Vector3.up*posOffset;

            }
        }

        if (scaling)
            something.transform.localScale += new Vector3(0f, Input.GetAxis("Mouse Y") * 0.5f, 0f);

        // When clicking and not scaling, then we're either changing position or height
        // When clicking and is scaling, removes the object and terminates the function
        if (Input.GetButtonDown("Fire1"))
        {
            if (Physics.Raycast(ray, out hit, 20f, maskTwo))
            {
                something.transform.SetParent( hit.transform);
            }
                if (!scaling)
            {
                scaling = true;
                return false;
            }
            something.layer = 25;
            for (int i = 0; i < something.transform.childCount; i++)
            {
                GameObject child = something.transform.GetChild(i).gameObject;
                child.layer = 25;
            }
            something = null;

            SceneInformation.ApplicationState = SceneInformation.AppState.Select;
            Debug.Log(SceneInformation.ApplicationState);
            return true;
        }
        return false;
    }
}