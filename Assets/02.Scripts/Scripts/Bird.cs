using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public Animator anim;

    Rigidbody rb;
    Collider col;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    IEnumerator IEFly()
    {
        anim.SetTrigger("MOVE");
        rb.velocity = Vector3.zero;

        rb.AddRelativeForce(0, 10, 5, ForceMode.Impulse);
        col.enabled = false;
        
        yield return new WaitForSeconds(0.7f);

        Destroy(gameObject);
    }

}
