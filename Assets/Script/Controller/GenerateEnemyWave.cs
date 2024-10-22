using System;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemyWave : Singleton<GenerateEnemyWave>
{

    public List<List<EnemyType.Enemytype> > enemyTypes = new List<List<EnemyType.Enemytype> >();

    public List<List<EnemyModel> > waves = new List<List<EnemyModel> >();

    public List<List<EnemyModel> > GetEnemyWaves(){
        return waves;
    }
    private static Dictionary<EnemyType.Enemytype, Func<EnemyModel>> enemyTypeToModelMap = 
        new Dictionary<EnemyType.Enemytype, Func<EnemyModel>>()
    {
        { EnemyType.Enemytype.Fish, () => new Fish() },
        { EnemyType.Enemytype.Knive, () => new Knive() },
        { EnemyType.Enemytype.Folk, () => new Folk() },
        { EnemyType.Enemytype.Spoon, () => new Spoon() },
        { EnemyType.Enemytype.Bee, () => new Bee() },
        { EnemyType.Enemytype.Prince, () => new Prince() }
    };
    private List<EnemyModel> GenerateWave(int waveIndex)
    {
        List<EnemyModel> wave = new List<EnemyModel>();

        if (waveIndex >= enemyTypes.Count)
        {
            Debug.LogError("Invalid wave index. Enemy types list is smaller than the requested number of waves.");
            return wave;
        }

        List<EnemyType.Enemytype> currentWaveTypes = enemyTypes[waveIndex];

        foreach (var enemyType in currentWaveTypes)
        {
            if (enemyTypeToModelMap.ContainsKey(enemyType))
            {
                EnemyModel enemy = enemyTypeToModelMap[enemyType]();
                wave.Add(enemy);
            }
            else
            {
                Debug.LogError($"Enemy type {enemyType} is not mapped to an enemy model.");
            }
        }

        return wave;
    }
    public void CreateEnemyWaves()
    {
        enemyTypes = Paper.Instance.enemyTypes;
        waves.Clear();
        Debug.Log(enemyTypes.Count.ToString());
        for (int i = 0; i < enemyTypes.Count; i++)
        {
            List<EnemyModel> wave = GenerateWave(i);
            waves.Add(wave);
        }
        Debug.Log("Waves Size? = " + waves.Count.ToString());
        Debug.Log("Enemy waves created!");
    }

}
