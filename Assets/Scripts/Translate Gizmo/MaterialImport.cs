using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialImport : MonoBehaviour
{
    public Material presetMat;
    void Start()
    {
        GetComponent<Renderer>().materials[0] = presetMat;
        //Material[] materialArr = gameObject.GetComponent<Renderer>().materials;

        //for (int i = 0; i < materialArr.Length; i++)
        //{
        //    gameObject.GetComponent<Renderer>().materials[i] = presetMat;
        //    Debug.Log(gameObject.GetComponent<Renderer>().materials[i].name);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
