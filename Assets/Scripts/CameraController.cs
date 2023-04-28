using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Translation")]
    public float topSpeed;
    public float accelerationSpeed;
    public float retardationSpeed;

    private Vector3 moveSpeed;

    [Header("Orientation")]
    public float mouseSensitivity;
    public float lookAcceleration;

    private float targetYaw = 0f;       // Y Axis, target values will be clamped
    private float targetPitch = 20f;    // X Axis
    private float yaw = 0f;             // Non-target values will be lerped
    private float pitch = 20f;

    [Header("Height - Scroll wheel")]
    public float heightStepSize;
    public float heightSmoothTime;

    private float height = 0f;
    private float targetHeight = 0f;
    private float heightVelocity;   // Reference for Height SmoothDamp

    [Header("Zoom - Scroll wheel")]
    public float zoomStepSize;
    public float zoomSmoothTime;

    private float zoomPos = -20f;
    private float targetZoom = -20f;
    private float zoomVelocity;     // Reference for Zoom SmoothDamp

    [Header("Other")]
    public Transform cameraArm;
    public Transform mainCamera;
    public MeshRenderer pivotGizmoMeshRenderer;

    private enum cameraState { Move, Look, Idle }
    private cameraState camState;

    private enum scrollWheelState { Height, Zoom }
    private scrollWheelState scrollState;

    void Start()
    {
        camState = cameraState.Move;
        scrollState = scrollWheelState.Zoom;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
            pivotGizmoMeshRenderer.enabled = !pivotGizmoMeshRenderer.enabled;   // Toggle Pivot Gizmo

        float threshold = 2;    // Distance (in pixels) to the screen edge where the mouse starts moving the camera
        Vector3 moveVector = Vector3.zero;
        Vector2 lookVector = Vector2.zero;

        switch (camState)
        {
            case cameraState.Move:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                #region moveVector - Mouse + WASD Inputs
                if ((Input.mousePosition.x < threshold) || (Input.GetKey(KeyCode.A)))                      // Move Left
                    moveVector -= transform.right;
                if ((Input.mousePosition.x > (Screen.width - threshold)) || (Input.GetKey(KeyCode.D)))     // Move Right
                    moveVector += transform.right;
                if ((Input.mousePosition.y < threshold) || (Input.GetKey(KeyCode.S)))                      // Move Back
                    moveVector -= transform.forward;
                if ((Input.mousePosition.y > (Screen.height - threshold)) || (Input.GetKey(KeyCode.W)))    // Move Fwd
                    moveVector += transform.forward;
                #endregion

                if (Input.GetMouseButtonDown(1))
                    camState = cameraState.Look;
                break;

            case cameraState.Look:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                lookVector = new Vector2(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")) * mouseSensitivity;

                #region moveVector - WASD Inputs
                if (Input.GetKey(KeyCode.A))            // Move Left
                    moveVector -= transform.right;
                if (Input.GetKey(KeyCode.D))            // Move Right
                    moveVector += transform.right;
                if (Input.GetKey(KeyCode.S))            // Move Back
                    moveVector -= transform.forward;
                if (Input.GetKey(KeyCode.W))            // Move Fwd
                    moveVector += transform.forward;
                #endregion

                if (Input.GetMouseButtonUp(1))
                    camState = cameraState.Move;
                break;

            case cameraState.Idle:
                break;
        }


        #region Handle Position
        // Normalize Move Vector
        moveVector.Normalize();

        // Linear Acceleration/Braking
        if ((moveVector != Vector3.zero) && (Vector3.Dot(moveVector, moveSpeed) >= 0f))
            moveSpeed = Vector3.MoveTowards(moveSpeed, moveVector * topSpeed, accelerationSpeed * Time.deltaTime);
        else
            moveSpeed = Vector3.MoveTowards(moveSpeed, Vector3.zero, retardationSpeed * Time.deltaTime);

        // Apply Movement
        transform.position += moveSpeed * Time.deltaTime;
        #endregion


        #region Handle Orientation
        // Update Target Orientation
        targetYaw += lookVector.y;
        targetPitch += lookVector.x;

        // Clamp Pitch
        targetPitch = Mathf.Clamp(targetPitch, -90f, 90f);

        // Lerp Yaw and Pitch
        yaw = Mathf.Lerp(yaw, targetYaw, lookAcceleration * Time.deltaTime);
        pitch = Mathf.Lerp(pitch, targetPitch, lookAcceleration * Time.deltaTime);

        // Apply Rotation
        transform.localEulerAngles = new Vector3(0f, yaw, 0f);
        cameraArm.localEulerAngles = new Vector3(pitch, 0f, 0f);
        #endregion


        // Scroll State Input
        if (Input.GetKey(KeyCode.LeftShift))
            scrollState = scrollWheelState.Height;
        else
            scrollState = scrollWheelState.Zoom;

        switch (scrollState)
        {
            case scrollWheelState.Height:
                // SmoothDamp Camera Height
                targetHeight += Input.mouseScrollDelta.y * heightStepSize;
                break;

            case scrollWheelState.Zoom:
                // SmoothDamp Zoom
                targetZoom += Input.mouseScrollDelta.y * zoomStepSize;
                targetZoom = Mathf.Clamp(targetZoom, -100f, -2f);
                break;
        }


        #region Handle Camera Height
        // SmoothDamp Camera Height
        height = Mathf.SmoothDamp(height, targetHeight, ref heightVelocity, heightSmoothTime);  // Pretty sure SmoothDamp takes care of deltaTime out of the box

        // Apply Camera Height
        Vector3 tempObjectPos = cameraArm.position; // We're moving the camera-arm object to avoid overwriting the new position of this game object
        tempObjectPos.y = height;
        cameraArm.position = tempObjectPos;
        #endregion


        #region Handle Camera Zoom
        // SmoothDamp Camera Zoom
        zoomPos = Mathf.SmoothDamp(zoomPos, targetZoom, ref zoomVelocity, zoomSmoothTime);
        
        // Apply Camera Zoom
        Vector3 tempCameraLocalPos = mainCamera.localPosition;
        tempCameraLocalPos.z = zoomPos;
        mainCamera.localPosition = tempCameraLocalPos;
        #endregion
    }
}