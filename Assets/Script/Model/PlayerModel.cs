
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerModel : CharacterBase
{
    public PlayerModel() : base("Player", 1000, 40) { }
    public override void Attack(ICharacter target)
    {
        target.TakeDamage(AttackPower);
    }
    public void attackAll(List<CharacterBase> target){
        foreach(ICharacter tar in target){
            tar.TakeDamage(AttackPower);
        }
    }
}
