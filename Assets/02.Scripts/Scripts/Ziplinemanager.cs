using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ziplinemanager : MonoBehaviour
{
    public static Ziplinemanager instance;
   

    private void Awake()
    {
        instance = this;
    }
    public float zipflyspeed = 3f;
    public bool zipshoot;
    public bool zipshootidle;
    public bool hit;
    public Vector3 zipcolpos;
    
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void setzipnothit()
    {
       
    }
}
