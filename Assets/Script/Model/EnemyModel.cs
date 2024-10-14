using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel : CharacterBase
{
    public EnemyModel(string name, int health, int attackPower) 
        : base(name, health, attackPower) { }

    public override void Attack(ICharacter target)
    {
        target.TakeDamage(AttackPower);
        Debug.Log($"{Name} attacked {target.Name} for {AttackPower} damage!");
    }
}