using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class will eventually be absorbed by GameManager or PlayerController
//Commented code is for mouse rotation for future consideration
public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 3f;
    public GameObject topView;
    public GameObject frontView;
    private float currentRotation = 0f; // Tracks the current rotation angle

    // Variables for mouse rotation option
    // private float mouseX, mouseY;
    // private bool isFreeLook = true; // Flag to toggle free camera rotation

    private void Start()
    {
        ChangeToFrontView();

        // Next two statements for mouse rotation
        // Cursor.lockState = CursorLockMode.Locked; --Locks the cursor for better control
        // Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeToTopView();
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeToFrontView();

        // Rotate camera using Q and E keys
        if (Input.GetKeyDown(KeyCode.Q)) RotateCamera(-90f); // Counterclockwise
        if (Input.GetKeyDown(KeyCode.E)) RotateCamera(90f);  // Clockwise

        /* For Camera rotation with mouse if needed
        if (isFreeLook)
        {
            RotateCamera();
        } 
        */
    }

    // Rotates the camera 90 degrees in the specified direction
    void RotateCamera(float angle)
    {
        currentRotation += angle;
        transform.rotation = Quaternion.Euler(0, currentRotation, 0);
    }

    // Handles free camera rotation around the character
    /* Rotation with mouse alternative option
    void RotateCamera()
    {
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -30, 60); // Limits vertical rotation

        // Rotates the character itself
        transform.rotation = Quaternion.Euler(0, mouseX, 0);
    } 
    */

    void ChangeToTopView()
    {
        //isFreeLook = false; // Disables free rotation
        topView.SetActive(true);
        frontView.SetActive(false);
    }

    void ChangeToFrontView()
    {
        //isFreeLook = true; // Enables free rotation
        topView.SetActive(false);
        frontView.SetActive(true);
    }
}
