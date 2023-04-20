using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class UI_Manager : MonoBehaviour
{
    public GameObject viewPanel;
    public GameObject mainPanel;
    public GameObject workPanel;
    public Button viewModeButton;
    public Button workModeButton;
    bool inViewMode = false;
    public Button[] toolDrawerButtons;
    enum Modes {models, materials};
    public GameObject[] toolPanels;
    public Button lightsButton;
    public GameObject lightsPanel;
    bool lightPanelOn = false;
    
    
    
    void Start()
    {
        viewModeButton.onClick.AddListener(delegate { GoToViewMode(); });
        workModeButton.onClick.AddListener(delegate { GoToWorkMode(); });
        lightsButton.onClick.AddListener(delegate { ShowLightControls(); });
        viewPanel.SetActive(false);
        lightsPanel.SetActive(false);
        toolDrawerButtons[(int)Modes.models].onClick.AddListener(delegate { ModelMode(); });
        toolDrawerButtons[(int)Modes.materials].onClick.AddListener(delegate { MaterialMode(); });

    }


    void ShowLightControls() 
    {
        if (lightPanelOn == false)
        {
            lightPanelOn = true;
            lightsPanel.SetActive(true);
        }
        else
        {
            lightPanelOn = false;
            lightsPanel.SetActive(false);
        }

    }

    void GoToWorkMode() 
    {
        inViewMode = false;
        workPanel.SetActive(true);
        viewPanel.SetActive(false);
    }

    void ModelMode()
    {
        foreach (GameObject panel in toolPanels) 
        {
            panel.SetActive(false);
        }
        toolPanels[(int)Modes.models].SetActive(true);
    }

    void MaterialMode()
    {
        foreach (GameObject panel in toolPanels)
        {
            panel.SetActive(false);
        }
        toolPanels[(int)Modes.materials].SetActive(true);
    }

    void GoToViewMode()
    {
        inViewMode= true;
        workPanel.SetActive(false);
        viewPanel.SetActive(true);
    }



}
