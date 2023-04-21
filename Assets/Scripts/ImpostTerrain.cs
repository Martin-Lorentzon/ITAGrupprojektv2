using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpostTerrain : MonoBehaviour
{

    public Texture2D map;
    public float strength = 0.1f;

    private Terrain t;
    TerrainData tData;
    float[,] heights;

    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Terrain>();
        tData = t.terrainData;

        //xRes = tData.heightmapResolution;
        //yRes = tData.heightmapResolution;
    }

    // Update is called once per frame
    void Update()
    {
        //t.terrainData.SetHeights();
        if (Input.GetKeyDown(KeyCode.N))
        {
            //RandomizeMap(tData.heightmapResolution, tData.heightmapResolution);
            UpdateMap(map.width+ 1,map.height+1 );
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            //RandomizeMap(tData.heightmapResolution, tData.heightmapResolution);
            UpdateMapPlatau(map.width + 1, map.height + 1);
        }
    }

    void UpdateMap(int xRes, int yRes)
    {
        print("apply");
        heights = tData.GetHeights(0, 0, xRes, yRes);

        for (int y = 0; y < yRes; y++)
        {
            for (int x = 0; x < xRes; x++)
            {
                heights[x, y] = map.GetPixel(x,y).grayscale * strength;
                //print(x + ", " + y);
            }

        }
        tData.SetHeights(0, 0, heights);
    }
    void UpdateMapPlatau(int xRes, int yRes)
    {
        print("apply");
        heights = tData.GetHeights(0, 0, xRes, yRes);

        for (int y = 0; y < yRes; y++)
        {
            for (int x = 0; x < xRes; x++)
            {
                heights[x, y] = Mathf.Round(map.GetPixel(x, y).grayscale * strength);              
            }

        }
        tData.SetHeights(0, 0, heights);
    }

    void RandomizeMap(int xRes, int yRes)
    {
        heights = tData.GetHeights(0, 0, xRes, yRes);

        for (int y = 0; y < yRes; y++)
        {
            for (int x = 0; x < xRes; x++)
            {
                heights[x, y] = Random.Range(0.0f, strength) * 0.5f;
            }

        }
        tData.SetHeights(0, 0, heights);
    }
}
