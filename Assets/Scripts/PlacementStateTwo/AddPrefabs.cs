using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPrefabs : MonoBehaviour
{
    GetSetImage imaging;
    MeshLibrary librarbys;
    bool checking = false;
    GameObject tempPre = null;
    public Camera renderCam;

    void Start()
    {
        imaging = GetComponent<GetSetImage>();
        librarbys = GetComponent<MeshLibrary>();
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
            librarbys.prefabs.Add(SceneInformation.selectedObjects[i]);
            librarbys.AddThumbnail(imaging.GetSetImage_non());
        }
        Destroy(tempPre);
        checking = false;
        SceneInformation.ApplicationState = SceneInformation.AppState.Select;
    }
}