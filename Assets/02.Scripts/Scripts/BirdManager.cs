using UnityEngine;

public class BirdManager : MonoBehaviour
{
    LayerMask layer;
    public float detectedRadius;
    public GameObject[] birds;

    // Start is called before the first frame update
    void Start()
    {
        birds = new GameObject[3];
        for (int i = 0; i < birds.Length; i++)
        {
            birds[i] = transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        layer = 1 << LayerMask.NameToLayer("Player");
        Collider[] cols = Physics.OverlapSphere(transform.position, detectedRadius, layer);
        if (cols.Length > 0)
        {
          
            for (int i = 0; i < birds.Length; i++)
            {
                if (birds[i] != null)
                {
                    birds[i].GetComponent<Bird>().StartCoroutine("IEFly");
                }
            }
        }
    }
}
