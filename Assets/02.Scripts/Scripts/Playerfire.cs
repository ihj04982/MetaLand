using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����ڰ� ���콺 ���� ��ư�� ������ �Ѿ˰��忡�� �Ѿ��� ���� �ѱ���ġ�� ��ġ�ϰ� �ʹ�.
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
    // *�ʿ� �Ӽ�
    public GameObject ArrowFactory;
    public PlayerAnimationEvent playeranimationevent;
    public STATE state;


    float ry;
    float rx;
    float mx;
    float my;
    float rotSpeed = 1000;

    public float xmove = 0;  // X�� ���� �̵���
    public float ymove = 0;  // Y�� ���� �̵���
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
            //3�� attack
        }
    }
    public bool check_bow_idle;
    float curtime;
    float shot_limit_time = 0.5f;
    public void BowArrow()
    {
        //���콺 ������ ��ư�� ������ ���� ��
        if (Input.GetMouseButton(1))
        {
            check_bow_idle = true;
            //�ִϸ��̼� ����
            anim.SetBool("Right_MouseButtonOn", true);
            anim.SetBool("armed_bow", true);
           // zipcanvas.SetActive(false);

            if (transform.rotation.eulerAngles.x > -20f)
            {
                //���콺 x�� �̵� ���� �޾Ƽ�
                mx = Input.GetAxis("Mouse X");
                my = Input.GetAxis("Mouse Y");
                // �������� �ʰ� �� ����� ���콺 ���� �޾ƿͼ�
                ry = mx * Time.deltaTime * rotSpeed;
                rx = my * Time.deltaTime * rotSpeed;
                // rx = Mathf.Clamp(rx, 1, 90);


                //�ڡڡڡ� �׶� ���� eulerAngle�� �����ش� �̷��� �ϸ� �ٶ󺸴� ������ �������� ȸ���ϰ� ��
                //transform.eulerAngles += new Vector3(-rx, ry, 0);
                transform.eulerAngles += new Vector3(0, ry, 0);
            }
            ZipShootRay();
        }

        //���콺 ������ ��ư ����
        if (Input.GetMouseButtonUp(1))
        {
            check_bow_idle = false;
            anim.SetBool("Right_MouseButtonOn", false);
            anim.SetBool("armed_bow", false);
            playeranimationevent.arrowobj.SetActive(false);
           // zipcanvas.SetActive(false);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
        }



        // �� �� 1�� ������
        if (curtime > shot_limit_time)
        {
            // ���콺 ���� ��ư�� ������
            if (Input.GetMouseButtonDown(0))
            {
                check_bow_idle = true;
                anim.SetBool("armed_bow", true);
                playeranimationevent.arrowobj.SetActive(true);
            }

            //���콺 ���� ��ư�� ����
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
                   
                    // ��Ŭ�� ��
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
                curtime = 0; //��� ���� 0.5�� ������
            }
        }
    }

    public void Zip()
    {
        //1. ���콺 ������ ��ư �� ������ ���� ��
        if (Input.GetMouseButton(0) && Ziplinemanager.instance.zipshoot == false)
        {
            Ziplinemanager.instance.zipshootidle = true;
            anim.SetBool("zipidle",true);
            
            //���콺 x�� �̵� ���� �޾Ƽ�
            mx = Input.GetAxis("Mouse X");
            // �������� �ʰ� �� ����� ���콺 ���� �޾ƿͼ�
            ry = mx * Time.deltaTime * rotSpeed;
            //�ڡڡڡ� �׶� ���� eulerAngle�� �����ش� �̷��� �ϸ� �ٶ󺸴� ������ �������� ȸ���ϰ� ��
            transform.eulerAngles += new Vector3(0, ry, 0);

            ZipShootRay();
        }

        //���콺 ������ ��ư �� ��
        if ((Input.GetMouseButtonUp(0) && Ziplinemanager.instance.zipshoot == false))
        {
            
            Ziplinemanager.instance.zipshootidle = false;
            Ziplinemanager.instance.zipshoot = true;
            anim.SetBool("zipidle", false);
            anim.SetTrigger("zipshoot");
            
            // ObjectPool ���� Zipline Object ���� �� �߻�
            GameObject zipline = ObjectPool.instance.GetDeactiveObject("Zipline");

            if (zipline) // = if(arrow != null)
            {
                zipline.SetActive(true);

                //zip shooting Ray Canvas ����
               // zipcanvas.SetActive(false);
                //transform.GetChild(5).gameObject.SetActive(false);
                // 3. �� �Ѿ��� �ѱ���ġ�� ��ġ�ϰ�ʹ�.
                // �Ѿ��� ��ġ = �ѱ��� ��ġ
                zipline.transform.position = firePosition.transform.position;
                zipline.transform.rotation = firePosition.transform.rotation;
            }
            ZipLayOff();

        }
    }

    public void Sield()
    {

    }
    // - �ǹ����� Projectile ��� ����ϸ� �������ϰ� �Ͼ
    // - ObjectPool,MemoryPool -> Garbege Collector�� �ֱ������� ����
    //prfep �� file -> ���� �����
    // �ѱ���ġ 
    public GameObject firePosition; // �Ӽ� �������� �ҹ��ڷ� ����
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
            // ry += mx �� �޾� �� ��� ��������� ry���� ����Ǽ� �߰��� Player�� ������ �ٲ� �ٽ� rŰ�� ������ ������ ȸ���ߴ� �������� ���� ����
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
            xmove += Input.GetAxis("Mouse X"); // ���콺�� �¿� �̵����� xmove �� �����մϴ�.
            ymove -= Input.GetAxis("Mouse Y"); // ���콺�� ���� �̵����� ymove �� �����մϴ�.
            transform.rotation = Quaternion.Euler(ymove, xmove, 0); // �̵����� ���� ī�޶��� �ٶ󺸴� ������ �����մϴ�.
    */
/*
            xmove += Input.GetAxis("Mouse X"); // ���콺�� �¿� �̵����� xmove �� �����մϴ�.
            ymove -= Input.GetAxis("Mouse Y"); // ���콺�� ���� �̵����� ymove �� �����մϴ�.
            Vector3 dir = new Vector3(xmove, 0, ymove);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotSpeed);
*/
//  Vector3 reverseDistance = new Vector3(0.0f, 0.0f, distance); // ī�޶� �ٶ󺸴� �չ����� Z ���Դϴ�. �̵���