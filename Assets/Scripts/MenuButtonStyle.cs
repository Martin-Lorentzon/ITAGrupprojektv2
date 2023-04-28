using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonStyle : MonoBehaviour, ISelectHandler
{

    public Button button;
    public UI_Manager uiManager;
    ColorBlock colorBlock;
    public Color buttonBaseColor;
    public Color buttonSelectColor;

    void Start()
    {
        button = gameObject.GetComponent<Button>();


        uiManager = GameObject.Find("UI Manager").GetComponent<UI_Manager>();
        uiManager.AddMenuButtons(gameObject);
        colorBlock = button.colors;
        buttonBaseColor = colorBlock.normalColor;
        buttonSelectColor = colorBlock.selectedColor;
    }



    public void OnSelect(BaseEventData eventData)
    {

        uiManager.setActiveMenuButton(gameObject);
        colorBlock.normalColor = buttonSelectColor;
        button.colors = colorBlock;


    }

    public void ManualSelect()
    {
        uiManager.setActiveMenuButton(gameObject);
        colorBlock.normalColor = buttonSelectColor;
        button.colors = colorBlock;

    }

    public void buttonBaseStyle()
    {
        colorBlock.normalColor = buttonBaseColor;
        button.colors = colorBlock;
    }
}
