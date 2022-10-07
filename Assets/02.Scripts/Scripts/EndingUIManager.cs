using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingUIManager : MonoBehaviour
{
    private float currentTime;
    Canvas canvas;
    public float showTime;

    // Start is called before the first frame update
    void Start()
    {
         canvas = GetComponent<Canvas>();
        canvas.enabled =false;

    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= showTime)
        {
            canvas.enabled = true;
        }
    }
}
