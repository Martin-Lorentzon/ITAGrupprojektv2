using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolButtonStyle : MonoBehaviour, ISelectHandler
{
    public Image icon;
    public Button button;
    public Color baseColor;
    public UI_Manager uiManager;
    ColorBlock colorBlock;
    public Color buttonBaseColor;
    public Color buttonSelectColor;

    void Start()
    {
        //button = gameObject.GetComponent<Button>();
        //icon = gameObject.transform.Find("Image").gameObject.GetComponent<Image>();
        //baseColor = icon.color;
        uiManager = GameObject.Find("UI Manager").GetComponent<UI_Manager>();
        uiManager.AddToolButtons(gameObject);
        //    colorBlock = button.colors;
        //    buttonBaseColor = colorBlock.normalColor;
        //    buttonSelectColor = colorBlock.selectedColor;
    }

    void Awake()
    {
        button = gameObject.GetComponent<Button>();
        icon = gameObject.transform.Find("Image").gameObject.GetComponent<Image>();
        baseColor = icon.color;
        uiManager = GameObject.Find("UI Manager").GetComponent<UI_Manager>();
        uiManager.AddToolButtons(gameObject);
        colorBlock = button.colors;
        buttonBaseColor = colorBlock.normalColor;
        buttonSelectColor = colorBlock.selectedColor;
    }

    public void OnSelect(BaseEventData eventData)
    {


        uiManager.setActiveToolButton(this.gameObject);
        
        colorBlock.normalColor = buttonSelectColor;
        button.colors = colorBlock;
        icon.color = Color.white;

    }

    public void ManualSelect()
    {
        uiManager.setActiveToolButton(this.gameObject);
        colorBlock.normalColor = buttonSelectColor;
        button.colors = colorBlock;
        icon.color = Color.white;
    }

    // ser till att referenserna finns 
    //void SetReferences()
    //{
    //    button = gameObject.GetComponent<Button>();
    //    uiManager = GameObject.Find("UI Manager").GetComponent<UI_Manager>();
    //    colorBlock = button.colors;
    //    buttonBaseColor = colorBlock.normalColor;
    //    buttonSelectColor = colorBlock.selectedColor;
    //    icon = gameObject.transform.Find("Image").gameObject.GetComponent<Image>();
    //    baseColor = icon.color;
    //}

    public void buttonBaseStyle()
    {

        icon.color = baseColor;
        colorBlock.normalColor = buttonBaseColor;
        button.colors = colorBlock;

    }
}
