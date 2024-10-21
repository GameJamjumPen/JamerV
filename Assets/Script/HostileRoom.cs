using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
public class HostileRoom : Room
{
    public List<List<EnemyType>> enemyTypes = new List<List<EnemyType>>();
    public int minscore;
    public int maxscore;
    public RoomType roomType;
    public Sprite _background;
    [Scene]
    public string combatScene;
    public void Awake(){
    }
    public override void OnPlayerAttack()
    {
        Paper.Instance.SetScore(UnityEngine.Random.Range(minscore, maxscore));
        Paper.Instance.SetCard(GameManager.singleton.cardSOs);
        Paper.Instance.SetBackground(_background);
    }
}

public enum RoomType{Easy, Medium , Hard, Boss}