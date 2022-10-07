using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    PlayerMove playermove;
    Playerfire playerfire;
    
    public enum STATE // 
    {
        NOWEAPON,
        SWORD,
        BOWARROW,
        ZIP,
        SHIELD
    }

    public STATE state;
    public GameObject Swordobj;
    public GameObject Bowobj;
    public GameObject Zipobj;
    public GameObject arrowobj;
    public GameObject Shieldobj;
    public GameObject Quiver;
    public GameObject SwordCol;
    Animator anim;
    //GameObject[] WeaponSwap = new GameObject[5];
    // Start is called before the first frame update
    void Start()
    {
        
        anim = GetComponent<Animator>();
        playermove = gameObject.GetComponentInParent<PlayerMove>();
        playerfire = gameObject.GetComponentInParent<Playerfire>();
        
        Swordobj.SetActive(false);
        Bowobj.SetActive(false);
        arrowobj.SetActive(false);
        Zipobj.SetActive(false);
        Shieldobj.SetActive(false);
        Quiver.SetActive(false);
        
    }
    
    public void NoweaponAnimation()
    {
    Swordobj.SetActive(false);
    Bowobj.SetActive(false);
    arrowobj.SetActive(false);
    Zipobj.SetActive(false);
    Shieldobj.SetActive(false);
    Quiver.SetActive(false);
    anim.SetTrigger("idle_noweapon");
    }
    public void swordAnimation()
    {
        Swordobj.SetActive(true);
        Bowobj.SetActive(false);
        arrowobj.SetActive(false);
        Zipobj.SetActive(false);
        Shieldobj.SetActive(false);
        Quiver.SetActive(false);
        anim.SetTrigger("idle_sword");

    }
    public void bowAnimation()
    {
        Swordobj.SetActive(false);
        Bowobj.SetActive(true);
        arrowobj.SetActive(false);
        Zipobj.SetActive(false);
        Shieldobj.SetActive(false);
        Quiver.SetActive(true);
        anim.SetTrigger("idle_bow");

    }
    public void arrowAnimation()
    {
        Swordobj.SetActive(false);
        Bowobj.SetActive(true);
        arrowobj.SetActive(false);
        Zipobj.SetActive(false);
        Shieldobj.SetActive(false);
        Quiver.SetActive(true);
        anim.SetTrigger("idle_bow");

    }
    public void zipAnimation()
    {
        Swordobj.SetActive(false);
        Bowobj.SetActive(false);
        arrowobj.SetActive(false);
        Zipobj.SetActive(true);
        Shieldobj.SetActive(false);
        Quiver.SetActive(false);
        anim.SetTrigger("idle_zip");

    }
    public void shieldAnimation()
    {
        Swordobj.SetActive(false);
        Bowobj.SetActive(false);
        arrowobj.SetActive(false);
        Zipobj.SetActive(false);
        Shieldobj.SetActive(true);
        Quiver.SetActive(false);
       // anim.SetTrigger("idle_noweapon");

    }


    // Update is called once per frame
    void Update()
    {
        OnMoveBool();
        Death();
        Jump();
    }

    private void OnMoveBool()
    {
       if(playermove.isMoving == true)
       {
            anim.SetBool("isMoving", true);
       }
       else
       {
            anim.SetBool("isMoving", false);
       }
    }
    private void Death()
    {
        if(!playermove.death && PlayerHP.instance.HP < 0)
        {
            anim.SetTrigger("Death");
            playermove.death = true;
        }
    }
    private void Jump()
    {
        if (playermove.isJumping == true)
        {
            anim.SetBool("isJump", true);
        }
        else
        {
            anim.SetBool("isJump", false);
        }
    }
    
    public void ArrowSetActivefalse()
    {
        arrowobj.SetActive(false);
    }
    public void ArrowSetActive()
    {
        arrowobj.SetActive(true);
    }

    public void AttackColSetActive()
    {
        SwordCol.SetActive(true);
    }
    public void AttackColSetActivefalse()
    {
        SwordCol.SetActive(false);
    }
}
