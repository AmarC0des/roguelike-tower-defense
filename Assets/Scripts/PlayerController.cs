using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator2D anim;
    Direction4Way direction;
    public float speed = 3;
    const int IDLE_ANIM = 0;
    const int WALK_ANIM = 1;
    const int ATTACK_ANIM = 2;
    const int IDLE_HAMMER_ANIM = 3;
    const int WALK_HAMMER_ANIM = 4;


    private void Start()
    {
        anim = GetComponent<Animator2D>();
        direction = GetComponent<Direction4Way>();
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
        if (isWalkingLeft) moveDirection += Vector3.right; //these have to be flipped because the player is rotated in the scene.
        if (isWalkingRight) moveDirection += Vector3.left; //

        //only update direction when moving, and keep direction when not moving.
        if (moveDirection != Vector3.zero)
        {
            direction.direction = Vector2Int.RoundToInt(new Vector2(moveDirection.x, -moveDirection.z));
        }

        anim.SetAnimation(moveDirection != Vector3.zero ? WALK_ANIM : IDLE_ANIM);

        //Move
        transform.position += moveDirection * speed * Time.deltaTime;


    }


}
