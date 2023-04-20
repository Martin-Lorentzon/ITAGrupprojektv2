using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleChange : MonoBehaviour
{

    [SerializeField]CameraController camCon;

    void Start()
    {
        
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
            camCon.enabled = false;
            for (int i = 0; i < SceneInformation.selectedObjects.Count; i++)
            {
                Vector3 changes = new Vector3();
                if(Input.GetButton("Fire1"))
                  changes = new Vector3(0f,Input.GetAxis("Mouse Y"),0f);
                else
                  changes = new Vector3(Input.GetAxis("Mouse X"),0f,Input.GetAxis("Mouse Y"));

                SceneInformation.selectedObjects[i].transform.localScale += changes;
            }
        }
        else
            camCon.enabled = true;
    }
}
