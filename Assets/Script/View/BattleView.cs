using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleView : MonoBehaviour
{
    public CharacterView playerView;
    public CharacterView enemyView;
    public BattleView(CharacterView playerView,CharacterView enemyView){
        this.playerView = playerView;
        this.enemyView = enemyView;
    }
    public void UpdatePlayerView(CharacterModel player){
        playerView.SetHealth(player.health);
    }
    public void UpdateEnemyView(CharacterModel enemy){
        enemyView.SetHealth(enemy.health);
    }
}
