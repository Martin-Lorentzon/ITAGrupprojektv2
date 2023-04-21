using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialButtons : MonoBehaviour
{
    public MaterialLibrary matLibrary;
    private int buttonIdx;

    public void SetIdx()
    {
        matLibrary = GameObject.Find("MaterialManager").GetComponent<MaterialLibrary>();
        for (int i = 0; i < matLibrary.materialButtons.Length; i++)
        {
            if (matLibrary.materialButtons[i] == gameObject)
            {
                buttonIdx = i;
                gameObject.GetComponent<Button>().onClick.AddListener(SetMat);
            }
        }
    }

    public void SetMat()
    {
        matLibrary.SetMaterial(buttonIdx);
    }
}
