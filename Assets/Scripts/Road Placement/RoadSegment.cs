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
        Transform controlPoint = transform.Find("Control Point").transform;
        Transform endPoint = transform.Find("End Point").transform;

        Transform previousPoint = anchor;


        

        foreach (GameObject tile in roadTiles)
            Destroy(tile);

        if (roadTiles.Count > 0)
            roadTiles.Clear();


        float incr = 0.1f;
        for (var i = 1; i <= 10; i++)
        {
            Vector3 vecA = Vector3.Lerp(anchor.position, controlPoint.position, i * incr);
            Vector3 vecB = Vector3.Lerp(controlPoint.position, endPoint.position, i * incr);
            Vector3 vecC = Vector3.Lerp(vecA, vecB, i * incr);

            var dir = previousPoint.position - vecC;            // A vector pointing from pointA to pointB
            var rot = Quaternion.LookRotation(dir, Vector3.up);

            GameObject newTile = Instantiate(roadTile, vecC, rot);
            newTile.transform.parent = gameObject.transform;
            roadTiles.Add(newTile);


            roadTile.transform.localScale = new Vector3(0.3f, 0.3f, Vector3.Distance(vecC, previousPoint.position)*0.5f);

            previousPoint = newTile.transform;
        }
        


        
    }
}
