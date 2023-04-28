using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetModel : MonoBehaviour
{
    public Material material;

    void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.Return))
        {
            string clipboard = GUIUtility.systemCopyBuffer;

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
            /*
            
            string vtx_data = CL3D.FetchData(clipboard, "VERTICES", "ENDVERTICES");
            string tri_data = CL3D.FetchData(clipboard, "TRIANGLES", "ENDTRIANGLES");
            Debug.Log(CL3D.ParseVertices(vtx_data));
            //Debug.Log(CL3D.ParseIndices(tri_data));
            */
        }
    }
}