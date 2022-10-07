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
       
        playeranimationevent = transform.Find("Animation_Boy").gameObject.GetComponent<PlayerAnimationEvent>();                                    // �θ� �پ��ִ� Component ã�ƿ��� �Լ�
                                                          // enemy = gameObject.GetComponentInChildren<Enemy>(); // �ڽĿ� �پ��ִ� Component ã�ƿ��� �Լ�
    }

    // Player fsm���� noweapon�� �Ǹ� -> Playerfire ��� ���� �ǰ�, �ִϸ��̼� �ش� ���� ����
    public void Noweapon() //Enemy Animation Event���� ���� ȣ���ϴ� �Լ�
    {                         //���� ��ü�� �پ��ִ� scripts�� ȣ�� �� �� ����
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
    public void Sword() //Enemy Animation Event���� ȣ���ϴ� �Լ�
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
