using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EasyRoom : HostileRoom
{
    public void Awake()
    {
        roomType = RoomType.Easy;
    }
    public override void OnPlayerAttack()
    {
        base.OnPlayerAttack();
        paper.SetEnemyDifficulty(EnemyDifficulty.Easy);
        SceneManager.LoadSceneAsync(combatScene);
    }
}
