using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public static PlayerHP instance;
    private void Awake()
    {
        instance = this;
    }
    float hp;
    public float maxHP = 10;
    public Slider s_sliderHP;
    public Slider w_sliderHP;

    public float HP
    {
        get { return hp; }
        set { 
            //w_sliderHP.fillRect.gameObject.SetActive(true);
            hp = value;
            s_sliderHP.value = value;
            w_sliderHP.value = value;
            StopCoroutine("WaitForIt");
            StartCoroutine("WaitForIt");
            //StartCoroutine("HPShow");
        }
    }
    //IEnumerator HPShow()
    //{
        
        
    //    yield return new WaitForSeconds(5.0f);
    //    w_sliderHP.fillRect.gameObject.SetActive(false);
    //}

    void Start()
    {
        s_sliderHP.maxValue = maxHP; 
        w_sliderHP.maxValue = maxHP;
        HP = maxHP;
        w_sliderHP.gameObject.SetActive(false);

    }
    IEnumerator WaitForIt()
    {
        w_sliderHP.gameObject.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        w_sliderHP.gameObject.SetActive(false);

    }
    
    private void Update()
    {
        if(hp <= 0)
        {
            StartCoroutine("IEDie");
        }
    }

    IEnumerator IEDie()
    {
        yield return new WaitForSeconds(3.0f);
        SceneController.instance.OnRestartClick();
    }
}
