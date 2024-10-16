using System;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemyWave : Singleton<GenerateEnemyWave>
{

    private EnemyDifficulty difficulty;

    public List<List<EnemyModel> > waves = new List<List<EnemyModel> >();

    public List<List<EnemyModel> > GetEnemyWaves(){
        return waves;
    }
    public void CreateEnemyWaves()
    {
        // RandomSetDifficulty();
        difficulty = Paper.Instance.enemyDifficulty;
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
                return UnityEngine.Random.Range(2, 4);
            case EnemyDifficulty.Hard:
                return UnityEngine.Random.Range(1, 3);
            default:
                return 3;
        }
    }

}
