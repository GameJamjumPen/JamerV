using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HostileRoom : Room
{
    public RoomType roomType ;
    public EnemyModel[] enemyModels;

    public override void OnPlayerAttack()
    {
        throw new System.NotImplementedException();
    }
}

public enum RoomType{Easy, Medium , Hard}