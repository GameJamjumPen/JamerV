using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemyWave : MonoBehaviour
{
    public static GenerateEnemyWave Instance;

    private EnemyDifficulty difficulty;

    public List<List<EnemyModel> > waves = new List<List<EnemyModel> >();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void CreateEnemyWaves()
    {
        waves.Clear();

        for (int i = 0; i < 3; i++)
        {
            List<EnemyModel> wave = GenerateWave();
            waves.Add(wave);
        }

        Debug.Log("Enemy waves created!");
    }
    public void RandomSetDifficulty()
    {
        Array difficulties = System.Enum.GetValues(typeof(EnemyDifficulty));
        difficulty = (EnemyDifficulty)difficulties.GetValue(UnityEngine.Random.Range(0, difficulties.Length));
        Debug.Log($"Selected Difficulty: {difficulty}");
    }
    private List<EnemyModel> GenerateWave()
    {
        List<EnemyModel> wave = new List<EnemyModel>();

        int enemyCount = GetEnemyCountForCurrentDifficulty();

        for (int i = 0; i < enemyCount; i++)
        {
            EnemyModel enemy = EnemyFactory.CreateEnemy(difficulty);
            wave.Add(enemy);
        }

        return wave;
    }

    private int GetEnemyCountForCurrentDifficulty()
    {
        switch (difficulty)
        {
            case EnemyDifficulty.Easy:
                return 3;
            case EnemyDifficulty.Medium:
                return UnityEngine.Random.Range(2, 4); // 2 to 3 enemies
            case EnemyDifficulty.Hard:
                return UnityEngine.Random.Range(1, 3); // 1 to 2 enemies
            default:
                return 3; // Default to 3 enemies if something goes wrong
        }
    }

}
