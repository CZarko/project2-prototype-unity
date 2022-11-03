using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    bool jumpInput;

    void Start()
    {
        SetBoundaries();

        rb = GetComponent<Rigidbody2D>();
        jumpForce = new Vector2(0f, jumpHeight);
        culTime = 0f;
        transform.position = new Vector3(transform.position.x, Random.Range(minY, maxY), transform.position.y);
        
        //GetComponent<Animation>().Play("Tilt");
        
        StartCoroutine(StartBuffer());
    }

    void Update()
    {
        MyInput();
        Jump();
        ConstrainPos();
        CheckDist();
        
    }

    private void MyInput()
    {
        jumpInput = Input.GetKeyDown(KeyCode.Space);
    }

    private void Jump()
    {
        if(jumpInput){
            rb.AddForce(jumpForce);
            GetComponent<Animation>().Play("Squish");
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
           Debug.Log("You lose!");
           popOut.GetComponent<Animation>().Play("PopOut");
           minY = -999;
        }

        if(distBetween < (selfHeight / 2)){
            if(timeset){
                if(((Time.time - startTime) + culTime) > winTime){
                    Debug.Log("You Win!");
                    popOut.GetComponent<Animation>().Play("PopOut");
                    minY = -999;
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

    IEnumerator StartBuffer()
    {
        ableToLose = false;
        yield return new WaitForSeconds (3.0f);
        ableToLose = true;
        Debug.Log("You can lose now!");
    }

    
    
}
