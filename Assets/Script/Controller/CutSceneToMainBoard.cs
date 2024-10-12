using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneToMainBoard : MonoBehaviour
{
    public void LoadNextScene(int x)
    {
        SceneManager.LoadScene(x);
    }
}
