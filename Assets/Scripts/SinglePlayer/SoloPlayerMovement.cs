using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoloPlayerMovement : MonoBehaviour
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

    public CardUIManager cardUIManager;



    private void Start()
    {
        //if (!IsOwner) return;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

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
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    public void SpeedUp()
    {
        moveSpeed *= 1.5f;
        Invoke("SpeedDown", 5f);
        cardUIManager.UpdateUtilLogo(1);
    }

    public void SpeedDown()
    {
        moveSpeed /= 1.5f;
        Debug.Log("Speed Down");
        cardUIManager.UpdateUtilLogo(0);
    }
}
