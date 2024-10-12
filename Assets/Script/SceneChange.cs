using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void ChangeSceneFunc(string scene){
        SceneManager.LoadSceneAsync(scene);
    }
}
