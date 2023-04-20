using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSlider : MonoBehaviour
{
    public Slider slider;
    public List<KeyValuePair<GameObject,int>> taggedObjects;
    int tagNum;
    int sliderMax;

    void Start()
    {
        tagNum = 0;
        taggedObjects = new List<KeyValuePair<GameObject,int>>();
        sliderMax = 1;
        slider.onValueChanged.AddListener(delegate { SetVisibility(); });
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            tagNum = 7;
            SetTag();
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            tagNum= 8;
            SetTag();
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            tagNum = 9;
            SetTag();
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            RemoveTag();
        }
    }


    // makes a key value pair of the selected object(s) and an int representing in which phase they will be visible
    // if an object is already matched with an int this function will update the current int 
    void SetTag() 
    {
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
        }

        existingKeys.Clear();
        removeList.Clear();
        SetSliderMax();
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
                sliderMax = pair.Value + 1;
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
            if (pair.Value >= slider.value)
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
    }
    
}
