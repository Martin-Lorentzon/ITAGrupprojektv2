using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookRotationAxis : MonoBehaviour
{
    public Transform targetTransform;
    public enum axisEnum { x, y, z };
    public axisEnum axis;
    public Vector3 eulerOffset;

    public bool targetMainCamera;

    private void Start()
    {
        if (targetMainCamera)
            targetTransform = GameObject.Find("Main Camera").transform;
    }

    void Update()
    {
        Quaternion rotationOffset = Quaternion.Euler(eulerOffset);
        Vector3 lookPos = targetTransform.position - transform.position;

        switch (axis)
        {
            case axisEnum.x:
                lookPos.x = 0;
                break;
            case axisEnum.y:
                lookPos.y = 0;
                break;
            case axisEnum.z:
                lookPos.z = 0;
                break;
        }
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation * rotationOffset;
    }
}
