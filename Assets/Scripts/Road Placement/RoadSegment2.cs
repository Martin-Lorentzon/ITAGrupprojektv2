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
        Transform controlPoint1 = transform.Find("Control Point").transform;
        Transform controlPoint2 = transform.Find("Control Point 2").transform;
        Transform endPoint = transform.Find("End Point").transform;

        Transform previousPoint;
        Transform nextPoint;

        

        foreach (GameObject tile in roadTiles)
            Destroy(tile);

        if (roadTiles.Count > 0)
            roadTiles.Clear();

        List<Transform> roadPoints = new List<Transform>();
        roadPoints.Add(anchor);

        float incr = 0.1f;
        for (var i = 1; i <= 10f; i++)
        {
            Vector3 vecA = Vector3.Lerp(anchor.position, controlPoint1.position, i * incr);
            Vector3 vecB = Vector3.Lerp(controlPoint1.position, controlPoint2.position, i * incr);
            Vector3 vecC = Vector3.Lerp(controlPoint2.position, endPoint.position, i * incr);
            Vector3 vecD = Vector3.Lerp(vecA, vecB, i * incr);
            Vector3 vecE = Vector3.Lerp(vecB, vecC, i * incr);
            Vector3 vecF = Vector3.Lerp(vecD, vecE, i * incr);

            Transform newPoint = new GameObject().transform;
            newPoint.position = vecF;
            newPoint.parent = gameObject.transform;

            roadPoints.Add(newPoint);
        }

        foreach (Transform point in roadPoints)
        {
            int thisIndex = roadPoints.IndexOf(point);
            int lastIndex = roadPoints.Count - 1;

            

            Vector3 dir;
            Quaternion rot = Quaternion.identity;

            if (thisIndex == 0)
            {
                nextPoint = roadPoints[thisIndex + 1];

                dir = point.position - nextPoint.position;
                rot = Quaternion.LookRotation(dir, Vector3.up);
            }
            else if (thisIndex == lastIndex)
            {
                previousPoint = roadPoints[thisIndex - 1];

                dir = previousPoint.position - point.position;
                rot = Quaternion.LookRotation(dir, Vector3.up);
            }
            else
            {
                previousPoint = roadPoints[thisIndex - 1];
                nextPoint = roadPoints[thisIndex + 1];

                Vector3 dir0 = previousPoint.position - point.position;
                Vector3 dir1 = point.position - nextPoint.position;
                Vector3 averageDir = (dir0 + dir1) / 2f;

                rot = Quaternion.LookRotation(averageDir, Vector3.up);
            }

            point.rotation = rot;
        }

        Vector3[] newVertices = new Vector3[roadPoints.Count * 2];      // Multiply by two for one vertex per side of the road

        foreach (Transform point in roadPoints)
        {
            newVertices[roadPoints.IndexOf(point)*2] = point.position + (point.right * 0.4f);
            newVertices[roadPoints.IndexOf(point)*2+1] = point.position - (point.right * 0.4f);
        }

        mesh.vertices = newVertices;

        int[] indices;
        int numTriangles = newVertices.Length - 2;
        indices = new int[numTriangles * 3];

        for (int i = 0; i < numTriangles; i++)
        {
            if (i % 2 != 0)
            {
                indices[i * 3] = i;
                indices[i * 3 + 1] = i + 1;
                indices[i * 3 + 2] = i + 2;
            }
            else
            {
                indices[i * 3] = i + 1;
                indices[i * 3 + 1] = i;
                indices[i * 3 + 2] = i + 2;
            }
        }

        mesh.triangles = indices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
    }

    private void OnDrawGizmos()
    {
        foreach (Vector3 vert in mesh.vertices)
        {
            Gizmos.DrawSphere(vert, 0.2f);
        }
    }
}
