using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    int maxHP = 3;
    int hp;


    public int HP
    {
        get
        { return hp; }
        set
        { hp = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        HP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

}
