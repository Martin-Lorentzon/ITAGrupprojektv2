using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetModel : MonoBehaviour
{
    public Material material;

    void Update()
    {
        string clipboard = GUIUtility.systemCopyBuffer;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            Mesh mesh;
            string name;

            CL3D.PasteModel(clipboard, out mesh, out name);

            meshFilter.mesh = mesh;
            meshFilter.mesh.RecalculateNormals();
            meshFilter.mesh.RecalculateBounds();

            meshRenderer.material = material;

            gameObject.transform.localScale = new Vector3(1f, 1f, 1f) * 0.1f;
        }
    }
}