using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    public GameObject playerCam;

    Rigidbody rb;

    private GameObject timer;
    private Timer timerScript;

    private GameObject haven;
    Transform havenTransform;



    private void Start()
    {
        //if (!IsOwner) return;
        timer = GameObject.Find("Canvas/Timer");
        timerScript = GameObject.Find("Timer").GetComponent<Timer>();
        haven = GameObject.Find("Haven");
        havenTransform = haven.transform;
        if (IsLocalPlayer)
        {
            Debug.Log("Activating camera for local player.");
            playerCam.gameObject.SetActive(true);

        }
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        if (!IsOwner) return;
    }


    private void Update()
    {
        // ground check
        MyInput();
        //SpeedControl();
        MovePlayer();

        //handle drag
        rb.drag = groundDrag;
    }

    private void FixedUpdate()
    {
    }

    private void MyInput()
    {        
        if (timerScript.CheckIfDead())
        {
            transform.position = havenTransform.position;
            return;
        }
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        //Debug.Log("Moving");
        rb.AddForce(moveDirection.normalized * moveSpeed * Time.deltaTime * 2f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}
