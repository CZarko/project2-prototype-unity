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

    [Header("Audio")]
    public int moveAudioLayers;
    int moveAudioIndex;
    bool soundPlaying;
    FMOD.Studio.EventInstance[] boatMove;
    
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        CreateAudioArray();
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
        boatMove[moveAudioIndex].set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject));
        
        if(!soundPlaying && isMoving){
            soundPlaying = true;
            boatMove[moveAudioIndex].start();
        }
        else if(soundPlaying && !isMoving){
            soundPlaying = false;
            boatMove[moveAudioIndex].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            moveAudioIndex = (moveAudioIndex + 1) % moveAudioLayers;
        }
    }

    private void CreateAudioArray(){
        boatMove = new FMOD.Studio.EventInstance[moveAudioLayers];
        
        for(moveAudioIndex = 0; moveAudioIndex < moveAudioLayers; moveAudioIndex++){
            boatMove[moveAudioIndex] = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/boatMove");
            boatMove[moveAudioIndex].set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject));
        }
        moveAudioIndex = 0;
    }
}
