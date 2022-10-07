using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사용자가 마우스 왼쪽 버튼을 누르면 총알공장에서 총알을 만들어서 총구위치에 배치하고 싶다.
public class Playerfire : MonoBehaviour
{
    public enum STATE // 
    {
        NOWEAPON,
        SWORD,
        BOWARROW,
        ZIP,
        SHIELD
    }
    // *필요 속성
    public GameObject ArrowFactory;
    public PlayerAnimationEvent playeranimationevent;
    public STATE state;


    float ry;
    float rx;
    float mx;
    float my;
    float rotSpeed = 1000;

    public float xmove = 0;  // X축 누적 이동량
    public float ymove = 0;  // Y축 누적 이동량
    Animator anim;
    void Start()
    {
        pm = GetComponent<PointManager>();
        lr = GetComponent<LineRenderer>();
        ObjectPool.instance.CreateInstance("Arrow");
        ObjectPool.instance.CreateInstance("Zipline");
        anim = transform.Find("Animation_Boy").gameObject.GetComponent<Animator>();
        playeranimationevent = transform.Find("Animation_Boy").gameObject.GetComponent<PlayerAnimationEvent>();
    }

    // Update is called once per frame
    void Update()
    {
        curtime += Time.deltaTime;
        switch (state)
        {
            case STATE.NOWEAPON:
                Noweapon();
                break;

            case STATE.SWORD:
                Sword();
                break;
            case STATE.BOWARROW:
                BowArrow();
                break;

            case STATE.ZIP:
                Zip();
                break;

            case STATE.SHIELD:
                Sield();
                break;
        }
    }
    public void Noweapon()
    {

    }
    public void Sword()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
                anim.SetTrigger("sword_attack");
            //3단 attack
        }
    }
    public bool check_bow_idle;
    float curtime;
    float shot_limit_time = 0.5f;
    public void BowArrow()
    {
        //마우스 오른쪽 버튼을 누르고 있을 때
        if (Input.GetMouseButton(1))
        {
            check_bow_idle = true;
            //애니메이션 제어
            anim.SetBool("Right_MouseButtonOn", true);
            anim.SetBool("armed_bow", true);
           // zipcanvas.SetActive(false);

            if (transform.rotation.eulerAngles.x > -20f)
            {
                //마우스 x축 이동 값을 받아서
                mx = Input.GetAxis("Mouse X");
                my = Input.GetAxis("Mouse Y");
                // 누적하지 않고 그 당시의 마우스 값을 받아와서
                ry = mx * Time.deltaTime * rotSpeed;
                rx = my * Time.deltaTime * rotSpeed;
                // rx = Mathf.Clamp(rx, 1, 90);


                //★★★★ 그때 부터 eulerAngle에 더해준다 이렇게 하면 바라보는 시점을 기준으로 회전하게 됌
                //transform.eulerAngles += new Vector3(-rx, ry, 0);
                transform.eulerAngles += new Vector3(0, ry, 0);
            }
            ZipShootRay();
        }

        //마우스 오른쪽 버튼 때면
        if (Input.GetMouseButtonUp(1))
        {
            check_bow_idle = false;
            anim.SetBool("Right_MouseButtonOn", false);
            anim.SetBool("armed_bow", false);
            playeranimationevent.arrowobj.SetActive(false);
           // zipcanvas.SetActive(false);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
        }



        // 쏠 때 1초 딜레이
        if (curtime > shot_limit_time)
        {
            // 마우스 왼쪽 버튼을 누르면
            if (Input.GetMouseButtonDown(0))
            {
                check_bow_idle = true;
                anim.SetBool("armed_bow", true);
                playeranimationevent.arrowobj.SetActive(true);
            }

            //마우스 왼쪽 버튼을 떼면
            if (Input.GetMouseButtonUp(0) && check_bow_idle)
            {
                check_bow_idle = false;
                anim.SetTrigger("fireBow");
                anim.SetBool("armed_bow", false);
                playeranimationevent.arrowobj.SetActive(false);

                GameObject arrow = ObjectPool.instance.GetDeactiveObject("Arrow");
                if (arrow) // = if(arrow != null)
                {

                    arrow.SetActive(true);
                    Rigidbody rb = arrow.GetComponent<Rigidbody>();
                   // rb.velocity = transform.forward * 20;
                    
                    arrow.transform.position = arrowFirePosition.transform.position;
                   
                    // 우클릭 시
                    if (Input.GetMouseButton(1))
                    {
                       arrow.transform.rotation = transform.rotation;
                        rb.useGravity = false;
                    }
                    else
                    {
                       arrow.transform.rotation = arrowFirePosition.transform.rotation;
                    }
                }
                curtime = 0; //쏘고 나서 0.5초 딜레이
            }
        }
    }

    public void Zip()
    {
        //1. 마우스 오른쪽 버튼 꾹 누르고 있을 때
        if (Input.GetMouseButton(0) && Ziplinemanager.instance.zipshoot == false)
        {
            Ziplinemanager.instance.zipshootidle = true;
            anim.SetBool("zipidle",true);
            
            //마우스 x축 이동 값을 받아서
            mx = Input.GetAxis("Mouse X");
            // 누적하지 않고 그 당시의 마우스 값을 받아와서
            ry = mx * Time.deltaTime * rotSpeed;
            //★★★★ 그때 부터 eulerAngle에 더해준다 이렇게 하면 바라보는 시점을 기준으로 회전하게 됌
            transform.eulerAngles += new Vector3(0, ry, 0);

            ZipShootRay();
        }

        //마우스 오른쪽 버튼 땔 때
        if ((Input.GetMouseButtonUp(0) && Ziplinemanager.instance.zipshoot == false))
        {
            
            Ziplinemanager.instance.zipshootidle = false;
            Ziplinemanager.instance.zipshoot = true;
            anim.SetBool("zipidle", false);
            anim.SetTrigger("zipshoot");
            
            // ObjectPool 에서 Zipline Object 생성 후 발사
            GameObject zipline = ObjectPool.instance.GetDeactiveObject("Zipline");

            if (zipline) // = if(arrow != null)
            {
                zipline.SetActive(true);

                //zip shooting Ray Canvas 끄기
               // zipcanvas.SetActive(false);
                //transform.GetChild(5).gameObject.SetActive(false);
                // 3. 그 총알을 총구위치에 배치하고싶다.
                // 총알의 위치 = 총구의 위치
                zipline.transform.position = firePosition.transform.position;
                zipline.transform.rotation = firePosition.transform.rotation;
            }
            ZipLayOff();

        }
    }

    public void Sield()
    {

    }
    // - 실무에서 Projectile 방식 사용하면 성능저하가 일어남
    // - ObjectPool,MemoryPool -> Garbege Collector가 주기적으로 관리
    //prfep 은 file -> 파일 입출력
    // 총구위치 
    public GameObject firePosition; // 속성 변수명은 소문자로 시작
    public GameObject arrowFirePosition;


    float zipMaxDistance;
    // Start is called before the first frame update

    PointManager pm;
    LineRenderer lr;

    void ZipLayOff()
    {
        for (int i = 0; i < pm.points.Count; i++)
        {
            pm.points[i].gameObject.SetActive(!(i < lr.positionCount));
        }
    }

    void ZipShootRay()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;
        lr.positionCount = 1;
        lr.SetPosition(0, ray.origin);
        int layerMask = 1 << LayerMask.NameToLayer("Default");
        if (Physics.Raycast(ray, out hitInfo, 100, layerMask))
        {
            print($"layermask : {layerMask}, {hitInfo.collider.gameObject.name}");
            float pointGap = 1;
            float t;
            int i;
            for (i = 1, t = 0; ; i++)
            {
                Vector3 pos = ray.origin + ray.direction * i * pointGap;
                lr.positionCount++;
                lr.SetPosition(i, pos);
                if (t + pointGap > hitInfo.distance)
                {
                    i++;
                    pos = hitInfo.point;
                    lr.positionCount++;
                    lr.SetPosition(i, pos);
                    break;
                }
                else
                {
                    t += pointGap;
                }
            }
        }
        
        for (int i = 0; i < pm.points.Count; i++)
        {
            pm.points[i].gameObject.SetActive(i < lr.positionCount-5);
            if (i < lr.positionCount-5)
            {
                pm.points[i].SetUp(lr.GetPosition(i+1), lr.GetPosition(i +2));
            }
        }

    }

}




