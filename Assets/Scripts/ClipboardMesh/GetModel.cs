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
            Mesh mesh;      // Mesh
            string name;    // Not used for anything

            // Paste Mesh Data
            CL3D.PasteModel(clipboard, out mesh, out name);

            // Update Mesh Filter
            meshFilter.mesh = mesh;
            meshFilter.mesh.RecalculateNormals();
            meshFilter.mesh.RecalculateBounds();

            // Set Material
            meshRenderer.material = material;

            // Update Mesh Collider
            MeshCollider meshCollider;
            try
            {
                meshCollider = GetComponent<MeshCollider>();
                Destroy(meshCollider);
            }
            catch (Exception ex)
            {

            }
            meshCollider = gameObject.AddComponent<MeshCollider>();
        }
    }
}