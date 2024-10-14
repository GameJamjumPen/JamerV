using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HostileRoom : Room
{
    public string _rewardPoint;
    public RoomType roomType;
    public RoomManager roomManager;
    public void Awake()
    {
        roomManager = FindObjectOfType<RoomManager>();
        switch (roomType)
        {
            case RoomType.Easy :
            roomScene = roomManager.Scenes[0];
            break;
            case RoomType.Medium :
            roomScene = roomManager.Scenes[1];
            break;
            case RoomType.Hard :
            roomScene = roomManager.Scenes[2];
            break;
        }
    }
    public override void OnPlayerAttack()
    {
        SceneManager.LoadSceneAsync(roomScene);
    }
}

public enum RoomType{Easy, Medium , Hard}