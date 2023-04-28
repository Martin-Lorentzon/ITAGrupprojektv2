using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float playerSpeed = 5f;

    //adding gravity variables
    float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;
    bool isGrounded;

    private bool isStart;

    // Start is called before the first frame update
    void Start()
    {
        isStart = true;
        groundMask = LayerMask.GetMask("Ground", "Scene Asset");
        Debug.Log(SceneInformation.focusPoint);



        transform.position = SceneInformation.focusPoint;

        isStart = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (isStart)
            return;


        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2F;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

       controller.Move(move * playerSpeed * Time.deltaTime);
        // gravity addition
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }
}
