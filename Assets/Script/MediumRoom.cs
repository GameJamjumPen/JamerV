using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MediumRoom : HostileRoom
{
    public void Awake()
    {
        roomType = RoomType.Medium;
    }
    public override void OnPlayerAttack()
    {
        base.OnPlayerAttack();
        Paper.Instance.SetEnemyDifficulty(EnemyDifficulty.Medium);
        SceneManager.LoadSceneAsync(combatScene);
    }
}
