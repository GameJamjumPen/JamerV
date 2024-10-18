using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToRestart : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Paper.Instance.sceneName = "";
            DataPersistenceMNG.Instance.NewGame();
            DataPersistenceMNG.Instance.SaveGame();
            SceneManager.LoadSceneAsync("StartScene");
        }
    }
}
