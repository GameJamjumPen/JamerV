using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public EnemyModel(string name, int health, int attackPower, EnemyDifficulty difficulty) 
        : base(name, health, attackPower)
    {
        Difficulty = difficulty;
    }

    public override void Attack(ICharacter target)
    {
        target.TakeDamage(AttackPower);
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
