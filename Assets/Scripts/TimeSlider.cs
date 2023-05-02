using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeSlider : MonoBehaviour
{
    public Slider slider;
    public List<KeyValuePair<GameObject,int>> taggedObjects;
    int tagNum;
    int sliderMax;
    public TMP_InputField userInput;
    public TMP_Text currentNumberText;
    public GameObject removeButton;
    public TMP_Text slidertext;
    

    void Start()
    {
        
        tagNum = 0;
        taggedObjects = new List<KeyValuePair<GameObject,int>>();
        sliderMax = 1;
        slider.onValueChanged.AddListener(delegate { SetVisibility(); });
        currentNumberText.text = "-";
        userInput.onSubmit.AddListener(delegate { SetTag(int.Parse(userInput.text)); });
        //userInput.onValueChanged.AddListener(delegate { CheckInput(); });
        removeButton.GetComponent<Button>().onClick.AddListener( delegate { RemoveTag(); });
    }

    

    void Update()
    {


    }

    
    public void printCurrentNumber(GameObject activeObject)
    {
        List<GameObject> existingKeys = new List<GameObject>();       

        foreach (KeyValuePair<GameObject, int> pair in taggedObjects)
            {
                existingKeys.Add(pair.Key);
            }

        if (existingKeys.Contains(activeObject))
        {

                foreach (KeyValuePair<GameObject, int> pair in taggedObjects)
                {
                    if (pair.Key == activeObject)
                    {
                    currentNumberText.text = pair.Value.ToString();
                         
                    }
                }

        }
        else
        {
            currentNumberText.text = "-";
        }
        existingKeys.Clear();
        

    }

    // makes a key value pair of the selected object(s) and an int representing in which phase they will be visible
    // if an object is already matched with an int this function will update the current int 
    void SetTag(int tagNum) 
    {
        if (tagNum <= 0) 
        { 
            RemoveTag();
        }
        else {
        List<GameObject> existingKeys = new List<GameObject>();

        List< KeyValuePair<GameObject, int>> removeList = new List<KeyValuePair<GameObject, int>>();

        foreach (KeyValuePair<GameObject, int> pair in taggedObjects)
        {
            existingKeys.Add(pair.Key);
        }

        foreach(GameObject obj in SceneInformation.selectedObjects)
        {
            if (existingKeys.Contains(obj))
            {
                foreach (KeyValuePair<GameObject, int> pair in taggedObjects)
                {
                    if(pair.Key == obj)
                    {
                        removeList.Add(pair);
                    }
                }
            }

            foreach(KeyValuePair<GameObject, int> pair in removeList)
            {
                taggedObjects.Remove(pair);
            }

            taggedObjects.Add(new KeyValuePair<GameObject, int>(obj, tagNum));
            printCurrentNumber(obj);
        }

        
        existingKeys.Clear();
        removeList.Clear();
        SetSliderMax();
         userInput.text= "";
        }
    }


    // removes number tags from objects 
    void RemoveTag()
    {
        List<GameObject> existingKeys = new List<GameObject>();
        List<KeyValuePair<GameObject, int>> removeList = new List<KeyValuePair<GameObject, int>>();

        foreach (KeyValuePair<GameObject, int> pair in taggedObjects)
        {
            existingKeys.Add(pair.Key);
        }

        foreach (GameObject obj in SceneInformation.selectedObjects)
        {
            if (existingKeys.Contains(obj))
            {
                foreach (KeyValuePair<GameObject, int> pair in taggedObjects)
                {
                    if (pair.Key == obj)
                    {
                        removeList.Add(pair);
                    }
                }
            }

            foreach (KeyValuePair<GameObject, int> pair in removeList)
            {
                taggedObjects.Remove(pair);
            }

            printCurrentNumber(obj);
        }

        existingKeys.Clear();
        removeList.Clear();
        SetSliderMax();
    }


    // sets the slider max value
    void SetSliderMax()
    {        
        foreach(KeyValuePair< GameObject, int> pair in taggedObjects)
        {
            if(pair.Value > sliderMax)
            {
                sliderMax = pair.Value;
            }
        }

        // sets slider max value to the highest tag number
        slider.maxValue = sliderMax;
        Debug.Log(slider.maxValue);

        //resets the highest number in case the user will change tags to a lower number
        sliderMax = 1;
    }


    void SetVisibility()
    {
        foreach (KeyValuePair<GameObject, int> pair in taggedObjects)
        {
            if (pair.Value > slider.value)
            {
                pair.Key.SetActive(false);
                if (SceneInformation.selectedObjects.Contains(pair.Key))
                {
                    SceneInformation.selectedObjects.Remove(pair.Key);
                }
            }
            else
            {
                pair.Key.SetActive(true);
            }
        }

        slidertext.text = slider.value.ToString();
    }
    
    public void SetAllVisible()
    {
        foreach (KeyValuePair<GameObject, int> pair in taggedObjects)
        {
           
                pair.Key.SetActive(true);            
        }
    }

    //void CheckInput()
    //{
    //    if (userInput.text. )
    //}
}
