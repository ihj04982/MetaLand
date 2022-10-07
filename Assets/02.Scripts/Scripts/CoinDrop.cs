using UnityEngine;

public class CoinDrop : MonoBehaviour
{
    public GameObject coinFactory;
    GameObject a;
    //public int minNum;
    //public int maxNum;
    // Start is called before the first frame update
    void Start()
    {
        a = Resources.Load<GameObject>("CFX3_Hit_SmokePuff");
       // if (this.gameObject.GetComponentInParent<GameObject>().name.Contains("Barrel_Box"))
      //  {
       //     Instantiate(a);
       //     a.transform.position = this.transform.position;
      //  }
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {

        
        if (other.gameObject.tag == "Weapon")
        {
            if (this.gameObject != null)
            {
                Instantiate(a);
                a.transform.position = transform.position;
                Destroy(this.gameObject);
                //Vector3 tf = transform.position;



                int randNum = Random.Range(0, 10);
                if (randNum < 1)
                {
                    GameObject coin = Instantiate(coinFactory);
                    coin.transform.position = transform.position;

                }
            }
        }
        
    }

    //for (int i = 0; i < randNum; i++)

}


