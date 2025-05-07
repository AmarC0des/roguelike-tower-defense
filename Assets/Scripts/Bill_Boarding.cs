using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera targetCamera; // Reference to the camera
    Direction4Way direction4Way;

    private PlayerController player;

    void Start()
    {
        targetCamera = Camera.main; // Set to main camera by default
        direction4Way = GetComponent<Direction4Way>();
        player = FindFirstObjectByType<PlayerController>();
    }

    void LateUpdate()
    {


        //find facing direction as a vector2.
        float angle = transform.rotation.eulerAngles.y-90;
        float rad = angle * Mathf.Deg2Rad;
        Vector2Int intendedDirection = Vector2Int.RoundToInt(new Vector2((float)Mathf.Cos(rad), (float)Mathf.Sin(rad)));

        float camAngle = player.transform.rotation.eulerAngles.y-90;
        float camRad = camAngle * Mathf.Deg2Rad;
        Vector2Int camIntendedDirection = Vector2Int.RoundToInt(new Vector2((float)Mathf.Cos(camRad), (float)Mathf.Sin(camRad)));

        
        Vector2Int vector2Int;
        Vector2 intendedPlusCam = intendedDirection + camIntendedDirection;
        if(intendedDirection-camIntendedDirection == Vector2Int.zero)
        {
            vector2Int = Vector2Int.down;
        }
        else if(intendedDirection+camIntendedDirection == Vector2Int.zero)
        {
            vector2Int = Vector2Int.up;
        }
        else if(intendedDirection.x * camIntendedDirection.y == -1 || intendedDirection.y * camIntendedDirection.x == 1)
        {
            vector2Int = Vector2Int.right;
        }
        else if(intendedDirection.x * camIntendedDirection.y == 1 || intendedDirection.y * camIntendedDirection.x == -1)
        {
            vector2Int = Vector2Int.left;
        }
        else
        {
            vector2Int = Vector2Int.left;
        }
        direction4Way.direction = vector2Int;
        /*
        enemy    cam
        {-1, 0}, { 0, 1} = {-1, 0}
        { 1, 0}, { 0, 1} = { 1, 0}
        {-1, 0}, { 0,-1} = { 1, 0}
        { 1, 0}, { 0,-1} = {-1, 0}
        { 0, 1}, { 1, 0} = {-1, 0}
        { 0,-1}, { 1, 0} = { 1, 0}
        { 0, 1}, {-1, 0} = { 1, 0}
        { 0,-1}, {-1, 0} = {-1, 0}
        
        */

        if (targetCamera != null)
        {
            // Make the sprite/object always face the camera
            Vector3 direction = targetCamera.transform.position - transform.position;
            direction.y = 0; // Keeps the sprite/object upright

            transform.rotation = Quaternion.LookRotation(-direction); // Faces the camera
        }


    }
}
