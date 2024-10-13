using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

public class Room : MonoBehaviour
{
    public bool isOwned;
    [Scene]
    public string roomScene;
    public void OnPlayerAttack(){
        Debug.Log("Player Attack on"+ this.name);
        //SceneManager.LoadSceneAsync(roomScene);
    }
}