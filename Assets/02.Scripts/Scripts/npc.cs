using TMPro;
using UnityEngine;

public class npc : MonoBehaviour
{
   
    public Canvas canvas;
    public TextMeshProUGUI text;
    public float detectedRadius;
    public string[] words;
    int index;

    public GameObject key;
    public int coinForKey;
    
    public Animator anim;

    public AudioSource audioSource;
    public AudioClip idleClip;
    public AudioClip happyClip;


    // Start is called before the first frame update
    void Start()
    {
        canvas.enabled = false;
        text.text = words[0];
        words[2] = coinForKey + " Coins ¢º Key";
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        int layer = 1 << LayerMask.NameToLayer("Player");
        Collider[] cols = Physics.OverlapSphere(transform.position, detectedRadius, layer);
        if (cols.Length > 0)
        {
            canvas.enabled = true;
            //clip = audioSource.GetComponent<AudioClip>();
            audioSource.clip = idleClip;
            audioSource.Play();
        }
        else { canvas.enabled = false; }
    }
    public void Talk()
    {
        if (index < words.Length - 2)
        {
            index++;
        }
        else if (CoinManager.instance.SCREEN_COIN >= coinForKey )
        //else if (CoinManager.instance.SCREEN_COIN >= coinForKey && index >= words.Length - 2)
        {
            CoinManager.instance.SCREEN_COIN = CoinManager.instance.SCREEN_COIN - coinForKey;
            key.SetActive(true);
            index = words.Length - 1;
            audioSource.clip = happyClip;
            audioSource.Play();
            anim.SetTrigger("Happy");
            
        }
        text.text = words[index];

    }





    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (CoinManager.instance.SCREEN_COIN >= coinForKey && index >= words.Length-2 )
    //    {
    //        CoinManager.instance.SCREEN_COIN = CoinManager.instance.SCREEN_COIN - coinForKey;
    //        GameObject key = Instantiate(keyFactory);
    //        key.transform.position = keyPosition.position;
    //        index = words.Length - 1;
    //    }

    //}
}
