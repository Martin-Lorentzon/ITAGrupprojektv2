using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MaterialLibrary : MonoBehaviour
{

    public GameObject[] materialButtons;
    public Material[] materials;

    void Start()
    {
   
        foreach(GameObject button in materialButtons)
        {
            //int i = 0;
            MaterialButtons mb = button.GetComponent<MaterialButtons>();
            mb.SetIdx();   
           //Color buttonColor = button.GetComponent<Color>();
           // buttonColor = materials[i].GetColor("_BaseColor");
           //     i++;
        }
    }

    public void SetMaterial(int idx)
    {
        UnityEngine.Debug.Log(idx);
        if (SceneInformation.selectedObjects.Count > 0) 
        {
            foreach(GameObject obj in SceneInformation.selectedObjects)
            {
                obj.GetComponent<MeshRenderer>().material = materials[idx];
            }
        }
    }

}
