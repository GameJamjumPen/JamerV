using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class BattleController : MonoBehaviour
{   
    public EnemyUIManager enemyUIManager;
    public PlayerUIManager playerUIManager;
    public Sprite playerSprite; 
    public List<Sprite> FishSprites = new List<Sprite>();
    public List<Sprite> KniveSprites = new List<Sprite>();
    public List<Sprite> FolkSprites = new List<Sprite>();
    public List<Sprite> SpoonSprites = new List<Sprite>();
    public List<Sprite> BeeSprites = new List<Sprite>();
    public List<List<EnemyModel> > waves = new List<List<EnemyModel> >();
    public GameObject playerPrefab;
    private BattleModel battleModel;
    public bool isPlayerTurn = true;
    private PlayerModel player;
    private List<EnemyModel> enemies = new List<EnemyModel>(); //POLYMORPHISM NA KUB PIPI
    private List<CharacterView> enemyViews = new List<CharacterView>();
    public TextMeshProUGUI ShowTurn;
    public Image background;
    private int currentWave = -1;

    [Header("Assignable")]
    public int shieldprop = 30;
    public int healprop = 10;
    void Awake()
    {
        enemyUIManager = FindObjectOfType<EnemyUIManager>();
        playerUIManager = FindObjectOfType<PlayerUIManager>();

        if (enemyUIManager == null)
        {
            Debug.LogError("EnemyUIManager not found in the scene! Make sure it's attached to a GameObject.");
            return;
        }
        if(GenerateEnemyWave.Instance == null){
            Debug.LogError("WTF Singleton bug");
        }
        GenerateEnemyWave.Instance.CreateEnemyWaves();
        waves = GenerateEnemyWave.Instance.GetEnemyWaves();
        initPlayer();
        NewWave();
        // background.sprite = Paper.Instance.sprite;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (!isPlayerTurn){
                ShowCurrentTurn();
                EnemyTurn();
            }
        }
    }
    void initPlayer()
    {
        player = new PlayerModel();
        playerUIManager.UpdatePlayerUI(player, playerSprite);
    }

    void PlayerTurn()
    {
        FindEnemyAddAttack();
        Debug.Log("PlayerTurn");
        GameOver();
        isPlayerTurn = false;
    }
    public void PlayerClickAttack(){
        if(isPlayerTurn){
            BattleModel.ResetShield(player);
            FindEnemyAddAttack();
            playerUIManager.UpdatePlayerUI(player);
            isPlayerTurn = false;
        }
        
    }
    public void PlayerShield(){
        if(isPlayerTurn){
            BattleModel.ResetShield(player);
            player.setShield(30);
            playerUIManager.UpdatePlayerUI(player);
            isPlayerTurn = false;
        }
        
    }
    public void PlayerHealing(){
        if(isPlayerTurn){
            BattleModel.ResetShield(player);
            player.HealByAmount(30);
            playerUIManager.UpdatePlayerUI(player);
            isPlayerTurn = false;
            BattleModel.ResetShield(enemies);
        }
    }
    void EnemyTurn()
    {
        BattleModel.ResetShield(enemies);
        EnemyModel.AttackPlayer(enemies,player,shieldprop,healprop);
        enemyUIManager.updateUI(enemies);
        GameOver();
        isPlayerTurn = true;
    }

    void AttackCharacter(ICharacter attacker, ICharacter target)
    {
        if (attacker.IsAlive() && target.IsAlive())
        {
            attacker.Attack(target);
            Debug.Log(target.Name + " current health is " + target.Health.ToString());
        }
    }
    void FindEnemyAddAttack(){
        for(int i =0;i<enemies.Count;i++)
        {
            if (enemies[i].IsAlive())
            {   
                Debug.Log("Attack");
                AttackCharacter(player, enemies[i]);
                if(!enemies[i].IsAlive()){
                    Debug.Log("Is Dead");
                    enemyUIManager.SetActiveFalseOf(i);
                    // enemyInstances[i].SetActive(false);
                }
                enemyUIManager.updateUI(enemies);
                break;
            }
        }
    }
    void GameOver(){
        if(battleModel == null){
            Debug.LogError("BattleModel is NULL");
            return;
        }
        if (battleModel.player == null || battleModel.enemies == null){
            Debug.LogError("BattleModel's player or enemies are NULL");
            return;
        }
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
        // enemyInstances.Clear();
    }
    void NewWave(){
        Debug.Log("NEW WAVE !!!");
        currentWave++;
        ClearLists();
        RenderEnemy();
    }
    void RenderEnemy()
    
    {
        enemies = waves[currentWave];

        List<Sprite> enemySprites = new List<Sprite>();

        foreach (var enemy in enemies)
        {
            enemySprites.Add(GetRandomSpriteOf(enemy.Name));
        }
        enemyUIManager.DisplayNewWave(currentWave, enemies, enemySprites);
        
        battleModel = new BattleModel(player, enemies);

        if(battleModel == null){
            Debug.LogError("BattleModel is NULL after init");
            return;
        }
        if (battleModel.player == null || battleModel.enemies == null){
            Debug.LogError("BattleModel's player or enemies are NULL");
            return;
        }
    }

    Sprite GetRandomSpriteOf(string enemyName)
    {
        switch (enemyName)
        {
            case "Fish":
                return FishSprites[UnityEngine.Random.Range(0, FishSprites.Count)];
            case "Knive":
                return KniveSprites[UnityEngine.Random.Range(0, KniveSprites.Count)];
            case "Folk":
                return FolkSprites[UnityEngine.Random.Range(0, FolkSprites.Count)];
            case "Spoon":
                return SpoonSprites[UnityEngine.Random.Range(0, SpoonSprites.Count)];
            case "Bee":
                return BeeSprites[UnityEngine.Random.Range(0, BeeSprites.Count)];
            default:
                return FishSprites[0]; // Default fallback sprite
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
