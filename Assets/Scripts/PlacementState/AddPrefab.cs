using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPrefab : MonoBehaviour
{
    GetSetImage imaging;
    MeshLibrary librarby;
    bool checking = false;
    GameObject tempPre = null;
    public Camera renderCam;

    void Start()
    {
        imaging = GetComponent<GetSetImage>();
        librarby = GetComponent<MeshLibrary>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
            checking = true;

        if (SceneInformation.ApplicationState != SceneInformation.AppState.Placement)
            return;
        for (int i = 0; i < SceneInformation.selectedObjects.Count; i++)
        {
            if(tempPre != null)
            Destroy(tempPre);

            tempPre = Instantiate(SceneInformation.selectedObjects[i]);
            tempPre.transform.position = renderCam.transform.position + renderCam.transform.forward * 5f;
            librarby.prefabs.Add(SceneInformation.selectedObjects[i]);
            librarby.AddThumbnail(imaging.GetSetImage_non());
        }
        checking = false;
        SceneInformation.ApplicationState = SceneInformation.AppState.Select;
    }
}