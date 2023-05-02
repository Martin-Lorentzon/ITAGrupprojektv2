using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImpostTerrain : MonoBehaviour
{
    public Texture2D map;
    public float strength = 0.1f;
    public bool platauEnabled;
    public float platauHeight = 0.05f;

    public Terrain t;
    public TerrainData tData;
    float[,] heights;

    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Terrain>();
        tData = t.terrainData;
        Build();
    }

    public void SetPlatauEnable(bool set)
    {
        platauEnabled = set;
    }
    public void SetStrength(float set)
    {
        strength = set;
    }
    public void SetPHeight(float set)
    {
        platauHeight = set;
    }
    public void Clear()
    {
        ClearMap(map.width + 0, map.height + 0);
    }


    public void Build()
    {
        if (platauEnabled)
        {
            UpdateMapPlatau(map.width + 0, map.height + 0);
        } else
        {
            UpdateMap(map.width + 0, map.height + 0);
        }
        UpdateObjectPosition.UpdatePosition();
    }

    public void Import(Texture2D rMap)
    {
        map = rMap;
        Debug.Log("x = " + map.width + " | y = " + map.height);

        tData.heightmapResolution = map.width;

        print("resolution = " + tData.heightmapResolution);

        tData.size = new Vector3(200,200,200);

        map = ScaleTexture(map, tData.heightmapResolution, tData.heightmapResolution);
        Debug.Log("rescaled to: x = " + map.width + " | y = " + map.height);
    }

    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);
        float incX = (1.0f / (float)targetWidth);
        float incY = (1.0f / (float)targetHeight);
        for (int i = 0; i < result.height; ++i)
        {
            for (int j = 0; j < result.width; ++j)
            {
                Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                result.SetPixel(j, i, newColor);
            }
        }
        result.Apply();
        return result;
    }

    void UpdateMap(int xRes, int yRes)
    {
        //print("apply");
        heights = tData.GetHeights(0, 0, xRes, yRes);


        for (int y = 0; y < yRes; y++)
        {
            for (int x = 0; x < xRes; x++)
            {
                heights[x, y] = map.GetPixel(x, y).grayscale * strength;
            }

        }
        tData.SetHeights(0, 0, heights);
    }
    void UpdateMapPlatau(int xRes, int yRes)
    {
        //print("apply");
        heights = tData.GetHeights(0, 0, xRes, yRes);

        for (int y = 0; y < yRes; y++)
        {
            for (int x = 0; x < xRes; x++)
            {
                heights[x, y] = Mathf.Round(map.GetPixel(x, y).grayscale / platauHeight) * platauHeight * strength;
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

    void ClearMap(int xRes, int yRes)
    {
        heights = tData.GetHeights(0, 0, xRes, yRes);

        for (int y = 0; y < yRes; y++)
        {
            for (int x = 0; x < xRes; x++)
            {
                heights[x, y] = 0;
            }

        }
        tData.SetHeights(0, 0, heights);
    }
}
