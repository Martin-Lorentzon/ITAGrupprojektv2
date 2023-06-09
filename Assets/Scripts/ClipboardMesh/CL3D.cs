using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class CL3D : MonoBehaviour
{
    public static void PasteModel(string str, bool UInt32, out Mesh mesh, out string name)
    {
        string vertexData = FetchData(str, "VERTICES", "ENDVERTICES");
        string triangleData = FetchData(str, "TRIANGLES", "ENDTRIANGLES");

        mesh = new Mesh();
        if (UInt32)
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        Vector3[] vertices = ParseVertices(vertexData);
        int[] triangles = ParseIndices(triangleData);

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        name = "testname";
    }


    public static string FetchData(string str, string key, string ending, bool trimNewLines = true)
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


    public static int[] ParseIndices(string str)
    {
        string[] indices = str.Split(' ');
        int[] intArray = new int[indices.Length];

        for (int i = 0; i < indices.Length; i++)
        {
            intArray[i] = int.Parse(indices[i]);
        }
        return intArray;
    }


    public static Vector3[] ParseVertices(string str)
    {
        string[] rows = str.Split('\n');

        foreach(string s in rows)
        {
            Debug.Log(s);
        }
        
        Vector3[] vectorArray = new Vector3[rows.Length];
        
        for (int i = 0; i < rows.Length; i++)
        {
            string[] values = rows[i].Split(' ');
            float x = float.Parse(values[0], CultureInfo.InvariantCulture);
            float y = float.Parse(values[1], CultureInfo.InvariantCulture);
            float z = float.Parse(values[2], CultureInfo.InvariantCulture);
            vectorArray[i] = new Vector3(x, y, z);
        }
        
        return vectorArray;
    }
}