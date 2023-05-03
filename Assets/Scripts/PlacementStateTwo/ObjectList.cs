using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;

[Serializable]
public struct Objects
{
    [SerializeField]
    string name;
    [SerializeField]
    GameObject obj;
    [SerializeField]
    public Button button;
    public GameObject GetObj { get { return obj; } }

    public static List<Objects> sceneObjects;

    public GameObject AddGetObj
    {
        get { return obj; }
        set
        {
            Objects newest = new Objects();
            newest.obj = value;
            newest.name = value.name;
            //newest.button = new PlaceObjList.buttonPref;
            sceneObjects.Add(newest);
        }
    }

    public void SetButton(Button _button)
    {
        button = _button;
    }
    //public Button Button { get { return button; } set { if (button == null) button = value; } }
}

public class PlaceObjList : MonoBehaviour
{
    public static Button buttonPref;
    Controlling cont;
    [SerializeField]
    GameObject panel;
    [SerializeField]
    Button button;
    [SerializeField]
    List<Objects> buttons;

    public GameObject Panel { get { return panel; } set { panel = value; } }

    void Start()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            GameObject placing = Instantiate(buttons[i].button.gameObject);
            buttons[i].SetButton(Instantiate(button));

            //buttons[i].GetButton.onClick.AddListener();
            placing.transform.SetParent(panel.transform);
            placing.transform.localPosition = Vector3.zero;
        }
    }
}