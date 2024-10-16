using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public static void ChangeSceneFunc(string scene){
        SceneManager.LoadSceneAsync(scene);
    }
}
