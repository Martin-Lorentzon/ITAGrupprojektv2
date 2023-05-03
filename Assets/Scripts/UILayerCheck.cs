using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILayerCheck : MonoBehaviour
{

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    public static bool uiHover = false;
    void Start()
    {

        m_Raycaster = GetComponent<GraphicRaycaster>();

        m_EventSystem = GetComponent<EventSystem>();
    }

    void Update()
    {
        
            m_PointerEventData = new PointerEventData(m_EventSystem);
            m_PointerEventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(m_PointerEventData, results);

            if (results.Count > 0)
            {
            uiHover = true;
            }
            else
            {
            uiHover = false;
            }

        
    }
}
