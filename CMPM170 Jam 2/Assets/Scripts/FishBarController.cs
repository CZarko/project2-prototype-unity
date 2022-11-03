using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class FishBarController : MonoBehaviour
{
    public GameObject bar;
    public GameObject tag;
    public Image redBar;
    public float offset;

    RectTransform rt;
    float tagHeight;
    float barHeight;
    float fillAmount;

    void Start()
    {
        rt = (RectTransform)bar.transform;
        barHeight = rt.rect.height;
    }

    void Update()
    {
        tagHeight = tag.transform.position.y;
        fillAmount = tagHeight / barHeight;
        redBar.fillAmount = fillAmount - offset;
    }
}
