using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPointVisual : MonoBehaviour
{
    public Vector3 smallScale;
    public Vector3 bigScale;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);

        GameObject parent = transform.parent.gameObject;

        bool hover = false;

        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.gameObject == parent)
                    hover = true;
            }
        }

        if (hover || SceneInformation.selectedObjects.Contains(parent))
            transform.localScale = Vector3.Lerp(transform.localScale, bigScale, 50 * Time.deltaTime);
        else
            transform.localScale = Vector3.Lerp(transform.localScale, smallScale, 50 * Time.deltaTime);
    }
}