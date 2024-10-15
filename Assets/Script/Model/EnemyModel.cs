using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Define difficulty levels
public enum EnemyDifficulty
{
    Easy,
    Medium,
    Hard
}

public abstract class EnemyModel : CharacterBase
{
    public EnemyDifficulty Difficulty { get; private set; }
    private static System.Random rng = new System.Random();
    public EnemyModel(string name, int health, int baseAttackPower, EnemyDifficulty difficulty) 
        : base(name, RandomizeStat(health), RandomizeStat(baseAttackPower))
    {
        Difficulty = difficulty;
    }

    // Randomize the attack power by ±20%
    private static int RandomizeStat(int baseValue, float minMultiplier = 1.0f, float maxMultiplier = 1.2f)
    {
        float randomMultiplier = Random.Range(minMultiplier, maxMultiplier);
        return Mathf.RoundToInt(baseValue * randomMultiplier);
    }
    public override void Attack(ICharacter target)
    {
        target.TakeDamage(AttackPower);
    }
    public static void Attack(int attackDamage,ICharacter target){
        target.TakeDamage(attackDamage);
    }
    public static void AttackPlayer(List<EnemyModel> enemyModels, PlayerModel player,int shieldProp,int healprop)
    {
        bool shieldUsed = false;
        bool healUsed = false;

        foreach (var enemy in enemyModels)
        {
            Debug.Log("Alive");
            if (!enemy.IsAlive()) continue;

            int decision = rng.Next(0, 100);

            if (decision < shieldProp && !shieldUsed)
            {
                bool shielded = SetShieldToLowestHealthEnemy(enemyModels);
                if (shielded) shieldUsed = true;
                else enemy.Attack(player);
            }
            else if (decision < shieldProp+healprop && !healUsed)
            {
                bool healed = HealLowHealthEnemy(enemyModels);
                if (healed) healUsed = true;
                else enemy.Attack(player);
            }
            else
            {
                enemy.Attack(player);
            }
        }
    }
    private static bool SetShieldToLowestHealthEnemy(List<EnemyModel> enemies)
    {
        var lowestHealthEnemy = enemies
            .Where(e => e.IsAlive())
            .OrderBy(e => e.Health)
            .FirstOrDefault();

        if (lowestHealthEnemy != null)
        {
            lowestHealthEnemy.setShield(30);
            return true;
        }

        Debug.Log("No valid enemy found to shield.");
        return false;
    }
    private static bool HealLowHealthEnemy(List<EnemyModel> enemies)
    {
        foreach (var enemy in enemies)
        {
            if (enemy.IsAlive())
            {
                enemy.HealByPercentage(0.2f);
                return true;
            }
        }

        Debug.Log("No enemies with health below 20% found to heal.");
        return false;
    }
}

public class Fish : EnemyModel
{
    public Fish() : base("Fish", 30, 10, EnemyDifficulty.Easy) { }

    public override void Attack(ICharacter target)
    {
        base.Attack(target);
    }
}
public class Knive : EnemyModel
{
    public Knive() : base("Knive", 50, 15, EnemyDifficulty.Medium) { }

    public override void Attack(ICharacter target)
    {
        base.Attack(target);
    }
}
public class Folk : EnemyModel
{
    public Folk() : base("Folk", 50, 15, EnemyDifficulty.Medium) { }
    public override void Attack(ICharacter target)
    {
        base.Attack(target);
    }
}
public class Spoon : EnemyModel
{
    public Spoon() : base("Spoon", 50, 15, EnemyDifficulty.Medium) { }

    public override void Attack(ICharacter target)
    {
        base.Attack(target);
    }
}

public class Bee : EnemyModel
{
    public Bee() : base("Bee", 100, 50, EnemyDifficulty.Hard) { }

    public override void Attack(ICharacter target)
    {
        base.Attack(target);
    }
}
