using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScatterButtons : MonoBehaviour
{
    public Scatter scatterScript;
    private int buttonIdx;

    void Start()
    {
        scatterScript = GameObject.Find("ScatterBrush").GetComponent<Scatter>();
        for (int i = 0; i < scatterScript.buttons.Count; i++)
        {
            if (scatterScript.buttons[i] == gameObject)
            {
                buttonIdx = i;
                gameObject.GetComponent<Button>().onClick.AddListener(ChangeObj);
            }
        }
    }

    void ChangeObj()
    {
        scatterScript.ChangeObject(buttonIdx);
    }
}
