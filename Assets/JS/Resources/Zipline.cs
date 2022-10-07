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

        //zipline �ȸ��� �� ���� �ֱ�
        currenttime += Time.deltaTime;
        if(currenttime > maxtime)
        {
            
            this.gameObject.SetActive(false);
            Ziplinemanager.instance.zipshoot = false;
            Destroy(this.gameObject);
            anim.SetTrigger("zipnothit");
        }

        //zip ���ư��� �ӵ�
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
        //�ش� ��ġ�� �̵��ϱ� ���� ��
        Ziplinemanager.instance.hit = true;
        Ziplinemanager.instance.zipshoot = false;
        Ziplinemanager.instance.zipcolpos = collision.contacts[0].point;
        this.gameObject.SetActive(false);
       
        Destroy(this.gameObject);

        // �����ؾ� ��
        // Destroy(this.gameObject);

    }
}
