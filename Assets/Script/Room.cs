using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

public abstract class Room : MonoBehaviour
{
    public bool isOwned;
    [Scene]
    public string roomScene;
    public abstract void OnPlayerAttack();
}