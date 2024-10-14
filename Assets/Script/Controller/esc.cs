using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class esc : MonoBehaviour
{
    public GameObject escPrefab;
    public Scene thisScene;
    void Awake()
    {
        escPrefab.SetActive(false);
    }

    void Start()
    {
        thisScene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OpenEsc()
    {
        Time.timeScale=0;
        escPrefab.SetActive(true);
    }

    public void CloseEsc()
    {
        Time.timeScale = 1;
        escPrefab.SetActive(false);
    }

    public void exit()
    {
        Time.timeScale=1;
        Application.Quit();
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

}