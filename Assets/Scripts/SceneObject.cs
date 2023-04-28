using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObject : MonoBehaviour
{
    [Header("Temporary Transform Values")]
    public Vector3 tempPosition;
    public float tempYRotation;

    private void Start()
    {
        tempPosition = transform.position;
        tempYRotation = transform.localEulerAngles.y;
    }

    void Update()
    {
        switch (SceneInformation.moveSnapIncrement)
        {
            case (0f):
                transform.position = tempPosition;
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, tempYRotation, transform.localEulerAngles.z);
                break;

            default:
                Vector3 roundPosition = Vector3.zero;
                roundPosition.x = Mathf.Round(tempPosition.x / SceneInformation.moveSnapIncrement) * SceneInformation.moveSnapIncrement;
                roundPosition.y = Mathf.Round(tempPosition.y / SceneInformation.moveSnapIncrement) * SceneInformation.moveSnapIncrement;
                roundPosition.z = Mathf.Round(tempPosition.z / SceneInformation.moveSnapIncrement) * SceneInformation.moveSnapIncrement;                

                transform.position = Vector3.Lerp(transform.position, roundPosition, SceneInformation.snapSpeed * Time.deltaTime);


                float roundYRotation = 0f;
                roundYRotation = Mathf.Round(tempYRotation / SceneInformation.rotationSnapIncrement) * SceneInformation.rotationSnapIncrement;

                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
                                                         Mathf.LerpAngle(transform.localEulerAngles.y, roundYRotation, SceneInformation.snapSpeed * Time.deltaTime),
                                                         transform.localEulerAngles.z);

                break;
        }
    }
}
