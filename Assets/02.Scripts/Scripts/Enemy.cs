using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    GameObject target = null;
    NavMeshAgent agent = null;

    public Animator anim;
    public State state;

    public float chaseDistance;

    EnemyHP enemyHP; // has a 관계

    SkinnedMeshRenderer[] smr;
    Material[] backup;
    public Material matGray;
    CapsuleCollider enemyCollider;

    AudioSource audioSource;
    public AudioClip hitSound;
    public AudioClip attackedSound;
    public AudioClip deathSound;


    public enum State
    {
        IDLE,
        MOVE,
        ATTACK,
        REACT,
        DIE,
    }



    void Start()
    {
        state = State.IDLE;
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("Player");
        enemyHP = GetComponent<EnemyHP>();

        enemyCollider = GetComponent<CapsuleCollider>();
        smr = GetComponentsInChildren<SkinnedMeshRenderer>();
        backup = new Material[smr.Length];
        for (int i = 0; i < smr.Length; i++)
        {
            //백업
            backup[i] = smr[i].material;
        }

        audioSource = GetComponent<AudioSource>();
     }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.IDLE:
                UpdateIdle();
                break;
            case State.ATTACK:
                UpdateAttack();
                break;
            case State.MOVE:
                UpdateMove();
                break;
            case State.REACT:
                UpdateReact();
                break;
            case State.DIE:
                
                UpdateDie();
                break;
        }
    }

    private void UpdateIdle()
    {
        float dist = Vector3.Distance(transform.position, target.transform.position);
        // 만약 추적 거리 이내라면
        if (dist <= chaseDistance)
        {
            //이동 상태로 전이하고 싶다
            state = State.MOVE;
            //애니메이션의 상태를 Move로 전이하고 싶다
            anim.SetTrigger("MOVE");
        }
    }

    private void UpdateMove()
    {
        // 타겟을 향해 이동한다
        agent.destination = target.transform.position;
        float dist = Vector3.Distance(transform.position, target.transform.position);
        audioSource.clip = hitSound;
        audioSource.Play();
        // 1. Player와의 거리를 구하고
        // 2. 그 거리가 공격 가능 거리 이하라면
        if (dist <= agent.stoppingDistance)
        {
            // 3. 공격 상태로 전이하고 싶다
            state = State.ATTACK;
            anim.SetTrigger("ATTACK");
        }
    }

    private void UpdateAttack()
    {
        
        
        float dist = Vector3.Distance(transform.position, target.transform.position);
        
        // 만약 추적 대상이 공격 거리를 벗어나면
        if (dist > agent.stoppingDistance)
        {
            // 추적 상태로 전환한다
            state = State.MOVE;
            anim.SetTrigger("MOVE");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
      
        if (other.gameObject.tag.Contains("Weapon"))
        {
   
            TakeDamage(1);
            agent.destination = target.transform.position;
        }
    }
    private void TakeDamage(int damage)
    {
        StopCoroutine("IEFlashBody");
        StartCoroutine("IEFlashBody");
        // damage만큼 체력을 감소시키고싶다
        enemyHP.HP -= damage;
        // NavMeshAgent를 멈추고 싶다
        // agent.isStopped = true;
        // 만약 체력이 0 이하라면
        if (enemyHP.HP <= 0)
        {
            enemyCollider.enabled = false;
            audioSource.clip = deathSound;
            audioSource.Play();

            // 죽음 상태
            state = State.DIE;
            anim.SetTrigger("DEATH");
            EnemyManager.instance.DEATHCOUNT++;
        }
        else
        {
            audioSource.clip = attackedSound;
            audioSource.Play();

            // state = State.REACT;
            anim.SetTrigger("REACT");
        }


    }

    IEnumerator IEFlashBody()
    {
       
        for (int i = 0; i < smr.Length; i++)
        {
            smr[i].material = matGray;
        }
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < smr.Length; i++)
        {
            //롤백
            smr[i].material = backup[i];
        }
    }
    private void UpdateReact()
    {
        audioSource.clip = attackedSound;
        audioSource.Play();
        //  anim.SetTrigger("REACT");
    }

    private void UpdateDie()
    {
        
        
        Destroy(gameObject,3.0f);

    }

    public void OnAttackHit()
    {
        PlayerHP.instance.HP--;

    }


   
}
