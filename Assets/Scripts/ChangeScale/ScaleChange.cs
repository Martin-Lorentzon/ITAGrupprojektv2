using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleChange : MonoBehaviour
{
    Camera cam;
    [SerializeField]CameraController camCon;

    void Start()
    {
        cam = Camera.main;
        if (camCon == null)
            camCon = GetComponent<CameraController>();
    }

    void Update()
    {
        bool placementState = SceneInformation.ApplicationState == SceneInformation.AppState.Select;
        if (!placementState)
            return;

        if (SceneInformation.selectedObjects.Count == 0)
            return;

        if (Input.GetButton("Fire2"))
        {
            Vector3 projCam = Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up);
            camCon.enabled = false;
            for (int i = 0; i < SceneInformation.selectedObjects.Count; i++)
            {
                Vector3 changes = new Vector3();
                if(Input.GetButton("Fire1"))
                  changes = new Vector3(0f,Input.GetAxis("Mouse Y"),0f);
                else
                  changes = Quaternion.LookRotation(projCam,Vector3.up)*  new Vector3(Input.GetAxis("Mouse X"),0f,Input.GetAxis("Mouse Y"));

                SceneInformation.selectedObjects[i].transform.localScale += changes;
            }
        }
        else
            camCon.enabled = true;
    }
}
