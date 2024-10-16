using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;

// Define difficulty levels
public enum EnemyDifficulty
{
    Easy,
    Medium,
    Hard
}

public abstract class EnemyModel : CharacterBase
{
    public class EnemyAttackInfo
    {
        public bool isAtk;
        public bool isDef;
        public bool isHeal;
        public int valueStat;
        public EnemyModel TargetenemyModel;

        public EnemyAttackInfo(bool isAtk,bool isDef,bool isHeal,EnemyModel enemyModel){
            this.isAtk = isAtk;
            this.isDef = isDef;
            this.isHeal = isHeal;
            this.TargetenemyModel = enemyModel;
        }
    }
    public class EnemyTargetBool{
        public bool isSuccess;
        public EnemyModel enemyModel;

        public EnemyTargetBool(bool isSuccess,EnemyModel enemyModel){
            this.isSuccess = isSuccess;
            this.enemyModel = enemyModel;
        }
        public bool Issuccess(){
            return isSuccess;
        }
        public EnemyModel TargetHelp(){
            return enemyModel;
        }
    }
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
    public static List<EnemyAttackInfo> AttackPlayer(List<EnemyModel> enemyModels, PlayerModel player,int shieldProp,int healProp)
    {
        bool shieldUsed = false;
        bool healUsed = false;
        int i = 0;
        List<EnemyAttackInfo> enemyAttackInfo = new List<EnemyAttackInfo>();
        foreach (EnemyModel enemy in enemyModels)
        {
            Debug.Log("Alive");
            if (!enemy.IsAlive()) continue;

            int decision = rng.Next(0, 100);

            if (decision < shieldProp && !shieldUsed)
            {
                EnemyTargetBool shielded = SetShieldToLowestHealthEnemy(enemyModels);
                if (shielded.Issuccess())
                {
                    shieldUsed = true;
                    enemyAttackInfo.Add(new EnemyAttackInfo(false, true, false, shielded.TargetHelp()));
                }
                else
                {
                    enemy.Attack(player);
                    enemyAttackInfo.Add(new EnemyAttackInfo(true, false, false, null)); // Log attack
                }
            }
            else if (decision < shieldProp + healProp && !healUsed)
            {
                EnemyTargetBool healed = HealLowHealthEnemy(enemyModels);
                if (healed.Issuccess())
                {
                    healUsed = true;
                    enemyAttackInfo.Add(new EnemyAttackInfo(false, false, true, healed.TargetHelp())); // Log heal
                }
                else
                {
                    enemy.Attack(player);
                    enemyAttackInfo.Add(new EnemyAttackInfo(true, false, false, null)); // Log attack
                }
            }
            else
            {
                enemy.Attack(player);
                enemyAttackInfo.Add(new EnemyAttackInfo(true, false, false, null)); // Log attack
                Debug.Log("Attack player");
            }
            i++;
            
        }
        return enemyAttackInfo;
        //return enemyModelList;
    }
    private static EnemyTargetBool SetShieldToLowestHealthEnemy(List<EnemyModel> enemies)
    {
        var lowestHealthEnemy = enemies
            .Where(e => e.IsAlive())
            .OrderBy(e => e.Health)
            .FirstOrDefault();

        if (lowestHealthEnemy != null)
        {
            lowestHealthEnemy.setShield(30);
            return new EnemyTargetBool(true,lowestHealthEnemy);
        }

        Debug.Log("No valid enemy found to shield.");
        return new EnemyTargetBool(false,enemies[0]);
    }
    private static EnemyTargetBool HealLowHealthEnemy(List<EnemyModel> enemies)
    {
        foreach (var enemy in enemies)
        {
            if (enemy.IsAlive())
            {
                enemy.HealByPercentage(0.2f);
                return new EnemyTargetBool(true,enemy);
            }
        }

        Debug.Log("No enemies with health below 20% found to heal.");
        return new EnemyTargetBool(false,enemies[0]);
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
