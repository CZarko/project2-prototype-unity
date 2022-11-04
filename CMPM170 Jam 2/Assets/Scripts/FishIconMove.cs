using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishIconMove : MonoBehaviour
{
    public float rate;
    public float amount;
    public float rate2;
    public float amount2;
    public float initOffset;
    public GameObject bar;
    float orgPos;

    void Start()
    {
        orgPos = this.transform.position.y;
        transform.position = new Vector3(transform.position.x, transform.position.y + Random.Range(initOffset, -initOffset), transform.position.y);
        
        rate *= (Random.value * 4) - 2;
        amount *= (Random.value * 4) - 2;
        rate2 *= (Random.value * 4) - 2;
        amount2 *= (Random.value * 4) - 2;
    }

    void Update()
    {
        orgPos = bar.transform.position.y;
        float ran = orgPos + (Mathf.Sin(Mathf.Tan(rate2 * Time.time)) * amount2) + Mathf.Sin(rate * Time.time) * amount - Mathf.Cos(500 + amount2 * Time.time) * amount / 4;
        transform.position = new Vector3(transform.position.x, ran, transform.position.y);
    }
}
