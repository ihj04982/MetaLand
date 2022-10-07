using UnityEngine;

public class waterfall : MonoBehaviour
{
    public float damage;

    public float speed = 0.1f;
    Material mat;
    public AudioSource audioSource;

    
    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer rend = gameObject.GetComponent<MeshRenderer>();
        mat = rend.material;
        audioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        mat.mainTextureOffset += Vector2.up * speed * Time.deltaTime;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerHP.instance.HP -= damage;
            if(PlayerHP.instance.HP <= 0)
            {
                Debug.Log("Á×À½");
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            audioSource.Play();
        }
    }
}