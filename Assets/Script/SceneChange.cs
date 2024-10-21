using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public GameObject Begin;
    public static void ChangeSceneFunc(string scene){
        SceneManager.LoadSceneAsync(scene);
    }
    public void BeginObj()
    {
        Begin.SetActive(true);
    }
}
