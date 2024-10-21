using System;
using System.Collections.Generic;

public class EnemyFactory
{
    private static Dictionary<EnemyDifficulty, List<Func<EnemyModel>>> enemyCreators = 
        new Dictionary<EnemyDifficulty, List<Func<EnemyModel>>>
    {
        {
            EnemyDifficulty.Easy, new List<Func<EnemyModel>>
            {
                () => new Fish()
            }
        },
        {
            EnemyDifficulty.Medium, new List<Func<EnemyModel>>
            {
                () => new Knive(),
                () => new Folk(),
                () => new Spoon(),
            }
        },
        {
            EnemyDifficulty.Hard, new List<Func<EnemyModel>>
            {
                () => new Bee(),
            }
        },
        {
            EnemyDifficulty.Boss, new List<Func<EnemyModel>>
            {
                () => new Prince(),
            }
        }
    };

    public static EnemyModel CreateEnemy(EnemyDifficulty difficulty)
    {
        if (!enemyCreators.ContainsKey(difficulty))
        {
            throw new ArgumentException($"No enemies available for difficulty: {difficulty}");
        }
        List<Func<EnemyModel>> possibleEnemies = enemyCreators[difficulty];

        int randomIndex = UnityEngine.Random.Range(0, possibleEnemies.Count);

        return possibleEnemies[randomIndex]();
    }
}
