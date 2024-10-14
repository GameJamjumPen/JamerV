using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class BattleController : MonoBehaviour
{   
    
    public List<GameObject> FishPrefab  = new List<GameObject>();
    public List<GameObject> KnivePrefab = new List<GameObject>();
    public List<GameObject> FolkPrefab  = new List<GameObject>();
    public List<GameObject> SpoonPrefab = new List<GameObject>();
    public List<GameObject> BeePrefab   = new List<GameObject>();
    public List<List<EnemyModel> > waves = new List<List<EnemyModel> >();
    public GameObject playerPrefab;
    private BattleModel battleModel = new BattleModel();
    private BattleView battleView = new BattleView();
    public bool isPlayerTurn = true;
    private PlayerModel player;
    private List<EnemyModel> enemies = new List<EnemyModel>(); //POLYMORPHISM NA KUB PIPI
    private List<CharacterView> enemyViews = new List<CharacterView>();
    private List<GameObject> enemyInstances = new List<GameObject>();
    private CharacterView playerView;
    public TextMeshProUGUI ShowTurn;
    private int currentWave = -1;
    void Awake()
    {
        GenerateEnemyWave.Instance.CreateEnemyWaves();
        waves = GenerateEnemyWave.Instance.GetEnemyWaves();
        initPlayer();
        NewWave();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (isPlayerTurn){
                ShowCurrentTurn();
                PlayerTurn();
            }
            else{
                ShowCurrentTurn();
                EnemyTurn();
            }
        }
    }

    void initPlayer()
    {
        player = new PlayerModel();
        GameObject playerInstance = Instantiate(playerPrefab, new Vector3(-5, -2, 0), Quaternion.identity);
        playerView = playerInstance.GetComponent<CharacterView>();
        playerView.MaxHealth = player.Health;
    }

    void PlayerTurn()
    {
        if (isPlayerTurn)
        {   
            FindEnemyAddAttack();
            isPlayerTurn = false;
            GameOver();
        }
    }

    void EnemyTurn()
    {
        foreach (var enemy in enemies)
        {
            if (enemy.IsAlive())
            {
                AttackCharacter(enemy, player);
            }
            GameOver();
        }
        isPlayerTurn = true;
    }

    void AttackCharacter(ICharacter attacker, ICharacter target)
    {
        if (attacker.IsAlive() && target.IsAlive())
        {
            attacker.Attack(target);
            battleView.UpdatePlayerView(player);
            battleView.UpdateEnemyViews(enemies);
        }
    }
    void FindEnemyAddAttack(){
        for(int i =0;i<enemies.Count;i++)
        {
            if (enemies[i].IsAlive())
            {
                AttackCharacter(player, enemies[i]);
                if(!enemies[i].IsAlive()){
                    Debug.Log("Is Dead");
                    enemyInstances[i].SetActive(false);
                }
                break;
            }
        }
    }
    void updateViewAndModel()
    {
        battleModel = new BattleModel(player, enemies);
        battleView = new BattleView(playerView, enemyViews);

        battleView.UpdatePlayerView(player);
        battleView.UpdateEnemyViews(enemies);
    }
    void GameOver(){
        if (battleModel.IsBattleOver())
        {
            Debug.Log("Wave Over");
            if (currentWave < 2 && battleModel.player.IsAlive())
            {
                NewWave();
            }
            else
            {
                SceneChange.ChangeSceneFunc("MainBoard");
            }
            return;
        }
    }

    void ClearLists()
    {
        enemies.Clear();
        enemyViews.Clear();
        enemyInstances.Clear();
    }
    void NewWave(){
        Debug.Log("NEW WAVE !!!");
        currentWave++;
        ClearLists();
        RenderEnemy();
    }
    void RenderEnemy(){
        Debug.Log("currentWave is : " + currentWave.ToString());
        Debug.Log("Waves is " + waves.Count.ToString());
        enemies = waves[currentWave];
        for(int i =0;i<enemies.Count;i++){
            GameObject newenemy = GetRandomPrefabOf(enemies[i].Name);
            GameObject enemyins = Instantiate(newenemy,new Vector3(3+i*2,-2,10-i),Quaternion.identity);
            CharacterView enemyView = enemyins.GetComponent<CharacterView>();
            enemyView.MaxHealth = enemies[i].Health;
            enemyInstances.Add(enemyins);
            enemyViews.Add(enemyView);
        }
        updateViewAndModel();
    }

    GameObject GetRandomPrefabOf(String Name){
        
        switch (Name)
        {
            case "Fish":
                return FishPrefab[UnityEngine.Random.Range(0,FishPrefab.Count)];
            case "Knive":
                return KnivePrefab[UnityEngine.Random.Range(0,KnivePrefab.Count)];
            case "Folk":
                return FolkPrefab[UnityEngine.Random.Range(0,FolkPrefab.Count)];
            case "Spoon":
                return SpoonPrefab[UnityEngine.Random.Range(0,SpoonPrefab.Count)];
            case "Bee":
                return BeePrefab[UnityEngine.Random.Range(0,BeePrefab.Count)];
            default:
                return FishPrefab[UnityEngine.Random.Range(0,FishPrefab.Count)];
        }
    }
    void ShowCurrentTurn(){
        if(isPlayerTurn){
            ShowTurn.text = "Player Turn !";
            return;
        }
        ShowTurn.text = "Enemy Turn !";
    }
}
