using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetModel : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            string vertexData = FetchData(GUIUtility.systemCopyBuffer, "VERTICES", "ENDVERTICES");
            Debug.Log(vertexData);
            Debug.Log(ParseVertices(vertexData));

            //Debug.Log("TEST");
        }
    }

    string FetchData(string str, string key, string ending)
    {
        char[] newLineChars = Environment.NewLine.ToCharArray();
        string data;
        str = str.TrimEnd('\r', '\n');
        str = str.TrimStart('\r', '\n');
        int pos1 = str.IndexOf(key) + key.Length;
        int pos2 = str.IndexOf(ending);
        data = str.Substring(pos1, pos2 - pos1);
        return data;
    }


    Vector3[] ParseVertices(string str)
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
