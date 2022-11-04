using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FishBoxController : MonoBehaviour
{
    public GameObject bar;
    public GameObject fishIcon;
    RectTransform barRt;
    RectTransform selfRt;

    float barHeight;
    float selfHeight;

    float maxY;
    float minY;
    
    public float jumpHeight;
    Vector2 jumpForce;
    Rigidbody2D rb;

    public float loseDist;
    public float winTime;
    float startTime;
    float culTime;
    bool ableToLose;
    bool timeset;

    public GameObject popOut;
    public GameObject toDelete;

    bool jumpInput;
    float horzInput;
    float vertInput;

    FMOD.Studio.EventInstance reelAmb;

    void Start()
    {
        SetBoundaries();

        rb = GetComponent<Rigidbody2D>();
        jumpForce = new Vector2(0f, jumpHeight);
        culTime = 0f;
        transform.position = new Vector3(transform.position.x, Random.Range(0, maxY), transform.position.y);
        Debug.Log("Pos : " + transform.position.y);
        Debug.Log("Rand : " + Random.Range(minY, maxY));
        
        reelAmb = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/fishingAmb");
        RuntimeManager.CreateInstance("event:/MUS/changeToFishing").start();
        reelAmb.start();

        startTime = Time.time;
        
        StartCoroutine(StartBuffer());
    }

    void Update()
    {
        MyInput();
        Jump();
        ConstrainPos();
        CheckDist();
        CheckMovement();
    }

    private void MyInput()
    {
        jumpInput = Input.GetKeyDown(KeyCode.Space);
        horzInput = Input.GetAxisRaw("Horizontal");
        vertInput = Input.GetAxisRaw("Vertical");
    }

    private void Jump()
    {
        if(jumpInput){
            rb.AddForce(jumpForce);
            GetComponent<Animation>().Play("Squish");
            RuntimeManager.CreateInstance("event:/SFX/minigamePress").start();
        }
    }

    private void ConstrainPos()
    {   
        float newY = Mathf.Max(Mathf.Min(transform.position.y, maxY), minY);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void SetBoundaries()
    {
        barRt = (RectTransform)bar.transform;
        selfRt = (RectTransform)this.transform;
        barHeight = barRt.rect.height;
        selfHeight = selfRt.rect.height;

        minY = selfHeight / 2f;
        maxY = barHeight - (selfHeight / 4f);
    }

    private void CheckDist(){
        float distBetween = Mathf.Abs(this.transform.position.y - fishIcon.transform.position.y);

        if(ableToLose && distBetween > loseDist){
           //Debug.Log("You lose!");
           ExitMinigame(false);
        }

        if(distBetween <= (selfHeight / 4)){
            if(timeset){
                if(((Time.time - startTime) + culTime) > winTime){
                    //Debug.Log("You Win!");
                    ExitMinigame(true);
                }
            }
            else{
                timeset = true;
                culTime += Time.time - startTime;
                startTime = Time.time;
            }
        }
        else{
            timeset = false;
        }
    }

    private void ExitMinigame(bool winCondition){
        popOut.GetComponent<Animation>().Play("PopOut");
        reelAmb.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        if (winCondition)
        {
            RuntimeManager.CreateInstance("event:/SFX/fishSplash").start();
        }
        else
        {
            RuntimeManager.CreateInstance("event:/SFX/whipSound").start();
        }
        minY = -999;
        RuntimeManager.CreateInstance("event:/MUS/changeToOverworld").start();
        StartCoroutine(Delete());
    }

    private void CheckMovement()
    {
        if(horzInput != 0 || vertInput != 0)
        {
            ExitMinigame(false);
        }
    }
    IEnumerator StartBuffer()
    {
        ableToLose = false;
        yield return new WaitForSeconds (3.0f);
        ableToLose = true;
        //Debug.Log("You can lose now!");
    }

    IEnumerator Delete()
    {
        yield return new WaitForSeconds (1.0f);
        Destroy(toDelete);
    }
    
    
}
