using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppManager : MonoBehaviour
{

    private GameObject translateGizmoInstance;



    void Start()
    {
        SceneInformation.ApplicationState = SceneInformation.AppState.Select;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SceneInformation.ApplicationState = SceneInformation.AppState.Select;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SceneInformation.ApplicationState = SceneInformation.AppState.Placement;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            SceneInformation.ApplicationState = SceneInformation.AppState.RoadEdit;
        if (Input.GetKeyDown(KeyCode.Alpha4))
            SceneInformation.ApplicationState = SceneInformation.AppState.Scatter;

        //Debug.Log(SceneInformation.ApplicationState);
    }
}
