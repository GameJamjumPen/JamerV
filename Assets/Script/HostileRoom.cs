using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HostileRoom : Room
{
    public RoomType roomType;
    public Paper paper;
    public Sprite _background;
    [Scene]
    public string combatScene;
    public void Awake(){
        paper = FindObjectOfType<Paper>();
    }
    public override void OnPlayerAttack()
    {
        paper.SetCard(GameManager.singleton.cardSOs);
        paper.SetBackground(_background);
    }
}

public enum RoomType{Easy, Medium , Hard}