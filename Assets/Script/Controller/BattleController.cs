using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    private BattleModel battleModel = new BattleModel();
    private BattleView battleView = new BattleView();
    public bool isPlayerTurn = true;

    public int waveNumber = 2;  // Total number of waves
    public int currentWave = 0; // Track the current wave
    public int enemiesPerWave = 3;  // Max enemies per wave
    private List<CharacterModel> enemies = new List<CharacterModel>();
    private List<CharacterView> enemyViews = new List<CharacterView>();
    private List<GameObject> enemyInstances = new List<GameObject>();
    private CharacterModel player;
    private GameObject playerInstance;
    private CharacterView playerView;
    void Awake()
    {
        initPlayer();
        StartWave();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (isPlayerTurn)
                PlayerTurn();
            else
                EnemyTurn();
        }
    }

    void StartWave()
    {
        ClearLists();
        createEnemyWave();
        updateViewAndModel();
    }

    public void PlayerTurn()
    {
        if (isPlayerTurn)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if(attackEnemy(i)){
                    break;
                }
            }
            GameOver();
            isPlayerTurn = false;
        }
    }

    public void EnemyTurn()
    {
        if (!isPlayerTurn)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                attackPlayer(i);
            }
            GameOver();
            isPlayerTurn = true;
            Debug.Log("Finished Enemy Turn");
        }
    }
    // ==============================================================
    // ==============================================================
    // ==============================================================
    
    // Clear lists to avoid stale references
    void ClearLists()
    {
        enemies.Clear();
        enemyViews.Clear();
        enemyInstances.Clear();
    }
    void GameOver(){
        if (battleModel.IsBattleOver())
        {
            Debug.Log("Wave Over");
            if (currentWave < waveNumber && battleModel.player.IsAlive())
            {
                currentWave++;
                StartWave();
            }
            else
            {
                SceneChange.ChangeSceneFunc("MainBoard");  // Change scene if game is over
            }
            return;
        }
    }
    bool attackEnemy(int i){
        if (enemies[i].IsAlive())
        {
            battleModel.Attack(battleModel.player, enemies[i]);
            Debug.Log($"Enemy {enemies[i].Name} health: {enemies[i].health}");
            battleView.UpdateEnemyViews(enemies);
            if (!enemies[i].IsAlive())
            {
                enemyInstances[i].SetActive(false);
                enemies.RemoveAt(i);
                enemyViews.RemoveAt(i);
                enemyInstances.RemoveAt(i);
                i--;
            }
            return true;
        }
        return false;
    }
    void attackPlayer(int i){
        if (enemies[i].IsAlive())
        {
            battleModel.Attack(enemies[i], battleModel.player);
            Debug.Log($"Player health: {battleModel.player.health}");
            battleView.UpdatePlayerView(battleModel.player);
        }
    }
    void createEnemyWave(){
        for (int i = 0; i < enemiesPerWave; i++)
        {
            CharacterModel enemy = new CharacterModel($"Enemy {i + 1}", 100, 1);
            enemies.Add(enemy);

            GameObject enemyInstance = Instantiate(enemyPrefab, new Vector3(3 + i * 2, -2, 0), quaternion.identity);
            CharacterView enemyView = enemyInstance.GetComponent<CharacterView>();
            enemyViews.Add(enemyView);
            enemyInstances.Add(enemyInstance);
            Debug.Log($"Enemy {i + 1} created.");
        }
    }
    void initPlayer(){
        player = new CharacterModel("Prom", 100, 40);
        playerInstance = Instantiate(playerPrefab, new Vector3(-5, -2, 0), quaternion.identity);
        playerView = playerInstance.GetComponent<CharacterView>();

    }

    void updateViewAndModel(){
        battleModel = new BattleModel(player, enemies);
        battleView = new BattleView(playerView, enemyViews);
        // Update initial views
        battleView.UpdatePlayerView(player);
        battleView.UpdateEnemyViews(enemies);
    }
}
