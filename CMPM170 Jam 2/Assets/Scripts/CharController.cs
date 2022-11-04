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
    bool fishInput;

    Vector3 moveDirection;
    Vector3 rotDirection;

    Rigidbody rb;

    [Header("Audio")]
    public int moveAudioLayers;
    int moveAudioIndex;
    bool soundPlaying;
    FMOD.Studio.EventInstance[] boatMove;

    [Header("Fishing")]
    public GameObject fishingGame;
    GameObject currentFishingGame;
    public float minFishTime;
    public float maxFishTime;
    float fishTime;
    bool waitingForFish;
    
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        waitingForFish = false;
        System.DateTime curTime = System.DateTime.Now;
        Random.seed = (int)curTime.Ticks;


        CreateAudioArray();
    }

    private void Update()
    {
        MyInput();
        CheckFishingStart(); 
    }
    
    private void FixedUpdate()
    {
        MovePlayer();
        PlaySFX();
    }

    private void MyInput()
    {
        horzInput = Input.GetAxisRaw("Horizontal");
        vertInput = Input.GetAxisRaw("Vertical");
        fishInput = Input.GetKeyDown(KeyCode.Space);
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

    private void CreateAudioArray()
    {
        boatMove = new FMOD.Studio.EventInstance[moveAudioLayers];
        
        for(moveAudioIndex = 0; moveAudioIndex < moveAudioLayers; moveAudioIndex++){
            boatMove[moveAudioIndex] = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/boatMove");
            boatMove[moveAudioIndex].set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(this.gameObject));
        }
        moveAudioIndex = 0;
    }

    private void CheckFishingStart(){

        if(fishInput && GameObject.Find("FishingMiniGame(Clone)") == null){
            if(!waitingForFish){
                StartFishing();
            }
            else{
                StartFishing();
            }
        }

        if(horzInput != 0  || vertInput != 0){
            waitingForFish = false;
            fishTime = 0f;
        }
    }

    private void StartFishing(){
        fishTime = Random.Range(minFishTime, maxFishTime);
        Debug.Log("FISHTIME: " + fishTime);
        StartCoroutine(StartFishingTime(fishTime));
        RuntimeManager.CreateInstance("event:/SFX/lineCast").start();
        waitingForFish = true;
    }

    IEnumerator StartFishingTime(float timeBuffer)
    {
        yield return new WaitForSeconds (timeBuffer);
        if(timeBuffer == fishTime){
            currentFishingGame = Instantiate(fishingGame);
            waitingForFish = false;
        }
    }
}
