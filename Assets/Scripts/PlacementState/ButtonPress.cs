using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Custom button, script placed on the button
public class ButtonPress : MonoBehaviour
{

    public UnityEvent buttonPress;

    private void Awake()
    {
        if (buttonPress == null) buttonPress = new UnityEvent();
    }

    public void SetButton(UnityAction action)
    {
        Debug.Log("Setting button");
        buttonPress.AddListener(action);
        gameObject.AddComponent<Controlling>();
    }

    private void OnMouseDown()
    {
        buttonPress.Invoke();
    }
}
