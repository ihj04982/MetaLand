using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    Material _Material;
    GameObject door;

    int MatId;
    public float SwitchTime = 0.5f;
    public Image key;

    // Start is called before the first frame update
    void Start()
    {

        door = GameObject.Find("Door2");
        key = GameObject.Find("Image key").GetComponent<Image>();
        
        _Material = door.GetComponent<Renderer>().materials[MatId];
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            _Material.SetColor("_TintColor", Color.cyan );
            door.GetComponent<BoxCollider>().enabled = true;
            key.enabled = true;
            Destroy(gameObject);
            //_Material.SetColor("_EmissionColor", Color.blue);
        }
    }
}