/*
            mx = Input.GetAxis("Mouse X");
            // ry += mx 로 받아 올 경우 현재까지의 ry값이 저장되서 중간에 Player의 방향이 바뀌어도 다시 r키를 누르면 기존에 회전했던 방향으로 고개를 돌림
            ry += mx * Time.deltaTime * rotSpeed;
            transform.eulerAngles = new Vector3(0, ry, 0);
*/

/*
            pos = Input.mousePosition;
            posz = Vector3.Distance(transform.position, Camera.main.transform.position);
            pos.z = posz;
            Vector3 mousepos = new Vector3(pos.x, 0, pos.z);

            target = Camera.main.ScreenPointToRay(pos);
            target.y = 0;
            transform.forward += target;

*/
/*
            xmove += Input.GetAxis("Mouse X"); // 마우스의 좌우 이동량을 xmove 에 누적합니다.
            ymove -= Input.GetAxis("Mouse Y"); // 마우스의 상하 이동량을 ymove 에 누적합니다.
            transform.rotation = Quaternion.Euler(ymove, xmove, 0); // 이동량에 따라 카메라의 바라보는 방향을 조정합니다.
    */
/*
            xmove += Input.GetAxis("Mouse X"); // 마우스의 좌우 이동량을 xmove 에 누적합니다.
            ymove -= Input.GetAxis("Mouse Y"); // 마우스의 상하 이동량을 ymove 에 누적합니다.
            Vector3 dir = new Vector3(xmove, 0, ymove);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotSpeed);
*/
//  Vector3 reverseDistance = new Vector3(0.0f, 0.0f, distance); // 카메라가 바라보는 앞방향은 Z 축입니다. 이동량