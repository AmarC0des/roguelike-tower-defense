using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera targetCamera; // Reference to the camera

    void Start()
    {
        targetCamera = Camera.main; // Set to main camera by default
    }

    void LateUpdate()
    {
        if (targetCamera != null)
        {
            // Make the sprite/object always face the camera
            Vector3 direction = targetCamera.transform.position - transform.position;
            direction.y = 0; // Keeps the sprite/object upright

            transform.rotation = Quaternion.LookRotation(-direction); // Faces the camera
        }
    }
}
