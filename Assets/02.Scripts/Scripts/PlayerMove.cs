using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;
using System;
// 사용자의 입력에따라서 상하좌우로 이동하고싶다.

public class PlayerMove : MonoBehaviour
{
        Playerfire playerfire;
        //지면으로 체크할 레이어 설정
        public LayerMask groundLayerMask = -1;

        //"전방 감지 거리"
        public float forwardCheckDistance = 1.0f;

        //"지면 감지 거리"
        public float groundCheckDistance = 1.0f;

        //"지면 인식 허용 거리"
        public float groundCheckThreshold = 0.01f;

        //"이동속도"
        public float speed = 5f;

        [Range(1f, 75f), Tooltip("등반 가능한 경사각")]
        public float maxSlopeAngle =45f;

        //[Range(0f, 4f), Tooltip("경사로 이동속도 변화율(가속/감속)")]
        public float slopeAccel = 1f;
        
       // [Range(-9.81f, 0f), Tooltip("중력")]
        public float gravity = -9.81f;

        public bool isMoving;
        public bool isRunning;
        public bool isGrounded = true;
        public bool isOnSteepSlope;   // 등반 불가능한 경사로에 올라와 있음
        public bool isJumpTriggered;
        public bool isJumping;
        public bool isForwardBlocked =false; // 전방에 장애물 존재
        public bool isOutOfControl;   // 제어 불가 상태
                                     // 추가
        public bool isCanWalk;
        public float zipFinalSpeed;
        
        public Vector3 groundNormal;
        public Vector3 groundCross;
        public Vector3 horizontalVelocity;
        public float outOfControllDuration;

      
        public float groundDistance;
        public float groundSlopeAngle;         // 현재 바닥의 경사각
        public float groundVerticalSlopeAngle; // 수직으로 재측정한 경사각
        public float forwardSlopeAngle; // 캐릭터가 바라보는 방향의 경사각
                                        //   public float slopeAccel;        // 경사로 인한 가속/감속 비율
    //private float _capsuleRadiusDiff= 0.99f;
    private float _capsuleRadiusDiff= 0.48f;

    private float _castRadius =1.0f; // Sphere, Capsule 레이캐스트 반지름
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

    //onCollisionEnter는 바꾸기 -> 바닥부분 collision 일어 날 시로

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

        //Camera 방향으로 이동 값 초기화
        mainCameraDir = Camera.main.transform.TransformDirection(dir);
        //Camera 방향으로 전환시 Y축 고정
        dir = new Vector3(mainCameraDir.x, 0, mainCameraDir.z);


        // 걸을 수 있을 때 경사 백터 값 계산 해서 축 회전 시킴)
        if (isCanWalk)
        {
            // dir.Quaternion.AngleAxis( 회전각 , 회전축)
            dir = (Quaternion.AngleAxis(-groundSlopeAngle, groundCross) * dir);

        }
        // Movee22
        
        CharMove(dir);
        isMoving = dir.sqrMagnitude > 0.01f;
        //isStop
        // Jump 와 Jump횟수 제한
        Jump(jumpCount);
    }

    void Jump(int jumpCount)
    {
        if (!Ziplinemanager.instance.hit && !Ziplinemanager.instance.zipshoot && !playerfire.check_bow_idle && !Ziplinemanager.instance.zipshootidle && !death) //zipline 상태에서 점프 불가
        { 
            if (isGrounded == true) //땅에 붙어있을 때
            {
                isJumping = false;

                if (Input.GetButtonDown("Jump") && jumpCount > 0) // 점프버튼 누르고 점프 count가 있다면
                {
                    rb.AddForce(0, jumpPower, 0, ForceMode.Impulse); // 점프
                    isGrounded = false;
                    isJumping = true;
                    jumpCount--;
                }
            }
        }

        //반점프 구현
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
      // hit 됐을 때 깜빡
      // player HP에서 피깎일 때 
    }

    void CharMove(Vector3 chardir)
    {
        //Zipline 맞았을 때 움직임 제한
        if (!Ziplinemanager.instance.hit && !Ziplinemanager.instance.zipshoot && !playerfire.check_bow_idle&& !Ziplinemanager.instance.zipshootidle && !death)
        {
            anim.SetBool("fly", false);
            float finalSpeed = 0;
            dir = chardir;
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            // Charater 시점 전환 
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

        //zipline 상태이면
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

        // 바닥이 검출 됐다면
        if (cast)
        {
            // 지면의 법선백터(노멀벡터)로 groundNormal 설정
            groundNormal = hit.normal;

            // 현재 위치한 지면의 경사각 구하기(캐릭터 이동방향 고려)
            groundSlopeAngle = Vector3.Angle(groundNormal, Vector3.up); // 경사각 구하기

            //isOnSteepSlope 가 True 이면 걸을 수 있는 경사에 올라 와 있음            
            isOnSteepSlope = (groundSlopeAngle <= maxSlopeAngle) && (groundSlopeAngle >0);

            //isCanWalk == true 이면 평지 + 경사면 / 기존의 isOnsteepSlope
            isCanWalk = (groundSlopeAngle <= maxSlopeAngle);

            // 앞 방향의 경사의 각도
            forwardSlopeAngle = Vector3.Angle(groundNormal, dir) - 90f;
            
            



            // 경사각 이중검증 (수직 레이캐스트) : 뾰족하거나 각진 부분 체크
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
        // groundNormal == Player가 밟고 있는 땅의 각도 , Vector3.up == y축
        // Cross == 외적을 하게 되면 두 벡터가 이루는 평면의 법선 벡터를 구할 수 있음
        // 그 축을 기준으로 회전하게 되면 바라 보는 방향에 대해 축이 회전함
        // 따라서 두 벡터가 이루는 법선 벡터가 회전 축이 됌
        // groundCross 는 회전축만 구한 것.
        groundCross = Vector3.Cross(groundNormal, Vector3.up);
        
    }




}
