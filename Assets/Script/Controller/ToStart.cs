using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (PlayerPrefs.HasKey("cutscene"))
            {
                SceneManager.LoadSceneAsync("MainBoard");
            }
            else{
                SceneManager.LoadSceneAsync("CutScene");
                PlayerPrefs.SetString("cutscene","0");
            }
        }
    }
}
