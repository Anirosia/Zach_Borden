using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    float crouchedSpeed = 2f;
    float sprintSpeed = 5f;
    public float moveSpeed;
    private float defultSpeed = 3f;

    public float jumpForce;
    public float gravityScale;

    private CharacterController controller;
    public Animator ani;
    public Transform pivot;
    public float rotateSpeed;
    private Vector3 moveDirection;
    public GameObject playerModel;

    bool crouched;
    bool sprint;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        crouched = false;
        moveSpeed = defultSpeed;
        
    }

    
    void Update()
    {
        //moveDirection = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, moveDirection.y, Input.GetAxis("Vertical") * moveSpeed);
        float yStore = moveDirection.y;
        moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        moveDirection = moveDirection.normalized * moveSpeed;
        moveDirection.y = yStore;

        //------------------------------------------------------------------------------------------------INPUT_GETS 
        if (controller.isGrounded)
        {
            moveDirection.y = 0f;
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;
            }

        }
        if (crouched)
        {
            moveSpeed = crouchedSpeed;
            ani.SetBool("Crouched", true);
        }
        else
        {
            moveSpeed = defultSpeed;
            ani.SetBool("Crouched", false);
        }
        if (sprint)
        {
            moveSpeed = sprintSpeed;
            ani.SetFloat("Switch", 2f);

        }
        else
        {
            moveSpeed = defultSpeed;
            ani.SetFloat("Switch", 0);
        }
        

        //Moving Player
        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale);
        controller.Move(moveDirection * Time.deltaTime);

        

        // Move player based on pivot direction not camera direction
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            transform.rotation = Quaternion.Euler(0.0f, pivot.rotation.eulerAngles.y, 0.0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }

        ani.SetBool("isGrounded", controller.isGrounded);
        ani.SetFloat("Speed", (Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal"))));

        //--------------------------------------------------------------------------------------------------------INPUT_SETS
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Crouched");
            if (!crouched)
            {
                Debug.Log("Crouched");
                crouched = true;
            }
            else
            {
                Debug.Log("Not crouched");
                crouched = false;
            }

        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!sprint)
            {
                sprint = true;
            }
            else
            {
                sprint = false;
            }
        }
            
        

    }
}
