using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public GameObject itemFactory;
    // Start is called before the first frame update
    void Start()
    {
        GameObject target = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject item = Instantiate(itemFactory);
        Vector3 tf = transform.position;
        item.transform.position = new Vector3(tf.x, tf.y, tf.z + 1);
    }
}
