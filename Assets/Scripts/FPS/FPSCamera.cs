using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{

    public Transform player;
    public float mouseSensitivity = 100f;
    float cameraVRotation = 0f;

   
    // Start is called before the first frame update
    void Start()
    {
        //hide and lock the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // get mouse input
        float inputX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float inputY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //rotate camera around X
        cameraVRotation -= inputY;
        cameraVRotation = Mathf.Clamp(cameraVRotation, -89f, 89f);
        transform.localEulerAngles = Vector3.right * cameraVRotation;

        //attach camera rotation to player on Y axis
        player.Rotate(Vector3.up * inputX);
    }
}
