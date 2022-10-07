using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : MonoBehaviour
{
    Playerfire playerfire;
    PlayerMove playermove;
    PlayerAnimationEvent playeranimationevent;
    void Start()
    {
        playerfire = gameObject.GetComponent<Playerfire>();
        playermove = gameObject.GetComponent<PlayerMove>();
       
        playeranimationevent = transform.Find("Animation_Boy").gameObject.GetComponent<PlayerAnimationEvent>();                                    // 부모에 붙어있는 Component 찾아오는 함수
                                                          // enemy = gameObject.GetComponentInChildren<Enemy>(); // 자식에 붙어있는 Component 찾아오는 함수
    }

    // Player fsm에서 noweapon이 되면 -> Playerfire 모드 변경 되고, 애니매이션 해당 모드로 변경
    public void Noweapon() //Enemy Animation Event에서 직접 호출하는 함수
    {                         //같은 객체에 붙어있는 scripts만 호출 할 수 있음
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerfire.state = Playerfire.STATE.NOWEAPON;
            playeranimationevent.NoweaponAnimation();
        }
    }
    void Zip()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            playerfire.state = Playerfire.STATE.ZIP;
            playeranimationevent.zipAnimation();
        }
    }

    public bool getsword =false;
    // Update is called once per frame
    public void Sword() //Enemy Animation Event에서 호출하는 함수
    {
        if (getsword == true && Input.GetKeyDown(KeyCode.Alpha3))
        {
            playerfire.state = Playerfire.STATE.SWORD;
            playeranimationevent.swordAnimation();
        }
    }

    public bool getbow = false;
    void BowArrow()
    {
        if (getbow == true && Input.GetKeyDown(KeyCode.Alpha4))
        {
            playerfire.state = Playerfire.STATE.BOWARROW;
           // playeranimationevent.bowAnimation();
            playeranimationevent.arrowAnimation();
        }
    }
    void Shield()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            playerfire.state = Playerfire.STATE.SHIELD;
            playeranimationevent.shieldAnimation();
        }
    }
    // Update is called once per frame
    void Update()
    {
        Noweapon();
        Sword();
        BowArrow();
        Zip();
        Shield();

    }
}
