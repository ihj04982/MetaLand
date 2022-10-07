using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tmp = Camera.main.transform.forward;

        transform.forward = new Vector3(tmp.x, 0, tmp.z);
    }
}
