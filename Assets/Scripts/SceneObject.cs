using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObject : MonoBehaviour
{

    [Header("Snap Values")]
    public float moveSnapIncrement;
    public float rotationSnapAngle;

    [Header("Snap Settings")]
    public float snapSpeed;

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
        switch (moveSnapIncrement)
        {
            case (0f):
                transform.position = tempPosition;
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, tempYRotation, transform.localEulerAngles.z);
                break;

            default:
                Vector3 roundPosition = Vector3.zero;
                roundPosition.x = Mathf.Round(tempPosition.x / moveSnapIncrement) * moveSnapIncrement;
                roundPosition.y = Mathf.Round(tempPosition.y / moveSnapIncrement) * moveSnapIncrement;
                roundPosition.z = Mathf.Round(tempPosition.z / moveSnapIncrement) * moveSnapIncrement;

                transform.position = Vector3.Lerp(transform.position, roundPosition, snapSpeed * Time.deltaTime);


                float roundYRotation = 0f;
                roundYRotation = Mathf.Round(tempYRotation / rotationSnapAngle) * rotationSnapAngle;

                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
                                                         Mathf.LerpAngle(transform.localEulerAngles.y, roundYRotation, snapSpeed * Time.deltaTime),
                                                         transform.localEulerAngles.z);

                break;
        }
    }
}
