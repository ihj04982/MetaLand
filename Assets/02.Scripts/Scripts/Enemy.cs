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

    EnemyHP enemyHP; // has a ����

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
            //���
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
        // ���� ���� �Ÿ� �̳����
        if (dist <= chaseDistance)
        {
            //�̵� ���·� �����ϰ� �ʹ�
            state = State.MOVE;
            //�ִϸ��̼��� ���¸� Move�� �����ϰ� �ʹ�
            anim.SetTrigger("MOVE");
        }
    }

    private void UpdateMove()
    {
        // Ÿ���� ���� �̵��Ѵ�
        agent.destination = target.transform.position;
        float dist = Vector3.Distance(transform.position, target.transform.position);
        audioSource.clip = hitSound;
        audioSource.Play();
        // 1. Player���� �Ÿ��� ���ϰ�
        // 2. �� �Ÿ��� ���� ���� �Ÿ� ���϶��
        if (dist <= agent.stoppingDistance)
        {
            // 3. ���� ���·� �����ϰ� �ʹ�
            state = State.ATTACK;
            anim.SetTrigger("ATTACK");
        }
    }

    private void UpdateAttack()
    {
        
        
        float dist = Vector3.Distance(transform.position, target.transform.position);
        
        // ���� ���� ����� ���� �Ÿ��� �����
        if (dist > agent.stoppingDistance)
        {
            // ���� ���·� ��ȯ�Ѵ�
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
        // damage��ŭ ü���� ���ҽ�Ű��ʹ�
        enemyHP.HP -= damage;
        // NavMeshAgent�� ���߰� �ʹ�
        // agent.isStopped = true;
        // ���� ü���� 0 ���϶��
        if (enemyHP.HP <= 0)
        {
            enemyCollider.enabled = false;
            audioSource.clip = deathSound;
            audioSource.Play();

            // ���� ����
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
            //�ѹ�
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
