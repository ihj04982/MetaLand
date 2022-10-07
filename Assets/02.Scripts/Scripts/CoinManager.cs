using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public int delayTime;
    public static CoinManager instance;
    private void Awake()
    {
        CoinManager.instance = this;
    }

     int totalAmount;
     int getAmount;
    public TextMeshProUGUI ScreenText;
    public TextMeshProUGUI WorldText;
    public GameObject CoinImage;

    public AudioSource audioSource;

    public int WORLD_COIN
    {
        get { return getAmount; }
        set
        {
            getAmount = value;
            WorldText.enabled = true;
            CoinImage.SetActive(true);
            WorldText.text = "+" + getAmount;
            StopCoroutine("WaitForIt");
            StartCoroutine("WaitForIt");
            if (getAmount != 0)
            {
                audioSource.Play();
            }
        }
    }
    public int SCREEN_COIN
    {
        get { return totalAmount; }
        set
        {
            totalAmount = value;
            ScreenText.text = "" + totalAmount;
        }
    }
    IEnumerator WaitForIt()
    {
        yield return new WaitForSeconds(delayTime);
        WORLD_COIN = 0;
        WorldText.enabled = false;
        CoinImage.SetActive(false);


    }
    // Start is called before the first frame update
    void Start()
    {
        WORLD_COIN = 0;
        SCREEN_COIN = 0;
        WorldText.enabled = false;
        CoinImage.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }

  
    
}
