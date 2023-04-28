using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ButtonStyle : MonoBehaviour, ISelectHandler /*IDeselectHandler */
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
        button = gameObject.GetComponent<Button>();
        icon = gameObject.transform.Find("Image").gameObject.GetComponent<Image>();
        baseColor = icon.color;
        uiManager= GameObject.Find("UI Manager").GetComponent<UI_Manager>();
        uiManager.AddSettingsButtons(gameObject);
        colorBlock = button.colors;
        buttonBaseColor = colorBlock.normalColor;
        buttonSelectColor = colorBlock.selectedColor;
    }



    public void OnSelect(BaseEventData eventData)
    {
        
        uiManager.setActiveSettingsButton(gameObject);
        colorBlock.normalColor = buttonSelectColor;
        button.colors = colorBlock;
        icon.color = Color.white;

    }

    public void ManualSelect()
    {
        uiManager.setActiveSettingsButton(gameObject);
        colorBlock.normalColor = buttonSelectColor;
        button.colors = colorBlock;
        icon.color = Color.white;
    }

    public void buttonBaseStyle()
    {

        icon.color = baseColor;
        colorBlock.normalColor = buttonBaseColor;
        button.colors = colorBlock;
    }

    //public void OnDeselect(BaseEventData eventData)
    //{
    //Debug.Log("deselected");
    //    icon.color = baseColor;
    //}



}
