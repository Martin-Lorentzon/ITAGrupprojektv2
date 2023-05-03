using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class UpdateObjectPosition : MonoBehaviour
{
  

   static public void UpdatePosition()
    {
        int sceneAssets = LayerMask.NameToLayer("Scene Asset");
        int groundLayer = LayerMask.GetMask("Ground");
        GameObject[] allObjects =  Object.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects) 
        {
            if (obj.layer == sceneAssets) 
            {
                Vector3 objPos = obj.transform.position;
                RaycastHit hit;
                Physics.Raycast(new Vector3(objPos.x, 1000f, objPos.z), Vector3.down, out hit, 7000f, groundLayer);
                obj.transform.position = hit.point;
                Debug.Log("ye");
            }
        }
        //Debug.Log(allObjects.Length);
        System.Array.Clear(allObjects,0, allObjects.Length-1);
    }
}
