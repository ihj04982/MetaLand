using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            CoinManager.instance.WORLD_COIN++;
            CoinManager.instance.SCREEN_COIN++;
            //CoinManager.instance.coinAmount++;
            //CoinManager.instance.getAmount++;
            Destroy(this.gameObject);
        }
    }

}
