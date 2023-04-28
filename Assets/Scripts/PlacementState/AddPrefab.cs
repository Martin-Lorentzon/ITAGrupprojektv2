using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPrefab : MonoBehaviour
{

    MeshLibrary librarby;
    bool checking = false;

    void Start()
    {
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
            librarby.prefabs.Add(SceneInformation.selectedObjects[i]);
        }
        librarby.AddThumbnail();
        checking = false;
        SceneInformation.ApplicationState = SceneInformation.AppState.Select;
    }
}