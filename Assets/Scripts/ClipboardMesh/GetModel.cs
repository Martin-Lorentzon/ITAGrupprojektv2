using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetModel : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log(ParseVertices(GUIUtility.systemCopyBuffer)[0]);
            Debug.Log(ParseVertices(GUIUtility.systemCopyBuffer)[1]);
            Debug.Log(ParseVertices(GUIUtility.systemCopyBuffer)[2]);
        }
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
