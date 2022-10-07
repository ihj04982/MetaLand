using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zipline : MonoBehaviour
{
    float zipShootingSpeed = 15f;
    float currenttime;
    float maxtime = 1.0f;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GameObject.Find("Animation_Boy").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        //zipline 안맞을 시 없애 주기
        currenttime += Time.deltaTime;
        if(currenttime > maxtime)
        {
            
            this.gameObject.SetActive(false);
            Ziplinemanager.instance.zipshoot = false;
            Destroy(this.gameObject);
            anim.SetTrigger("zipnothit");
        }

        //zip 날아가는 속도
        Vector3 vir = transform.forward;
        vir.Normalize();
        transform.position += vir * zipShootingSpeed * Time.deltaTime;
/*
        zipMaxDistance = Vector3.Distance(zip.transform.position, firePosition.transform.position);
        if (zipMaxDistance > 1)
        {
            zipline.SetActive(false);
    }*/
    }
    private void OnCollisionEnter(Collision collision)
    {
        //해당 위치로 이동하기 위한 것
        Ziplinemanager.instance.hit = true;
        Ziplinemanager.instance.zipshoot = false;
        Ziplinemanager.instance.zipcolpos = collision.contacts[0].point;
        this.gameObject.SetActive(false);
       
        Destroy(this.gameObject);

        // 수정해야 함
        // Destroy(this.gameObject);

    }
}
