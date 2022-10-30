using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

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

    bool soundPlaying;
    FMOD.Studio.EventInstance boatMove;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        boatMove = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/boatMove");
    }

    private void FixedUpdate()
    {
        MyInput();
        MovePlayer();
        PlaySFX();
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

    private void PlaySFX()
    {
        bool isMoving = (horzInput != 0 || vertInput != 0);
        boatMove.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject));
        
        if(!soundPlaying && isMoving){
            soundPlaying = true;
            boatMove.start();
        }
        else if(soundPlaying && !isMoving){
            soundPlaying = false;
            boatMove.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
