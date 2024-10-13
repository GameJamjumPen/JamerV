using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BattleController : MonoBehaviour
{   
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public BattleModel battleModel;
    public BattleView battleView;
    public bool isPlayerTurn = true;

    void Start() {
        CharacterModel player = new CharacterModel("Player",100,10);
        CharacterModel enemy = new CharacterModel("Enemy",100,8);

        GameObject enemmyInstance = Instantiate(enemyPrefab,new Vector2(10,0),quaternion.identity);
        GameObject playerInstance = Instantiate(playerPrefab,new Vector2(5,0),quaternion.identity);

        CharacterView enemyView = enemmyInstance.GetComponent<CharacterView>();
        CharacterView playerView = playerInstance.GetComponent<CharacterView>();

        battleModel = new BattleModel(player,enemy);

        battleView = new BattleView(playerView,enemyView);
        battleView.playerView.setName(player.name);
        battleView.enemyView.setName(enemy.name);
        battleView.UpdatePlayerView(player);
        battleView.UpdateEnemyView(enemy);
    }
    void Update() {
        if(Input.GetKeyDown(KeyCode.W)){
            if(isPlayerTurn){
                PlayerTurn();
            }
            else{
                EnemyTurn();
            }
        }
    }
    public void PlayerTurn(){
        if(isPlayerTurn){
            battleModel.Attack(battleModel.player,battleModel.enemy);
            battleView.UpdateEnemyView(battleModel.enemy);
            if(battleModel.IsBattleOver()){
                return;
            }
            isPlayerTurn = false;
            EnemyTurn();
        }
    }
    public void EnemyTurn(){
        if(!isPlayerTurn){
            battleModel.Attack(battleModel.enemy,battleModel.player);
            battleView.UpdatePlayerView(battleModel.player);
            if(battleModel.IsBattleOver()){
                return;
            }
            isPlayerTurn = true;
            PlayerTurn();
        }
    }
}
