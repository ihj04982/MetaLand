using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;
using System;
// ������� �Է¿����� �����¿�� �̵��ϰ�ʹ�.

public class PlayerMove : MonoBehaviour
{
        Playerfire playerfire;
        //�������� üũ�� ���̾� ����
        public LayerMask groundLayerMask = -1;

        //"���� ���� �Ÿ�"
        public float forwardCheckDistance = 1.0f;

        //"���� ���� �Ÿ�"
        public float groundCheckDistance = 1.0f;

        //"���� �ν� ��� �Ÿ�"
        public float groundCheckThreshold = 0.01f;

        //"�̵��ӵ�"
        public float speed = 5f;

        [Range(1f, 75f), Tooltip("��� ������ ��簢")]
        public float maxSlopeAngle =45f;

        //[Range(0f, 4f), Tooltip("���� �̵��ӵ� ��ȭ��(����/����)")]
        public float slopeAccel = 1f;
        
       // [Range(-9.81f, 0f), Tooltip("�߷�")]
        public float gravity = -9.81f;

        public bool isMoving;
        public bool isRunning;
        public bool isGrounded = true;
        public bool isOnSteepSlope;   // ��� �Ұ����� ���ο� �ö�� ����
        public bool isJumpTriggered;
        public bool isJumping;
        public bool isForwardBlocked =false; // ���濡 ��ֹ� ����
        public bool isOutOfControl;   // ���� �Ұ� ����
                                     // �߰�
        public bool isCanWalk;
        public float zipFinalSpeed;
        
        public Vector3 groundNormal;
        public Vector3 groundCross;
        public Vector3 horizontalVelocity;
        public float outOfControllDuration;

      
        public float groundDistance;
        public float groundSlopeAngle;         // ���� �ٴ��� ��簢
        public float groundVerticalSlopeAngle; // �������� �������� ��簢
        public float forwardSlopeAngle; // ĳ���Ͱ� �ٶ󺸴� ������ ��簢
                                        //   public float slopeAccel;        // ���� ���� ����/���� ����
    //private float _capsuleRadiusDiff= 0.99f;
    private float _capsuleRadiusDiff= 0.48f;

    private float _castRadius =1.0f; // Sphere, Capsule ����ĳ��Ʈ ������
    private Vector3 CapsuleTopCenterPoint
        => new Vector3(transform.position.x, transform.position.y + capCol.height - capCol.radius, transform.position.z);
    private Vector3 CapsuleBottomCenterPoint
        => new Vector3(transform.position.x, transform.position.y + capCol.radius, transform.position.z);
    Vector3 mainCameraDir;
    private Rigidbody rb;
    private CapsuleCollider capCol;
    Animator anim;
    public float jumpPower = 10;
    //public float speed = 5;
    Vector3 dir;
    public int rotSpeed = 10;
    float h;
    float v;

    
    int jumpCount;
    //public float movesensitivity = 0.05f;
    Quaternion lookRotation;
    public bool death = false;

    //onCollisionEnter�� �ٲٱ� -> �ٴںκ� collision �Ͼ� �� �÷�

    private void CheckForward()

    {
        bool cast =
            Physics.CapsuleCast(CapsuleBottomCenterPoint, CapsuleTopCenterPoint, _castRadius, dir + Vector3.down * 0.1f ,
                out var hit, forwardCheckDistance, -1, QueryTriggerInteraction.Ignore);

        isForwardBlocked = false;
        if (cast)
        {
            print(forwardCheckDistance + "forward");
            float forwardObstacleAngle = Vector3.Angle(hit.normal, Vector3.up);
            isForwardBlocked = forwardObstacleAngle >= maxSlopeAngle;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = new Vector3(0, -15f, 0)*3;
        jumpCount = 1;
        rb = GetComponent<Rigidbody>();
        capCol = GetComponent<CapsuleCollider>();
        anim = transform.Find("Animation_Boy").gameObject.GetComponent<Animator>();
        isGrounded = true;
        playerfire = GetComponent<Playerfire>();
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        CheckForward();
        dir = new Vector3(h, 0, v);

        //Camera �������� �̵� �� �ʱ�ȭ
        mainCameraDir = Camera.main.transform.TransformDirection(dir);
        //Camera �������� ��ȯ�� Y�� ����
        dir = new Vector3(mainCameraDir.x, 0, mainCameraDir.z);


        // ���� �� ���� �� ��� ���� �� ��� �ؼ� �� ȸ�� ��Ŵ)
        if (isCanWalk)
        {
            // dir.Quaternion.AngleAxis( ȸ���� , ȸ����)
            dir = (Quaternion.AngleAxis(-groundSlopeAngle, groundCross) * dir);

        }
        // Movee22
        
        CharMove(dir);
        isMoving = dir.sqrMagnitude > 0.01f;
        //isStop
        // Jump �� JumpȽ�� ����
        Jump(jumpCount);
    }

    void Jump(int jumpCount)
    {
        if (!Ziplinemanager.instance.hit && !Ziplinemanager.instance.zipshoot && !playerfire.check_bow_idle && !Ziplinemanager.instance.zipshootidle && !death) //zipline ���¿��� ���� �Ұ�
        { 
            if (isGrounded == true) //���� �پ����� ��
            {
                isJumping = false;

                if (Input.GetButtonDown("Jump") && jumpCount > 0) // ������ư ������ ���� count�� �ִٸ�
                {
                    rb.AddForce(0, jumpPower, 0, ForceMode.Impulse); // ����
                    isGrounded = false;
                    isJumping = true;
                    jumpCount--;
                }
            }
        }

        //������ ����
        if (isGrounded == false)
        {
            isJumping = true;
            if (Input.GetButtonUp("Jump"))
            {
                rb.AddForce(0, -jumpPower / 4, 0, ForceMode.Impulse);
                isJumping = false;
            }
        }
       // transform.position += dir * speed * Time.deltaTime;
    }



    void OnCollisionEnter(Collision collision)
    {
      // hit ���� �� ����
      // player HP���� �Ǳ��� �� 
    }

    void CharMove(Vector3 chardir)
    {
        //Zipline �¾��� �� ������ ����
        if (!Ziplinemanager.instance.hit && !Ziplinemanager.instance.zipshoot && !playerfire.check_bow_idle&& !Ziplinemanager.instance.zipshootidle && !death)
        {
            anim.SetBool("fly", false);
            float finalSpeed = 0;
            dir = chardir;
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            // Charater ���� ��ȯ 
            if (Input.anyKey && (v != 0 || h != 0))
            {
                lookRotation = Quaternion.LookRotation(dir);
                lookRotation.Normalize();
                finalSpeed = speed;
                Quaternion rot = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * rotSpeed);
                rot.Normalize();
                transform.rotation = rot;
                //rb.MoveRotation(rot);
            }
            dir.Normalize();
            //transform.position += dir * finalSpeed * Time.deltaTime;
            rb.MovePosition(transform.position + dir * finalSpeed * Time.deltaTime);
        }

        //zipline �����̸�
        else if (Ziplinemanager.instance.hit == true)
        {
            anim.SetBool("fly", true);
            rb.useGravity = false;
            //transform.position = Vector3.MoveTowards(transform.position, Ziplinemanager.instance.zipcolpos, Ziplinemanager.instance.flyspeed);
            Vector3 dir = Ziplinemanager.instance.zipcolpos - transform.position;
            zipFinalSpeed = Ziplinemanager.instance.zipflyspeed;
            float coldist = Vector3.Distance(transform.position, Ziplinemanager.instance.zipcolpos);
            if (coldist > 1f)
            {
                //transform.position += dir * zipFinalSpeed * Time.deltaTime;
                transform.position += dir * zipFinalSpeed * Time.deltaTime;

            }
            else
            {
                rb.useGravity = true;
                Ziplinemanager.instance.hit = false;
                zipFinalSpeed = 0;
            
            }
        }
       
    }

