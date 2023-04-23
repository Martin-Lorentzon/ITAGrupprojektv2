using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetModel : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            string clipboard = GUIUtility.systemCopyBuffer;

            string vertexData = FetchData(clipboard, "VERTICES", "ENDVERTICES");
            string triangleData = FetchData(clipboard, "TRIANGLES", "ENDTRIANGLES");

            Vector3[] vertices = ParseVertices(vertexData);
            int[] triangles = ParseIndices(triangleData);


            Debug.Log(vertexData);
            Debug.Log(vertices[0]);
            Debug.Log(triangleData);
            Debug.Log(triangles[3]);
        }
    }

    string FetchData(string str, string key, string ending, bool trimNewLines = true)
    {
        char[] newLineChars = Environment.NewLine.ToCharArray();
        string data;
        int pos1 = str.IndexOf(key) + key.Length;
        int pos2 = str.IndexOf(ending);
        data = str.Substring(pos1, pos2 - pos1);
        if (trimNewLines)
            data = data.Trim(newLineChars);

        return data;
    }


    int[] ParseIndices(string str)
    {
        string[] indices = str.Split(' ');
        int[] intArray = new int[indices.Length];

        for (int i = 0; i < indices.Length; i++)
        {
            intArray[i] = int.Parse(indices[i]);
        }
        return intArray;
    }


    Vector3[] ParseVertices(string str)
    {
        string[] rows = str.Split('\n');
        Vector3[] vectorArray = new Vector3[rows.Length];

        for (int i = 0; i < rows.Length; i++)
        {
            string[] values = rows[i].Split(' ');
            float x = float.Parse(values[0]);
            float y = float.Parse(values[1]);
            float z = float.Parse(values[2]);
            vectorArray[i] = new Vector3(x, y, z);
        }
        return vectorArray;
    }
}
