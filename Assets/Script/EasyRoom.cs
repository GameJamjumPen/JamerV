using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EasyRoom : HostileRoom
{
    public void Awake()
    {
        roomType = RoomType.Easy;
        minscore = 1;
        maxscore = 2;
    }
    public override void OnPlayerAttack()
    {
        base.OnPlayerAttack();
        Paper.Instance.SetSceneName("Easy");
        Paper.Instance.SetEnemyDifficulty(EnemyDifficulty.Easy);
        SceneManager.LoadSceneAsync(combatScene);
    }
}
