using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LightDontDestroyOnLoad : MonoBehaviour
{


    public LightControls lightController;

    void Awake()
    {

        GameObject[] objs = GameObject.FindGameObjectsWithTag("DLight");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(gameObject);

        

        
    }

    void Start()
    {
        lightController = GameObject.Find("LightController").GetComponent<LightControls>();
        lightController.sun = gameObject;
    }
}