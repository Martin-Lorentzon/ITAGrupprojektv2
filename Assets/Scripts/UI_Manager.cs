using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

//using UnityEngine.UIElements;

public class UI_Manager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject viewPanel;
    public GameObject mainPanel;
    public GameObject workPanel;
    public GameObject lightsPanel;
    public GameObject[] toolPanels;
    public GameObject[] settingsPanels;

    [Header("Buttons")]
    public Button viewModeButton;
    public Button workModeButton;
    public Button[] toolDrawerButtons;
    public Button lightsButton;
    public Button roadEditButton;
    public Button addRoadButton;

    [Header("Sliders")]
    public Slider roadWidth;

    [Header("Buttons (do not set in inspector)")]
    public List<GameObject> settingsButtons;
    public List<GameObject> menuButtons;
    public List<GameObject> toolButtons;

    [Header("Others (do not set in inspector)")]
    public Image lightsIcon;

    enum Modes {models, materials, roads, scatter};
    enum Settings { roads, numTags };

    bool lightPanelOn = false;

    static GameObject activeSettingsButton;
    static GameObject activeMenuButton;
    static GameObject activeToolButton;

    TimeSlider timeSliderScript;


    void Start()
    {
        viewPanel.SetActive(false);
        lightsPanel.SetActive(false);


        viewModeButton.onClick.AddListener(delegate { GoToViewMode(); });
        workModeButton.onClick.AddListener(delegate { GoToWorkMode(); });
        lightsButton.onClick.AddListener(delegate { ShowLightControls(); });
        settingsPanels[(int)Settings.roads].SetActive(false);
        toolDrawerButtons[(int)Modes.models].onClick.AddListener(delegate { ModelMode(); });
        toolDrawerButtons[(int)Modes.materials].onClick.AddListener(delegate { MaterialMode(); });
        toolDrawerButtons[(int)Modes.roads].onClick.AddListener(delegate { RoadMode(); });
        toolDrawerButtons[(int)Modes.scatter].onClick.AddListener(delegate { ScatterMode(); });
        roadWidth.onValueChanged.AddListener(delegate { SetRoadWidth(); });
        addRoadButton.onClick.AddListener(delegate { RoadBuild(); });
        roadEditButton.onClick.AddListener(delegate { RoadEdit(); });

        timeSliderScript = GameObject.Find("TimeSliderControl").GetComponent<TimeSlider>();
        lightsIcon = lightsButton.transform.Find("Image").gameObject.GetComponent<Image>();

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
                    // show road width slider if road is selected
                    settingsPanels[(int)Settings.roads].SetActive(true);
                }
                else
                {
                    // show timeline tag settings if non-road object is selected and auto activate the input field 
                    settingsPanels[(int)Settings.roads].SetActive(false);
                    settingsPanels[(int)Settings.numTags].SetActive(true);
                    timeSliderScript.userInput.ActivateInputField();
                }
            }

        }
        else
        {
            // if no objects are selected, show no object specific panels
            foreach (GameObject panel in settingsPanels)
            {
                panel.SetActive(false);
            }
        }
     
    }


    // the following functions handles which buttons are highlighted as active in the different menus
    public void AddSettingsButtons(GameObject button)
    {
        settingsButtons.Add(button);
    }

    public void AddMenuButtons(GameObject button)
    {
        menuButtons.Add(button);
    }

    public void AddToolButtons(GameObject button)
    {
        toolButtons.Add(button);
    }

    public void setActiveSettingsButton(GameObject button)
    {
       button = activeSettingsButton;
       
    foreach(GameObject obj in settingsButtons)
        {
            if (obj != activeSettingsButton)
            { 
            ButtonStyle styleManager = obj.GetComponent<ButtonStyle>();
                styleManager.buttonBaseStyle();
            }
        }
    }

    public void setActiveMenuButton(GameObject button)
    {
        button = activeMenuButton;

        foreach (GameObject obj in menuButtons)
        {
            if (obj != activeMenuButton)
            {
                MenuButtonStyle menuStyleManager = obj.GetComponent<MenuButtonStyle>();
                menuStyleManager.buttonBaseStyle();
            }
        }
    }

    public void setActiveToolButton(GameObject button)
    {
        button = activeToolButton;

        foreach (GameObject obj in toolButtons)
        {
            if (obj != activeToolButton)
            {
                ToolButtonStyle styleManager = obj.GetComponent<ToolButtonStyle>();
                styleManager.buttonBaseStyle();
            }
        }
    }


    // toggles the visibility of the light controls panel
    void ShowLightControls() 
    {
        if (lightPanelOn == false)
        {
            lightPanelOn = true;
            lightsPanel.SetActive(true);
            lightsIcon.color = new Color32(233, 86, 41, 255);
        }
        else
        {
            lightPanelOn = false;
            lightsPanel.SetActive(false); 
            lightsIcon.color = new Color32(0, 128, 55, 255);
        }

    }

    
    // sets UI to work space. Sets Model mode as active menu tab. Sets all Objects visible (in case they were hidden by the timeline slider)
    void GoToWorkMode() 
    {
        workPanel.SetActive(true);
        viewPanel.SetActive(false);

        timeSliderScript.SetAllVisible();
        ModelMode();
        setActiveMenuButton(toolDrawerButtons[(int)Modes.models].gameObject);
        MenuButtonStyle menuButtonStyle = toolDrawerButtons[(int)Modes.models].gameObject.GetComponent<MenuButtonStyle>();
        menuButtonStyle.ManualSelect();
    }

    //sets mode to model mode

    void ModelMode()
    {
        foreach (GameObject panel in toolPanels) 
        {
            panel.SetActive(false);
        }
        toolPanels[(int)Modes.models].SetActive(true);       

        SceneInformation.ApplicationState = SceneInformation.AppState.Select;
    }

    //sets materials as active mode
    void MaterialMode()
    {
        foreach (GameObject panel in toolPanels)
        {
            panel.SetActive(false);
        }
        toolPanels[(int)Modes.materials].SetActive(true);


        SceneInformation.ApplicationState = SceneInformation.AppState.Select;

    }

    //sets Roads as active mode. Defaults to always start on the road build tool
    void RoadMode()
    {
        foreach (GameObject panel in toolPanels)
        {
            panel.SetActive(false);
        }
        toolPanels[(int)Modes.roads].SetActive(true);

        SceneInformation.ApplicationState = SceneInformation.AppState.RoadEdit;

        setActiveToolButton(addRoadButton.gameObject);
        ToolButtonStyle toolButtonStyle = addRoadButton.gameObject.GetComponent<ToolButtonStyle>();
        toolButtonStyle.ManualSelect();
        
    }

    void RoadBuild()
    {
        SceneInformation.ApplicationState = SceneInformation.AppState.RoadEdit;
    }
    void RoadEdit()
    {
        SceneInformation.ApplicationState = SceneInformation.AppState.Select;
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

    //sets Scatter as active mode. Defaults to always start on the "scatter" setting of the scatter brush
    void ScatterMode()
    {
        foreach (GameObject panel in toolPanels)
        {
            panel.SetActive(false);
        }
        toolPanels[(int)Modes.scatter].SetActive(true);

        SceneInformation.selectedObjects.Clear();
        Scatter scatterBrush = GameObject.Find("ScatterBrush").GetComponent<Scatter>();

        ButtonStyle buttonStyle = scatterBrush.defaultActiveSettingsButton.GetComponent<ButtonStyle>();
        buttonStyle.ManualSelect();

        SceneInformation.ApplicationState = SceneInformation.AppState.Scatter;
       
    }

    // sets scene to presentation/view mode. Sets state to select. Sets timeline slider to max value so that everything is visible.
    void GoToViewMode()
    {
        workPanel.SetActive(false);
        viewPanel.SetActive(true);
        TimeSlider timeSliderScript = GameObject.Find("TimeSliderControl").GetComponent<TimeSlider>();
        timeSliderScript.slider.value = timeSliderScript.slider.maxValue;
        SceneInformation.ApplicationState = SceneInformation.AppState.Select;
    }

}
