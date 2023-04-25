using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSegment : MonoBehaviour
{
    // Start is called before the first frame update

    public int roadResolution;

    public GameObject roadTile;

    public List<GameObject> roadTiles;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Transform anchor = transform.Find("Anchor").transform;
        Transform controlPoint1 = transform.Find("Control Point").transform;
        Transform controlPoint2 = transform.Find("Control Point 2").transform;
        Transform endPoint = transform.Find("End Point").transform;

        Vector3 previousPoint = anchor.position;


        

        foreach (GameObject tile in roadTiles)
            Destroy(tile);

        if (roadTiles.Count > 0)
            roadTiles.Clear();

        List<Vector3> points = new List<Vector3>();
        points.Add(anchor.position);

        float incr = 0.1f;
        for (var i = 1; i <= 10f; i++)
        {
            Vector3 vecA = Vector3.Lerp(anchor.position, controlPoint1.position, i * incr);
            Vector3 vecB = Vector3.Lerp(controlPoint1.position, controlPoint2.position, i * incr);
            Vector3 vecC = Vector3.Lerp(controlPoint2.position, endPoint.position, i * incr);
            Vector3 vecD = Vector3.Lerp(vecA, vecB, i * incr);
            Vector3 vecE = Vector3.Lerp(vecB, vecC, i * incr);
            Vector3 vecF = Vector3.Lerp(vecD, vecE, i * incr);

            points.Add(vecF);

            Vector3 dir = previousPoint - vecF;            // A vector pointing from pointA to pointB
            Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);

            GameObject newTile = Instantiate(roadTile, vecF, rot);
            newTile.transform.parent = gameObject.transform;
            

            Vector3 newPoint = newTile.transform.position;

            newTile.transform.localScale = new Vector3(0.3f, 0.3f, Vector3.Distance(points[i], points[i-1]) /2f);

            previousPoint = newPoint;

            roadTiles.Add(newTile);
        }
        


        
    }
}
