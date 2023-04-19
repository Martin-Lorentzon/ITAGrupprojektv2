using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public enum Options { Add = 1, Change = 2}

/*
 * Menyn för ett object.
 * Menyn håller knappar som den viker ut on in runt objektet
 * Den ger knapparna's UnityEvent en funktion som modifierar objektet.
 */
public class ChangeOptions : MonoBehaviour
{
    [SerializeField]
    GameObject obj;

    public GameObject Obj { get { return obj; } set { obj = value; } }

    public Options options;
    [SerializeField]
    RectTransform menuCanvas;
    Vector3 menuSize = Vector3.one;
    
    List<Transform> buttons = new List<Transform>();

    public UnityAction Actions;

    [SerializeField]
    List<Button> butts = new List<Button>();

    [SerializeField]
    List<UnityAction> actors = new List<UnityAction>();

    [SerializeField]
    bool notStarted = false;

    void Start()
    {
        //actors.Add(Actor0);
        //actors.Add(Actor1);

        transform.localScale = Vector3.one*1.2f; // menyn visas inte från början
        menuSize = Vector3.one*1.2f;
        //this.enabled = false;

        for (int i = 0; i < menuCanvas.childCount; i++)
        {
            butts.Add(menuCanvas.GetChild(i).GetComponent<Button>());
            PointNPlaceSingle placer = butts[i].gameObject.GetComponent<PointNPlaceSingle>();
            actors.Add(placer.PlaceOut);

            if (i < actors.Count)
                butts[i].onClick.AddListener(actors[i]);
            //buttons.Add(menuCanvas.transform.GetChild(i).gameObject.transform);
            //buttons[i].gameObject.GetComponent<ButtonPress>().SetButton(actors[i]);
        }
    }

    // Kallas för att starta menyn och välja.
    // Skickar tillbaka en bool för att säga att den är på
    public bool StartChoices()
    {
        Debug.Log("sets menu size and busy");
        //Variabeln som används för att gradvis sätta menyn's storlek I update
        //menuSize = new Vector3( .005f,.01f,0f)*(transform.position-Camera.main.transform.position).sqrMagnitude*0.5f;
        menuSize = Vector3.one;
        Debug.Log("starting");
        return this.enabled = true;
    }

    void Update()
    {
        bool placementState = SceneInformation.ApplicationState == SceneInformation.AppState.Placement;

        transform.localScale = Vector3.MoveTowards(transform.localScale,menuSize,Time.deltaTime*.8f);

        if (Input.GetKeyDown("2")&& notStarted == placementState)
            SceneInformation.ApplicationState = SceneInformation.AppState.Select;
        if (placementState)
        {
            if (notStarted != placementState)
                StartChoices();
        }
        else
        {
            if (notStarted != placementState)
            {
                Debug.Log("end choices");
                menuSize = Vector3.one*1.2f;
                notStarted = placementState;
            }
            return;
        }

        //menuSize = new Vector3(.005f, .01f, 0f) * (transform.position - Camera.main.transform.position).sqrMagnitude * 0.05f;
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].localScale = Vector3.MoveTowards(buttons[i].localScale, Vector3.one * (transform.position - Camera.main.transform.position).sqrMagnitude * 0.05f,Time.deltaTime*.02f);
        }

        // Ändra menyn's storlek gradvis
        CameraRotate();

        switch (options)
        {
            case Options.Add:
                //Actor0();
            break;
            case Options.Change:
                //Actor1();
            break;
            default:

            break;
        }
        notStarted = placementState;
    }

    void CameraRotate()
    {
        Vector3 direction = Vector3.ProjectOnPlane(gameObject.transform.position - Camera.main.transform.position,transform.up);
        menuCanvas.rotation = Quaternion.LookRotation(direction,transform.up);
    }
    /*
    void Actor0()
    {

    }

    void Actor1()
    {
        Debug.Log("Buttoned1");
        if (Input.GetButtonDown("Fire1"))
            options = 0;
    }
    */
    public void C_Change() { options = Options.Change; }

    public void C_Add() { options = Options.Add; }
}