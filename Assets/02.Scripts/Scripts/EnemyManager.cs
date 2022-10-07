using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] enemys;
    public GameObject heart;
    public Transform targetPos;
    public int deathCount = 0;

    public static EnemyManager instance;
    private void Awake()
    {
        instance = this;
    }
    public int DEATHCOUNT
    {
        get
        { return deathCount; }
        set
        { deathCount = value;
            if (enemys.Length == deathCount)
            {
                GameObject ht = Instantiate(heart);
                ht.transform.position = targetPos.position;
            }
        }
    }


    void Start()
    {
       enemys =  GameObject.FindGameObjectsWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
    //    if(enemys.Length == deathCount)
    //    {
    //        GameObject ht = Instantiate(heart);
    //        ht.transform.position = targetPos.position;
    //    }
    }
}
