using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSegment2 : MonoBehaviour
{
    public int roadResolution;

    public GameObject roadTile;

    public List<GameObject> roadTiles;


    //|||||||||||||||||||||||

    private Mesh mesh;



    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
    }

    void Update()
    {
        //Vector3[] vertices = mesh.vertices;
        //Vector3[] normals = mesh.normals;



        Transform anchor = transform.Find("Anchor").transform;
        Transform controlPoint = transform.Find("Control Point").transform;
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
            Vector3 vecA = Vector3.Lerp(anchor.position, controlPoint.position, i * incr);
            Vector3 vecB = Vector3.Lerp(controlPoint.position, endPoint.position, i * incr);
            Vector3 vecC = Vector3.Lerp(vecA, vecB, i * incr);

            points.Add(vecC);

            Vector3 dir = previousPoint - vecC;            // A vector pointing from pointA to pointB
            Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);

            GameObject newTile = Instantiate(roadTile, vecC, rot);
            newTile.transform.parent = gameObject.transform;
            

            Vector3 newPoint = newTile.transform.position;

            newTile.transform.localScale = new Vector3(0.3f, 0.3f, Vector3.Distance(points[i], points[i-1]) /2f);

            previousPoint = newPoint;

            roadTiles.Add(newTile);
        }
        


        
    }
}
