using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDamage : MonoBehaviour
{
    public Material matWhite;
    Material[] backup;
    SkinnedMeshRenderer[] smr;
    // Start is called before the first frame update
    void Start()
    {
        smr = GetComponentsInChildren<SkinnedMeshRenderer>();
        backup = new Material[smr.Length];
        for (int i = 0; i < smr.Length; i++)
        {
            //백업
            backup[i] = smr[i].material;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
      //  if (other.gameObject.tag.Contains("Enemy"))
      //  {
            TakeDamage();
       // }
    }
    IEnumerator IEFlashBody()
    {

        for (int i = 0; i < smr.Length; i++)
        {
            smr[i].material = matWhite;
        }
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < smr.Length; i++)
        {
            //롤백
            smr[i].material = backup[i];
        }
    }

    private void TakeDamage()
    {
        StopCoroutine("IEFlashBody");
        StartCoroutine("IEFlashBody");
    }
}
