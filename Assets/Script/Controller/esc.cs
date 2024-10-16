using UnityEngine;
using UnityEngine.SceneManagement;

public class esc : MonoBehaviour
{
    public GameObject escPrefab;
    public Collider2D dice;

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
        escPrefab.SetActive(true);
        dice.enabled = false;
    }

    public void ClickSound()
    {
        SoundManager.Instance.PlaySFX("tap");
    }

    public void CloseEsc()
    {
        escPrefab.SetActive(false);
        dice.enabled = true;
        {
            
        }
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
