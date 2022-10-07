using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int arrowSpeed = 20;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * arrowSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // objectPool���� ���� �� �� rb.velocity �ʱ�ȭ
        transform.forward = rb.velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
       // this.gameObject.SetActive(false);
        
       // ObjectPool.instance.deactiveDic[this.gameObject.name].Add(this.gameObject);
    }

}
