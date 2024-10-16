using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MediumRoom : HostileRoom
{
    public void Awake()
    {
        roomType = RoomType.Medium;
        minscore = 2;
        maxscore =4;
    }
    public override void OnPlayerAttack()
    {
        base.OnPlayerAttack();
        Paper.Instance.SetSceneName("Medium");
        Paper.Instance.SetEnemyDifficulty(EnemyDifficulty.Medium);
        SceneManager.LoadSceneAsync(combatScene);
    }
}
