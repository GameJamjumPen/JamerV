using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HardRoom : HostileRoom
{
    public void Awake()
    {
        roomType = RoomType.Hard;
    }
    public override void OnPlayerAttack()
    {
        base.OnPlayerAttack();
        paper.SetEnemyDifficulty(EnemyDifficulty.Hard);
        SceneManager.LoadSceneAsync(combatScene);
    }
}
