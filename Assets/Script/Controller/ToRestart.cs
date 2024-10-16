using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToRestart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DataPersistenceMNG.Instance.NewGame();
            SceneManager.LoadSceneAsync("StartScene");
            
        }
    }
}
