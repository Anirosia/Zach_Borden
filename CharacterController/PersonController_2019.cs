using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonController : MonoBehaviour
{
    Vector3 playerMovment;

    private const float STARTSPEED = 4f;
    private const float SLOPESPEED = 2.5f;
    private const float AIRSPEED = 3.25f;
    private const float SWINGSPEED = 1.5f;

    private Vector3 StartGravity = new Vector3(0.0f, -9.8f, 0.0f);
    private Vector3 SlopeGravity = new Vector3(0.0f, -100f, 0.0f);

    public float speed = STARTSPEED;
    public float rotateSpeed;
    public float rayLength;
    public float jumpForce;
    public float playerHealth;

    float moveSpeed;
    float jumpDelay;
    public float deathDelay = 2;
    float maxSlopeHeight = 70f;
    float minSlopeHeight = 90f;
    float jumpTime;

    public LayerMask groundLayer;
    public LayerMask lookatMask;

    public DialogueTrigger_CW trigger;
    private HealthLoss hearts;

    //public GameObject triggerOBJ;
    public GameObject HealthLossREF;
    public GameObject character;
    public GameObject SlopeDetect;
    public GameObject camPivot;
    public BoxCollider swordCollider;

    public Transform pivot;
    Transform objectHit;

    Animator ani;
    Rigidbody rb;
    CapsuleCollider col;

    bool crouched;
    bool sprint;
    bool SwordMode;
    bool jumpable;
    bool jumping;
    public bool dead;

    void Start()
    {
        col = GetComponent<CapsuleCollider>();
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        hearts = HealthLossREF.GetComponent<HealthLoss>();

        SwordMode = false;
        jumpable = true;

    }

    void FixedUpdate()
    {
        //RaycastHit hitSlopeUp;

        //RaycastHit hitSlopeDown;

        ////Finding upwards slopes 
        //if (Physics.Raycast(gameObject.transform.position, Vector3.forward, out hitSlopeUp, rayLength))
        //{
        //    if (hitSlopeUp.collider.gameObject.layer == 0)
        //    {
        //        float slopeAngleUp = Vector3.Angle(hitSlopeUp.normal, Vector3.up);

        //        if(slopeAngleUp >= maxSlopeHeight)
        //        {
        //            speed = SLOPESPEED;
        //            Physics.gravity = SlopeGravity;
        //        }
        //        else
        //        {
        //            speed = STARTSPEED;
        //            Physics.gravity = StartGravity;
        //        }

        //    }
        //}

        //Finding downwards slopes
        //if (Physics.Raycast(SlopeDetect.transform.position, Vector3.forward, out hitSlopeDown, rayLength))
        //{
        //    if (hitSlopeDown.collider.gameObject.layer == 0)
        //    {
        //        float slopeAngleDown = Vector3.Angle(hitSlopeDown.normal, Vector3.down);

        //        if (slopeAngleDown <= minSlopeHeight)
        //        {
        //            speed = SLOPESPEED;
        //            Physics.gravity = SlopeGravity;
        //        }
        //        else
        //        {
        //            speed = STARTSPEED;
        //            Physics.gravity = StartGravity;
        //        }
        //    }
        //}

        if (!dead)
        {
            movementManager();
            AirManager();
            if (!trigger.talking)
            {
            
                Sword();
            }
        }
        else
        {
            ani.SetBool("Dead", false);
            deathDelay -= Time.deltaTime;
        }
    }

    void movementManager()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");


        //FREERUNMODE
        if (!SwordMode)
        {
            //Move with camPivot because camPivot is not rotated while the cam itself has a rotation of 33        
            playerMovment = (camPivot.transform.forward * ver * speed * Time.deltaTime) + (camPivot.transform.right * hor * speed * Time.deltaTime);
            Vector3.Normalize(playerMovment * Time.deltaTime);
            transform.Translate(playerMovment, Space.World);

            //Rotate player when moving 
            if (hor != 0 || ver != 0)
            {
                transform.rotation = Quaternion.Euler(0.0f, pivot.rotation.eulerAngles.y, 0.0f);
                Quaternion newRotation = Quaternion.LookRotation(new Vector3(playerMovment.x, 0f, playerMovment.z));
                character.transform.rotation = Quaternion.Slerp(character.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
                moveSpeed = 1;                

            }
            //Not Moving 
            else
            {
                moveSpeed = 0;
            }

        }
        //SWORDMODE
        else
        {
            //RayCast from camera to mouse
            Vector3 mousePosition = -Vector3.one;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, lookatMask))
            {
                //Look rotation of the player to be the mouse.
                mousePosition = hit.point;
                Vector3 forward = mousePosition - character.transform.position;
                character.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
            }

            //Move Player
            playerMovment = (camPivot.transform.forward * ver * speed * Time.deltaTime) + (camPivot.transform.right * hor * speed * Time.deltaTime);
            Vector3.Normalize(playerMovment * Time.deltaTime);
            transform.Translate(playerMovment, Space.World);

            //Check the way the player is faceing(forward) to find rotation; find what Quaternion he is faces and base strafe ani off that direction.
            //after finding character forward subtract 90Degrees to find right; take the - of that value to find left; take forward -180Degrees to find back. 
            //Facing Forward
            if (character.transform.eulerAngles.y >= 0f && character.transform.eulerAngles.y <= 80f)
            {
                ani.SetFloat("Hor", hor);
                ani.SetFloat("Ver", ver);
            }
            //Facing Right
            else if (character.transform.eulerAngles.y >= 81f && character.transform.eulerAngles.y <= 190f)
            {   
                ani.SetFloat("Hor", ver);
                ani.SetFloat("Ver", hor);
            }
            //Facing Backwards
            else if (character.transform.eulerAngles.y >= 191f && character.transform.eulerAngles.y <= 285f)
            {   
                ani.SetFloat("Hor", -hor);
                ani.SetFloat("Ver", -ver);
            }
            //Facing Left
            else if (character.transform.eulerAngles.y >= 286f && character.transform.eulerAngles.y <= 360f) 
            {
                ani.SetFloat("Hor", -ver);
                ani.SetFloat("Ver", -hor);
            }

            //Play Ani 
            if (hor != 0 || ver != 0)
            {
                moveSpeed = 1;
            }
            //Not Moving 
            else
            {
                moveSpeed = 0;
            }
        }
        
    }

    void AirManager()
    {
        //CHECK IS GROUND METHOD
        if (IsGround())
        {

            speed = STARTSPEED;
            //---------------------------------------------------Animation sets
            ani.SetBool("isGrounded", true);

            ani.SetFloat("moveSpeed", moveSpeed);

            //-----------------------------------------------------KeyPress Input
            //Jumping
            //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            if (Input.GetKey(KeyCode.Mouse1))
            {
                if (SwordMode)
                {
                    SwordMode = false;
                }
                else
                {
                    SwordMode = true;
                }
            }

            if (jumpable)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    jumpDelay = 0.2f;
                    jumpTime = 0.5f;
                    jumping = true;
                    jumpable = false;
                }
            }
            else
            {
                JumpTimeCountDown();
            }

            if (jumping)
            {
                JumpAni();
            }
            
        }
        else 
        {
            //  IN AIR
            ani.SetBool("isGrounded", false);
            speed = AIRSPEED;

        }
    }
    void JumpAni()
    {
        jumpDelay -= Time.deltaTime;
        if (jumpDelay < 0)
        {
            rb.velocity = Vector3.up * jumpForce;
            jumping = false;
        }
    }

    void JumpTimeCountDown() //For the people who spam space.
    {
        jumpTime -= Time.deltaTime;
        if (jumpTime < 0)
        {
            jumpable = true;
        }
    }

    void Sword()
    {
        //LEFTMOUSECLICK
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ani.SetBool("Swing", true);
            speed = SWINGSPEED;
        }
        else
        {
            ani.SetBool("Swing", false);
            speed = STARTSPEED;
        }

        //SwordModeBOOL
        if (SwordMode)
        {
            ani.SetBool("SwordMode", true);
            speed = SWINGSPEED;
        }
        else
        {
            ani.SetBool("SwordMode", false);
            speed = STARTSPEED;
        }

    }

    public void TakeDamage(float damage)
    {
        hearts.lossAHeart();
        playerHealth = playerHealth - damage;
        if(playerHealth <= 0)
        {
            Dead();
        }
    }

    void Dead()
    {
        ani.SetBool("Dead", true);
        dead = true;
    }

    public void EnableSword()
    {
        swordCollider.enabled = true;
    }

    public void DisableSword()
    {
        swordCollider.enabled = false;
    }

    private bool IsGround()
    {
        //Casts a Capsule below the character to check if he is on a layerMask the is ground 
        return Physics.CheckCapsule(col.bounds.center, new Vector3(col.bounds.center.x,
                col.bounds.min.y, col.bounds.center.z), col.radius * .8f, groundLayer);
    }
}

   
 





