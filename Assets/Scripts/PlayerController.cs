using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform playerPos;
    public Animator playerAnim;
    public float speed = 3;


    private void Start()
    {
        playerPos = gameObject.transform;
        playerAnim = gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckMovement();
    }

    void CheckMovement() // Checks player movement
    {
        Vector3 moveDirection = Vector3.zero;

        //set codes
        bool isWalkingUp = Input.GetKey(KeyCode.W);
        bool isWalkingDown = Input.GetKey(KeyCode.S);
        bool isWalkingLeft = Input.GetKey(KeyCode.A);
        bool isWalkingRight = Input.GetKey(KeyCode.D);

        //Check movement direction
        if (isWalkingUp) moveDirection += Vector3.back;
        if (isWalkingDown) moveDirection += Vector3.forward;
        if (isWalkingLeft) moveDirection += Vector3.right;
        if (isWalkingRight) moveDirection += Vector3.left;

        //Move
        playerPos.position += moveDirection * speed * Time.deltaTime;

        //Update animation
        playerAnim.SetBool("WalkingUp", isWalkingUp);
        playerAnim.SetBool("WalkingDown", isWalkingDown);
        playerAnim.SetBool("WalkingLeft", isWalkingLeft);
        playerAnim.SetBool("WalkingRight", isWalkingRight);

        //Set Idling if no movement is happening
        bool isMoving = isWalkingUp || isWalkingDown || isWalkingLeft || isWalkingRight;
        playerAnim.SetBool("Idling", !isMoving);
    }


}
