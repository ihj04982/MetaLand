using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BowGet : MonoBehaviour
{
    public GameObject UI;
    public GameObject Bow;
    public GameObject canvas;
    PlayerFSM fsm;
    // Start is called before the first frame update
    void Start()
    {
        fsm = GameObject.Find("Player").GetComponent<PlayerFSM>();
        UI = GameObject.Find("Screen Canvas").transform.GetChild(6).gameObject;
    }
        // Update is called once per frame
    void Update()
    {
  
    }

    private void OnTriggerStay(Collider other)
    {
        canvas.SetActive(true);
        if (Input.GetKey(KeyCode.R))
        {
            UI.SetActive(true);
            fsm.getbow = true;
            Destroy(this.gameObject);
        }
    }
}
