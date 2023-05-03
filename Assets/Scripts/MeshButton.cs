using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MeshButton : MonoBehaviour
{
    public MeshLibrary meshLibrary;
    private int buttonIdx;

    public int GetButtonIdx { get { return buttonIdx; } }

    void Start()
    {
        meshLibrary = GameObject.Find("Content").GetComponent<MeshLibrary>();
        for (int i = 0; i < meshLibrary.buttons.Count; i++)
        {
            if (meshLibrary.buttons[i] == gameObject)
            {
                buttonIdx = i;
                gameObject.GetComponent<Button>().onClick.AddListener(SetMesh);
            }
        }
    }

    void SetMesh()
    {
        meshLibrary.ChangeMesh(buttonIdx);
    }
}