    private void CheckGround()
    {
        groundDistance = float.MaxValue;
        groundNormal = Vector3.up;
        groundSlopeAngle = 0f;
        forwardSlopeAngle = 0f;

        bool cast =
            Physics.SphereCast(CapsuleBottomCenterPoint, _castRadius, Vector3.down, out var hit, groundCheckDistance, groundLayerMask, QueryTriggerInteraction.Ignore);

        isGrounded = false;

        // �ٴ��� ���� �ƴٸ�
        if (cast)
        {
            // ������ ��������(��ֺ���)�� groundNormal ����
            groundNormal = hit.normal;

            // ���� ��ġ�� ������ ��簢 ���ϱ�(ĳ���� �̵����� ���)
            groundSlopeAngle = Vector3.Angle(groundNormal, Vector3.up); // ��簢 ���ϱ�

            //isOnSteepSlope �� True �̸� ���� �� �ִ� ��翡 �ö� �� ����            
            isOnSteepSlope = (groundSlopeAngle <= maxSlopeAngle) && (groundSlopeAngle >0);

            //isCanWalk == true �̸� ���� + ���� / ������ isOnsteepSlope
            isCanWalk = (groundSlopeAngle <= maxSlopeAngle);

            // �� ������ ����� ����
            forwardSlopeAngle = Vector3.Angle(groundNormal, dir) - 90f;
            
            



            // ��簢 ���߰��� (���� ����ĳ��Ʈ) : �����ϰų� ���� �κ� üũ
            //if (State.isOnSteepSlope)
            //{
            //    Vector3 ro = hit.point + Vector3.up * 0.1f;
            //    Vector3 rd = Vector3.down;
            //    bool rayD = 
            //        Physics.SphereCast(ro, 0.09f, rd, out var hitRayD, 0.2f, COption.groundLayerMask, QueryTriggerInteraction.Ignore);

            //    groundVerticalSlopeAngle = rayD ? Vector3.Angle(hitRayD.normal, Vector3.up) : groundSlopeAngle;

            //    State.isOnSteepSlope = groundVerticalSlopeAngle >= MOption.maxSlopeAngle;
            //}
            groundDistance = Mathf.Max(hit.distance - _capsuleRadiusDiff - groundCheckThreshold, 0f);
            isGrounded = (groundDistance <= 0.1f); 
/*
            if(groundDistance >= 0.1f)
            {
                isJumping = true;
            }
            if (isGrounded)
            {
                isJumping = false;
                jumpCount = 1;
            }
*/
        }
        // groundNormal == Player�� ��� �ִ� ���� ���� , Vector3.up == y��
        // Cross == ������ �ϰ� �Ǹ� �� ���Ͱ� �̷�� ����� ���� ���͸� ���� �� ����
        // �� ���� �������� ȸ���ϰ� �Ǹ� �ٶ� ���� ���⿡ ���� ���� ȸ����
        // ���� �� ���Ͱ� �̷�� ���� ���Ͱ� ȸ�� ���� ��
        // groundCross �� ȸ���ุ ���� ��.
        groundCross = Vector3.Cross(groundNormal, Vector3.up);
        
    }




}
