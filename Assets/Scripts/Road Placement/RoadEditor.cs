using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RoadEditor : MonoBehaviour
{
    public List<GameObject> points;
    public GameObject point1;
    public GameObject point2;
    public List<GameObject> roadtiles;
    public GameObject roadtile;
    public GameObject controlPoint;
    public GameObject point3;
    public GameObject roadObj;

    //|||||||||||||||||||||||||||||||||||||||||||||||||

    public enum State { point1, point2 };
    public State RoadEditState;

    private LayerMask GroundLayerMask;

    public GameObject roadSegment;

    private GameObject newRoadSegment;
    private GameObject newRoadContainer;

    private float roadHeight = 0.02f;

    void Start()
    {
        RoadEditState = State.point1;
        GroundLayerMask = LayerMask.GetMask("Ground");
    }


    void Update()
    {

        switch (RoadEditState)
        {
            case State.point1:
                MakeAnchor();
                break;

            case State.point2:
                MakeEndPoint();
                break;
        }
    }



    void MakeAnchor()
    {
        bool inRoadEditState = SceneInformation.ApplicationState == SceneInformation.AppState.RoadEdit;

        if (inRoadEditState)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, GroundLayerMask);

            if (hits.Length > 0 && Input.GetMouseButtonDown(0))
            {
                newRoadSegment = Instantiate(roadSegment, hits[0].point, Quaternion.identity);
                newRoadContainer = new GameObject("Road Container");
                newRoadContainer.AddComponent<DontDestroyOnLoad>();

                Transform anchor = newRoadSegment.transform.Find("Anchor").transform;
                Transform controlPoint1 = newRoadSegment.transform.Find("Control Point").transform;
                Transform controlPoint2 = newRoadSegment.transform.Find("Control Point 2").transform;
                Transform endPoint = newRoadSegment.transform.Find("End Point").transform;
                anchor.position = controlPoint1.position = controlPoint2.position = endPoint.position = hits[0].point + Vector3.up * roadHeight;

                RoadEditState = State.point2;
            }
        }
    }

    void MakeEndPoint()
    {
        bool inRoadEditState = SceneInformation.ApplicationState == SceneInformation.AppState.RoadEdit;

        if (inRoadEditState)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, GroundLayerMask);

            newRoadSegment.transform.parent = newRoadContainer.transform;

            if (hits.Length > 0)
            {
                Transform anchor = newRoadSegment.transform.Find("Anchor").transform;
                Transform controlPoint1 = newRoadSegment.transform.Find("Control Point").transform;
                Transform controlPoint2 = newRoadSegment.transform.Find("Control Point 2").transform;
                Transform endPoint = newRoadSegment.transform.Find("End Point").transform;
                endPoint.position = hits[0].point + Vector3.up * roadHeight;

                controlPoint1.position = Vector3.Lerp(anchor.position, endPoint.position, 0.33f);
                controlPoint2.position = Vector3.Lerp(anchor.position, endPoint.position, 0.66f);

                bool makeContinuous = Input.GetKey(KeyCode.LeftShift);

                if (Input.GetMouseButtonDown(0))
                {
                    if (makeContinuous)
                    {
                        newRoadSegment = Instantiate(roadSegment, hits[0].point, Quaternion.identity);
                        anchor = newRoadSegment.transform.Find("Anchor").transform;
                        controlPoint1 = newRoadSegment.transform.Find("Control Point").transform;
                        controlPoint2 = newRoadSegment.transform.Find("Control Point 2").transform;
                        endPoint = newRoadSegment.transform.Find("End Point").transform;
                        anchor.position = controlPoint1.position = controlPoint2.position = endPoint.position = hits[0].point + Vector3.up * roadHeight;
                    }
                    else
                    {
                        RoadEditState = State.point1;
                    }
                }
            }
        }
    }










    void PlaceControlpoint()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);

        RaycastHit hit;
        Physics.Raycast(Camera.main.transform.position, (mouseWorldPos - Camera.main.transform.position).normalized, out hit, 50f);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Plane")
            {
                GameObject controlPoint = Instantiate(point2);
                controlPoint.transform.position = hit.point + new Vector3(0, 0.5f, 0);
                points.Add(controlPoint);
            }
        }
    }


    void PlaceRoadpoint()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);

        RaycastHit hit;
        Physics.Raycast(Camera.main.transform.position, (mouseWorldPos - Camera.main.transform.position).normalized, out hit, 50f);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Plane")
            {

                {
                    GameObject pointInstance = Instantiate(point1);
                    pointInstance.transform.position = hit.point + new Vector3(0, 0.5f, 0);
                    points.Add(pointInstance);

                    PlaceRoadtile(points[points.IndexOf(pointInstance) - 1], points[points.IndexOf(pointInstance)]);
                }
            }
        }
    }


    void PlaceRoadtile (GameObject pos1, GameObject pos2)
    {

        Vector3 startPos = pos1.transform.position;
        Vector3 endPos = pos2.transform.position;

        GameObject roadInstance = Instantiate(roadtile);
        Vector3 roadPos = startPos;
        roadtiles.Add(roadInstance);

        Vector3 tilescale = roadInstance.transform.localScale;

        float scaleMultiplier = Vector3.Distance(startPos, endPos);
        tilescale = tilescale * scaleMultiplier;

        roadInstance.transform.localScale = new Vector3(0.1f, 0.1f, tilescale.z);

        roadInstance.transform.position = roadPos;

        Vector3 relDirection = endPos - startPos;

        Quaternion roadRotation = Quaternion.LookRotation(relDirection, Vector3.up);

        roadInstance.transform.rotation = roadRotation;
    }


    void MakeCurve()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosition);

        RaycastHit hit;
        Physics.Raycast(Camera.main.transform.position, (mouseWorldPos - Camera.main.transform.position).normalized, out hit, 50f);
        if (hit.collider != null)

        {
            if (hit.collider.tag == "Plane")
            {

                GameObject pointInstance = Instantiate(point1);
                pointInstance.transform.position = hit.point + new Vector3(0, 0.5f, 0);
                points.Add(pointInstance);

                Vector3 startPos = points[points.IndexOf(pointInstance) - 2].transform.position;
                Vector3 controlPos = points[points.IndexOf(pointInstance) - 1].transform.position;
                Vector3 endPos = points[points.IndexOf(pointInstance)].transform.position;

                List<GameObject> curvePoints = new List<GameObject>();
                
                float incr = 0.1f;

                for (int i = 0; i < 10; i++)
                {
                    Vector3 vecA = Vector3.Lerp(startPos, controlPos, i * incr);
                    Vector3 vecB = Vector3.Lerp(controlPos, endPos, i * incr);
                    Vector3 vecC = Vector3.Lerp(vecA, vecB, i * incr);
                    GameObject curvePoint = Instantiate(point3);
                    curvePoints.Add(curvePoint);
                    curvePoint.transform.position = vecC;

                    if (curvePoint != curvePoints[0])
                    {
                        PlaceRoadtile(curvePoints[curvePoints.IndexOf(curvePoint) - 1], curvePoints[curvePoints.IndexOf(curvePoint)]);
                    }
                }

                PlaceRoadtile(curvePoints[curvePoints.Count - 1], pointInstance);

            }
        }
    }

    void UpdateCurve()
    {
        
    }

}
