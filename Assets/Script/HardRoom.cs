using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HardRoom : HostileRoom
{
    public void Awake()
    {
        roomType = RoomType.Hard;
        minscore = 4;
        maxscore = 6;
    }
    public override void OnPlayerAttack()
    {
        base.OnPlayerAttack();
        Paper.Instance.SetSceneName("Hard");
        Paper.Instance.SetEnemyDifficulty(EnemyDifficulty.Hard);
        SceneManager.LoadSceneAsync(combatScene);
    }
}
