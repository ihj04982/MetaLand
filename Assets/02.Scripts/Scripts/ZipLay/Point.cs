using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    Camera cam;
    public Vector3 start;
    public Vector3 end;
    PointManager pm;

    public void SetPointManager(PointManager pm)
    {
        this.pm = pm;
    }

    public void SetUp(Vector3 s, Vector3 e)
    {
        start = s;
        end = e;
    }
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
            transform.forward = cam.transform.forward;
            transform.position = Vector3.Lerp(start, end, pm.t);

    }
}
