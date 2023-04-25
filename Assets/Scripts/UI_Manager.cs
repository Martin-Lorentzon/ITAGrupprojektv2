using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public Button[] toolDrawerButtons;
    enum Modes {models, materials, roads, scatter};
    public GameObject[] toolPanels;
    public Button lightsButton;
    public GameObject lightsPanel;
    bool lightPanelOn = false;
    public GameObject[] settingsPanels;
    enum Settings { roads };

    public Slider roadWidth;
    public Button roadEditButton;
    public Button addRoadButton;    




    void Start()
    {
        viewModeButton.onClick.AddListener(delegate { GoToViewMode(); });
        workModeButton.onClick.AddListener(delegate { GoToWorkMode(); });
        lightsButton.onClick.AddListener(delegate { ShowLightControls(); });
        viewPanel.SetActive(false);
        lightsPanel.SetActive(false);
        settingsPanels[(int)Settings.roads].SetActive(false);
        toolDrawerButtons[(int)Modes.models].onClick.AddListener(delegate { ModelMode(); });
        toolDrawerButtons[(int)Modes.materials].onClick.AddListener(delegate { MaterialMode(); });
        toolDrawerButtons[(int)Modes.roads].onClick.AddListener(delegate { RoadMode(); });
        toolDrawerButtons[(int)Modes.scatter].onClick.AddListener(delegate { ScatterMode(); });

        roadWidth.onValueChanged.AddListener(delegate { SetRoadWidth(); });
        addRoadButton.onClick.AddListener(delegate { RoadBuild(); });
        roadEditButton.onClick.AddListener(delegate { RoadEdit(); });


        foreach (GameObject panel in settingsPanels)
        {
            panel.SetActive(false);
        }
        foreach (GameObject panel in toolPanels)
        {
            panel.SetActive(false);
        }

        toolPanels[0].SetActive(true);
    }

    void Update()
    {
        if (SceneInformation.selectedObjects.Count > 0)
        {
            foreach (GameObject obj in SceneInformation.selectedObjects)
            {
                if (obj.tag == "Road")
                {
                    settingsPanels[(int)Settings.roads].SetActive(true);
                }
                else
                {
                    settingsPanels[(int)Settings.roads].SetActive(false);
                }
            }

        }
        else
        {
            foreach (GameObject panel in settingsPanels)
            {
                panel.SetActive(false);
            }
        }

        
    }


    void RoadBuild()
    {
        SceneInformation.ApplicationState =  SceneInformation.AppState.RoadEdit;
    }
    void RoadEdit()
    {
        SceneInformation.ApplicationState = SceneInformation.AppState.Select;
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
        

        SceneInformation.ApplicationState = SceneInformation.AppState.Select;

    }

    void MaterialMode()
    {
        foreach (GameObject panel in toolPanels)
        {
            panel.SetActive(false);
        }
        toolPanels[(int)Modes.materials].SetActive(true);


        SceneInformation.ApplicationState = SceneInformation.AppState.Select;

    }

    void RoadMode()
    {
        foreach (GameObject panel in toolPanels)
        {
            panel.SetActive(false);
        }
        toolPanels[(int)Modes.roads].SetActive(true);

        SceneInformation.ApplicationState = SceneInformation.AppState.RoadEdit;
    }

    void ScatterMode()
    {
        foreach (GameObject panel in toolPanels)
        {
            panel.SetActive(false);
        }
        toolPanels[(int)Modes.scatter].SetActive(true);


            SceneInformation.ApplicationState = SceneInformation.AppState.Scatter;

    }

    void GoToViewMode()
    {
        workPanel.SetActive(false);
        viewPanel.SetActive(true);
    }

    void SetRoadWidth()
    {
        if (SceneInformation.selectedObjects.Count > 0)
        {
            foreach (GameObject obj in SceneInformation.selectedObjects)
            {
                if (obj.tag == "Road")
                {
                    RoadSegment2 rs2 = obj.GetComponent<RoadSegment2>();
                    rs2.SetWidth(roadWidth.value);
                }
            }
        }
    }


}
