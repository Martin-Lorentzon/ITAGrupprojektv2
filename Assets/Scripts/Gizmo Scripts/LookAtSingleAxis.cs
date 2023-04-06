using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtSingleAxis : MonoBehaviour
{
    public enum axisEnum { x, y, z };
    public axisEnum axis;

    public Transform targetObject;

    void Start()
    {
        
    }

    void Update()
    {
        switch (axis)
        {
            case axisEnum.x:
                var lookPos = targetObject.position - transform.position;
                lookPos.x = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = rotation;

                transform.localEulerAngles += new Vector3(90f, 0f, 0f);
                break;
            case axisEnum.y:
                break;
            case axisEnum.z:
                break;
        }
    }
}
