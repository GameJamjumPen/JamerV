using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleView : MonoBehaviour
{
    public CharacterView playerView;
    public List<CharacterView> enemyViews = new List<CharacterView>();
    public BattleView(){
        
    }
    public BattleView(CharacterView playerView,List<CharacterView> enemyViews){
        this.playerView = playerView;
        this.enemyViews = enemyViews;
    }
    public void UpdatePlayerView(CharacterModel player){
        playerView.SetHealth(player.health);
    }
    public void UpdateEnemyViews(List<CharacterModel> enemies)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemyViews[i].SetHealth(enemies[i].health);
        }
    }
}
