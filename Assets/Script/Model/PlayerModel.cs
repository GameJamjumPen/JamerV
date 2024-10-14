
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerModel : CharacterBase
{
    public PlayerModel() : base("Player", 100, 40) { }

    public override void Attack(ICharacter target)
    {
        target.TakeDamage(AttackPower);
        Debug.Log($"{Name} attacked {target.Name} for {AttackPower} damage!");
    }
}
