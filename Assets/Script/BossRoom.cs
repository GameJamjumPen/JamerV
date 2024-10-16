using UnityEngine;
using UnityEngine.SceneManagement;

public class BossRoom : HostileRoom
{
    public void Awake(){
    roomType = RoomType.Boss;
    minscore = 0;
    maxscore = 0;
    
}
    public override void OnPlayerAttack()
    {
        base.OnPlayerAttack();
        Paper.Instance.SetSceneName("Boss");
        Paper.Instance.SetEnemyDifficulty(EnemyDifficulty.Hard);
        SceneManager.LoadSceneAsync(combatScene);
    }
}

