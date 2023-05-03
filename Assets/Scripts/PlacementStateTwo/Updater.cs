using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Updater
{
    public abstract bool Updating();
    bool placementState = SceneInformation.ApplicationState == SceneInformation.AppState.RoadEdit;



}