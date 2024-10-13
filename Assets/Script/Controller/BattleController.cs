using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BattleController : MonoBehaviour
{   
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    private BattleModel battleModel;
    private BattleView battleView;
    public bool isPlayerTurn = true;

    void Start() {
        CharacterModel player = new CharacterModel("Player",100,10);
        CharacterModel enemy = new CharacterModel("Enemy",100,8);

        GameObject enemmyInstance = Instantiate(enemyPrefab,new Vector3(5,-2,0),quaternion.identity);
        GameObject playerInstance = Instantiate(playerPrefab,new Vector3(-5,-2,0),quaternion.identity);

        CharacterView enemyView = enemmyInstance.GetComponent<CharacterView>();
        CharacterView playerView = playerInstance.GetComponent<CharacterView>();

        battleModel = new BattleModel(player,enemy);

        battleView = new BattleView(playerView,enemyView);
        battleView.playerView.setName(player.Name);
        battleView.enemyView.setName(enemy.Name);
        battleView.UpdatePlayerView(player);
        battleView.UpdateEnemyView(enemy);
    }
    void Update() {
        if(Input.GetKeyDown(KeyCode.W)){
            Debug.Log("isPlayerTurn : " + isPlayerTurn.ToString());
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
            Debug.Log("Enemy"+ battleModel.enemy.health);
            battleView.UpdateEnemyView(battleModel.enemy);
            if(battleModel.IsBattleOver()){
                Debug.Log("Over");
                return;
            }
            isPlayerTurn = false;
            Debug.Log("Finished PlayerTurn");
        }
    }
    public void EnemyTurn(){
        if(!isPlayerTurn){
            battleModel.Attack(battleModel.enemy,battleModel.player);
            Debug.Log(battleModel.player.health);
            battleView.UpdatePlayerView(battleModel.player);
            if(battleModel.IsBattleOver()){
                Debug.Log("Over");
                return;
            }
            isPlayerTurn = true;
            Debug.Log("Finished EnemyTurn");
        }
    }
}
