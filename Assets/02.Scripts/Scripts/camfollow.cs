using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camfollow : MonoBehaviour
{
    public Camera cam;
    public float wheelSpeed = 10f;
    public float camRotSpeed = 2.0f;
    public float currentZoom = 7.0f;
    public float minZoom = 5.0f;
    public float maxZoom = 20.0f;
    public Transform target;
    public Vector3 offset;
        //= new Vector3(-0.31f, 3.08f, -5.51f);
   // bool scrollup = true;


    void Start()
    {
        
        Vector3 tmp = transform.position - target.position;
        tmp.Normalize();
        offset = tmp;


    }
    void Update()
    {
        
        if (Input.GetKey("q"))// right
        {
            // rotate toward left Yaxis
            transform.RotateAround(target.position, Vector3.up, camRotSpeed);
            offset = transform.position - target.position;
            offset.Normalize();
        }
        if (Input.GetKey("e")) // left
        {
            transform.RotateAround(target.position, Vector3.up, -camRotSpeed);
            offset = transform.position - target.position;
            offset.Normalize();
        }
        
        if (Input.GetKey("c") && (transform.rotation.eulerAngles.x < 89)) // up
        {
            
            transform.RotateAround(target.position, this.transform.right, camRotSpeed);
            offset = transform.position - target.position;
            offset.Normalize();
        }
        if (Input.GetKey("z") && (transform.rotation.eulerAngles.x > 1)) //down
        {
           

            transform.RotateAround(target.position, this.transform.right, -camRotSpeed);
            offset = transform.position - target.position;
            offset.Normalize();
        }
        // 줌 최소 및 최대 설정 
        //currentZoom -= Input.GetAxis("Mouse ScrollWheel");
        float scrollspeed = Input.GetAxis("Mouse ScrollWheel") * wheelSpeed;
        currentZoom -= scrollspeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        
        /*
                float wheelInput = Input.GetAxis("Mouse ScrollWheel") * wheelSpeed;
                //cam.fieldOfView <= 20 &&
                if (wheelInput > 0)
                {
                   cam.fieldOfView = 50.0f;
                }

                else if (wheelInput < 0)
                {
                    cam.fieldOfView = 90.0f;
                }
        */
    }

    void LateUpdate()
    {
        // 변경된 카메라 위치 적용
        transform.position = target.position + offset * currentZoom;
        transform.LookAt(target);
    }
}
