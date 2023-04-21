using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMaterials : MonoBehaviour
{

    public Material material0;
    public Material material1;
    public Material material2;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();

        Material[] mats = new Material[3];
        mats[0] = material0;
        mats[1] = material1;
        mats[2] = material2;

        renderer.materials = mats;
    }

}
