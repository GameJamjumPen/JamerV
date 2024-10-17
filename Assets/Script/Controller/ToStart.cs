using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToStart : MonoBehaviour
{

    public void Click()
    {
        DataPersistenceMNG.Instance.NewGame();
        SceneManager.LoadSceneAsync("CutScene");
    }

    public void Continue()
    {
        SceneManager.LoadSceneAsync("MainBoard");
    }
}
