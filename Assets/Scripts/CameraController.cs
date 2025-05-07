using UnityEngine;
using UnityEngine.TextCore.Text;

//This class will eventually be absorbed by GameManager or PlayerController
//Commented code is for mouse rotation for future consideration
public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 3f;
    public GameObject topView;
    public GameObject frontView;
    private float currentRotation = 0f; // Tracks the current rotation angle
    private float targetRotation = 0f;
    public GameObject charSprite;
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

       
        currentRotation = Mathf.LerpAngle(currentRotation, targetRotation, Time.deltaTime * 5);
        transform.rotation = Quaternion.Euler(0, currentRotation, 0);
    }




    // Rotates the camera 90 degrees in the specified direction
    void RotateCamera(float angle)
    {
        targetRotation += angle;
    }

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
