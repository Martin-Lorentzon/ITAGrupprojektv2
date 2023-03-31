using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    public float accelerationSpeed;
    public float retardationSpeed;
    public float topSpeed;

    private Vector3 moveSpeed;

    void Start()
    {

    }

    void Update()
    {

        float threshold = Screen.height * 0.06f;    //Distance (in percentage) to the screen edge where the mouse starts moving the camera

        #region moveVector - Inputs
        Vector3 moveVector = Vector3.zero;

        if (Input.mousePosition.x < threshold)                      //Move Left
            moveVector += Vector3.left;
        if (Input.mousePosition.x > (Screen.width - threshold))     //Move Right
            moveVector += Vector3.right;
        if (Input.mousePosition.y < threshold)                      //Move Back
            moveVector += Vector3.back;
        if (Input.mousePosition.y > (Screen.height - threshold))    //Move Fwd
            moveVector += Vector3.forward;

        moveVector.Normalize();
        #endregion

        //Linear Acceleration/Deacceleration
        if ((moveVector != Vector3.zero) && (Vector3.Dot(moveVector, moveSpeed.normalized) > 0f))
            moveSpeed = Vector3.MoveTowards(moveSpeed, moveVector * topSpeed, accelerationSpeed * Time.deltaTime);
        else
            moveSpeed = Vector3.MoveTowards(moveSpeed, moveVector * topSpeed, retardationSpeed * Time.deltaTime);

        //Apply Movement
        transform.position += moveSpeed * Time.deltaTime;


    }
}