using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInformation
{
    public enum AppState { Select, Placement, RoadEdit, Scatter };
    public static AppState ApplicationState;

    public static List<GameObject> selectedObjects;

    public static float moveSnapIncrement;
    public static float rotationSnapIncrement;
    public static float snapSpeed;
}