/* Player Controller Class
 * ----------------------------
 * Handles player movement and animation updating. 
 * 
 *  Created by Ryan Trozzolo 3/22/25:
 * -Added movement and rotation logic
 * -Added anim updating
 * 
 * 
 * Modified by Camron Carr 3/27/25:
 * -Added functions to attach and detach the character. This is used to control the tower while
 * the character sprite is frozen during tower placement.
 * 
 * NOTES:
 * -Fix tower rotation so that controls work properly when rotation occurs.
 */




using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject character;

    Animator2D anim;
    Direction4Way direction;
    public float speed = 3;

    const int IDLE_ANIM = 0;
    const int WALK_ANIM = 1;
    const int ATTACK_ANIM = 2;
    const int IDLE_HAMMER_ANIM = 3;
    const int WALK_HAMMER_ANIM = 4;
    [SerializeField] Collider hammer;
    bool charActive;
    Vector3 oldPos;

    private void Start()
    {
        charActive = true;
        anim = GetComponentInChildren<Animator2D>();
        direction = GetComponentInChildren<Direction4Way>();
        hammer.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckMovement();
    }

    void CheckMovement() // Checks player movement
    {
        Vector3 moveDirection = Vector3.zero;

        // Set movement input keys
        bool isWalkingUp = Input.GetKey(KeyCode.W);
        bool isWalkingDown = Input.GetKey(KeyCode.S);
        bool isWalkingLeft = Input.GetKey(KeyCode.A);
        bool isWalkingRight = Input.GetKey(KeyCode.D);

        // Check movement direction
        if (isWalkingUp) moveDirection += Vector3.back;
        if (isWalkingDown) moveDirection += Vector3.forward;
        if (isWalkingLeft) moveDirection += Vector3.right;
        if (isWalkingRight) moveDirection += Vector3.left;  // Directions are flipped because of the rotation of the camera

        // Only update direction when moving, and keep direction when not moving
        if (moveDirection != Vector3.zero)
        {
            // Convert movement to be relative to the character's rotation
            moveDirection = transform.rotation * moveDirection;

            // Convert movement to local space for animations
            Vector3 localMoveDirection = transform.InverseTransformDirection(moveDirection);
            direction.direction = Vector2Int.RoundToInt(new Vector2(localMoveDirection.x, -localMoveDirection.z));

            // Move character
            transform.position += moveDirection.normalized * speed * Time.deltaTime;
        }
        
        if (charActive)
        {
            
            if (Input.GetKeyDown(KeyCode.Space) && !hammer.gameObject.activeSelf)
            {
                hammer.gameObject.SetActive(true);
                hammer.GetComponent<Direction4Way>().direction = direction.direction;
                hammer.GetComponent<Animator2D>().Start();
                hammer.GetComponent<Animator2D>().RestartAnimation(0);
                Invoke(nameof(DisableHammer), 0.3f);
                hammer.transform.localPosition = new Vector3(direction.direction.x, 0, -direction.direction.y);
            }

            // Set animation state based on movement/attack
            if (hammer.gameObject.activeSelf)
            {
                anim.SetAnimation(ATTACK_ANIM);
            }
            else
            {
                anim.SetAnimation(moveDirection != Vector3.zero ? WALK_ANIM : IDLE_ANIM);

                return;
            }
        }
        
    }

    public void DetachCharacter()
    {
        if(!charActive) return;
        oldPos = transform.position;
        charActive = false;
        character.gameObject.transform.parent = null;

    }

    public void AttachCharacter()
    {
        this.gameObject.transform.position = oldPos;
        charActive = true;
        character.gameObject.transform.parent = this.transform;

    }
    void DisableHammer()
    {
        hammer.gameObject.SetActive(false);
    }

}
