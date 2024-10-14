using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    // Difficulty Levels
    public enum Difficulty { Easy, Medium, Hard, Boss }

    // Enemy Prefab Class to Store Prefab and Difficulty
    [Serializable]
    public class EnermyPrefab
    {
        public GameObject prefab;
        public Difficulty difficulty;

        public EnermyPrefab() { }

        public EnermyPrefab(GameObject prefab, Difficulty difficulty)
        {
            this.prefab = prefab;
            this.difficulty = difficulty;
        }
    }

    private Difficulty selectedDifficulty;  // Selected difficulty for the current wave

    public GameObject playerPrefab;
    public List<EnermyPrefab> enemyPrefab = new List<EnermyPrefab>();  // List of enemy prefabs

    private BattleModel battleModel = new BattleModel();
    private BattleView battleView = new BattleView();
    public bool isPlayerTurn = true;

    public int waveNumber = 2;  // Total waves
    public int currentWave = 0;  // Track the current wave
    public int enemiesPerWave = 3;  // Max enemies per wave

    private PlayerModel player;
    private List<EnemyModel> enemies = new List<EnemyModel>(); //POLYMORPHISM NA KUB PIPI
    private List<CharacterView> enemyViews = new List<CharacterView>();
    private List<GameObject> enemyInstances = new List<GameObject>();
    private CharacterView playerView;

    void Awake()
    {
        SetRandomDifficulty();  // Assign a random difficulty before starting
        initPlayer();  // Initialize player
        StartWave();  // Start the first wave
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

    void initPlayer()
    {
        player = new PlayerModel();
        GameObject playerInstance = Instantiate(playerPrefab, new Vector3(-5, -2, 0), Quaternion.identity);
        playerView = playerInstance.GetComponent<CharacterView>();
    }

    void createEnemyWave()
    {
        List<EnermyPrefab> filteredEnemies = enemyPrefab.FindAll(e => e.difficulty == selectedDifficulty);

        if (filteredEnemies.Count == 0)
        {
            return;
        }

        for (int i = 0; i < enemiesPerWave; i++)
        {
            EnermyPrefab randomEnemyPrefab = filteredEnemies[UnityEngine.Random.Range(0, filteredEnemies.Count)];

            EnemyModel enemy = new EnemyModel(randomEnemyPrefab.prefab.name, 100, 1);
            enemies.Add(enemy);

            
            GameObject enemyInstance = Instantiate(
                randomEnemyPrefab.prefab, 
                new Vector3(3 + i * 2, -2, 0), 
                Quaternion.identity
            );
            CharacterView enemyView = enemyInstance.GetComponent<CharacterView>();
            enemyViews.Add(enemyView);
            enemyInstances.Add(enemyInstance);

        }
    }

    void PlayerTurn()
    {
        if (isPlayerTurn)

        {   for(int i =0;i<enemies.Count;i++)
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

    void ClearLists()
    {
        enemies.Clear();
        enemyViews.Clear();
        enemyInstances.Clear();
    }

    void updateViewAndModel()
    {
        battleModel = new BattleModel(player, enemies);
        battleView = new BattleView(playerView, enemyViews);

        battleView.UpdatePlayerView(player);
        battleView.UpdateEnemyViews(enemies);
    }

    void SetRandomDifficulty()
    {
        Difficulty[] difficulties = (Difficulty[])Enum.GetValues(typeof(Difficulty));
        selectedDifficulty = difficulties[UnityEngine.Random.Range(0, difficulties.Length)];

        if (selectedDifficulty == Difficulty.Boss)
        {
            waveNumber = 1;
            enemiesPerWave = 1;
        }

    }
    void GameOver(){
        if (battleModel.IsBattleOver())
        {
            Debug.Log("Wave Over");
            if (currentWave < waveNumber - 1 && battleModel.player.IsAlive())
            {
                currentWave++;
                StartWave();
            }
            else
            {
                SceneChange.ChangeSceneFunc("MainBoard");
            }
            return;
        }
    }
}
