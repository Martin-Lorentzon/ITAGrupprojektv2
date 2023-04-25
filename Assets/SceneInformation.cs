using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInformation : MonoBehaviour
{
    public enum AppState { Select, Placement, RoadEdit, Scatter };
    public static AppState ApplicationState;

    public static List<GameObject> selectedObjects;

    public static List<GameObject> highlightedObjects;
    public static List<GameObject> markers;

    public static float moveSnapIncrement;
    public static float rotationSnapIncrement;
    public static float snapSpeed;

    public static GameObject marker;

    static void PlaceMarker()
    {
        foreach (GameObject obj in selectedObjects)
        {
            if (!highlightedObjects.Contains(obj))
            {
                LayerMask sceneAssets = LayerMask.GetMask("Scene Asset");
                Ray ray = new Ray(obj.transform.position + (Vector3.up * 100f), -obj.transform.up * Mathf.Infinity);
                RaycastHit[] hits = Physics.RaycastAll(ray, 1000f, sceneAssets);
                if (hits.Length > 0)
                {
                    GameObject mark = Instantiate(marker);
                }
            }
            else
            {
                highlightedObjects.Remove(obj);
            }
        }
    }
}

