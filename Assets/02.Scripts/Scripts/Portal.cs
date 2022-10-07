using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 문을 통과하면 다른 문으로 이동하고 싶다
// 열쇠를 획득하고 Door2와 충돌할 경우 level2Pos로 이동하고싶다

public class Portal : MonoBehaviour
{
    public Transform startPos;
    public Transform level1Pos;
    public Transform level2Pos;

    Vector3 sPos;
    Vector3 pos1;
    Vector3 pos2;
    
    public GameObject key;
    public AudioSource BGM1;
    public AudioSource BGM2;

    private void Start()
    {
        sPos = startPos.position;
        pos1 = level1Pos.position;
        pos2 = level2Pos.position;
        // 플레이어 시작 초기 위치값
        this.gameObject.transform.position = new Vector3(sPos.x, sPos.y + 2, sPos.z);
        //this.gameObject.transform.position = new Vector3(startPos.position.x, 2, startPos.position.z);
    }

    private void OnCollisionEnter(Collision other)
    {


        if (other.gameObject.name.Contains("Door2"))
        {
            transform.position = new Vector3(pos2.x, pos2.y+2, pos2.z);
            BGM1.enabled = false;
            BGM2.enabled = true;

            key.SetActive(false);
        }
        if (other.gameObject.name.Contains("Door3"))
        {
            transform.position = new Vector3(pos1.x, pos1.y+2, pos1.z);
            BGM1.enabled = true;
            BGM2.enabled = false;
        }
        if (other.gameObject.name.Contains("Door4"))
        {
            SceneController.instance.OnEndingEnter();
        }
    }
}
