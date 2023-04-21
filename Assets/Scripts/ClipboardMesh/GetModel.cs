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
            Vector3[] vertices = ParseVertices(vertexData);

            string triangleData = FetchData(clipboard, "TRIANGLES", "ENDTRIANGLES");


            //Debug.Log(vertexData);
            //Debug.Log(vertices[0]);
            Debug.Log(triangleData);
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


    Vector3[] ParseVertices(string str)     // Should also work for vertex normals and vertex color(?)
    {
        string inputString = str;
        string[] rows = inputString.Split('\n');
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
