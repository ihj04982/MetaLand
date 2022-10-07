using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{
    Material _Material;
    GameObject door;

    int MatId;


    // Start is called before the first frame update
    void Start()
    {

        door = GameObject.Find("Door4");
       
        _Material = door.GetComponent<Renderer>().materials[MatId];

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            _Material.SetColor("_TintColor", Color.cyan);
            door.GetComponent<BoxCollider>().enabled = true;
            //_Material.SetColor("_EmissionColor", Color.blue);
        }
    }
}