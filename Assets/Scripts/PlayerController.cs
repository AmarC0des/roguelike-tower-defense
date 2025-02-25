using System.Collections;
using System.Collections.Generic;
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
        if (isWalkingLeft) moveDirection += Vector3.right; //these have to be flipped because the player is rotated 90 degrees in the scene.
        if (isWalkingRight) moveDirection += Vector3.left; //

        //only update direction when moving, and keep direction when not moving.
        if (moveDirection != Vector3.zero)
        {
            playerAnim.SetFloat("XDir", moveDirection.x);
            playerAnim.SetFloat("YDir", moveDirection.z);
        }

        playerAnim.SetBool("Walking", moveDirection != Vector3.zero);

        //Move
        playerPos.position += moveDirection * speed * Time.deltaTime;


    }


}
