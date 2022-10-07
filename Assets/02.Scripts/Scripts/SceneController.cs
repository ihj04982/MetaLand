using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    private void Awake()
    {
        instance = this;
    }
    public Animator animator;
    private int SceneToLoad;

    public GameObject screenUI;

    public void FadeToScene(int SceneIndex)
    {
        SceneToLoad = SceneIndex;
        animator.SetTrigger("Fade_Out");
    }
    public void OnFadeComplete()
    {
        SceneManager.LoadScene(SceneToLoad);
    }

    public void OnClickStart()
    {
        print("OnClickStart");
        FadeToScene(1);
    }
    public void OnClickExit()
    {
        print("OnClickQuit");
        Application.Quit();
    }
    public void OnHomeClick()
    {
        FadeToScene(0);
    }

    public void OnRestartClick()
    {
        FadeToScene(1);
    }
    //재시작 버튼을 누르면 재시작 하고 싶다

    public void UISetActive()
    {
        screenUI.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEndingEnter()
    {
        FadeToScene(2);
    }
}
