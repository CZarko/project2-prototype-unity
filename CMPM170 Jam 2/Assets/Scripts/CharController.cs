using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float rotSpeed;

    public Transform orientation;
    
    float horzInput;
    float vertInput;

    Vector3 moveDirection;
    Vector3 rotDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        MyInput();
        MovePlayer();
        
    }

    private void MyInput()
    {
        horzInput = Input.GetAxisRaw("Horizontal");
        vertInput = Input.GetAxisRaw("Vertical");
        
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * -vertInput;
        rb.AddForce(moveDirection.normalized * moveSpeed);

        rotDirection = new Vector3(0, horzInput, 0);
        rb.AddTorque(rotDirection * rotSpeed);
    }
}
