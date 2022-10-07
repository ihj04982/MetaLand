using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public GameObject pointFactory;
    public int maxCount = 100;
    public List<Point> points;
    public float t;
    // Start is called before the first frame update
    void Start()
    {
        points = new List<Point>();
        for (int i = 0; i < maxCount; i++)
        {
            Point p = Instantiate(pointFactory,transform).GetComponent<Point>();
            p.SetPointManager(this);
            points.Add(p);
        }
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        if (t > 1)
            t = 0;
    }
}
