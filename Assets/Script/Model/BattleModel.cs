using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleModel : MonoBehaviour
{
    public CharacterModel player;
    public CharacterModel enemy;

    public BattleModel(CharacterModel player,CharacterModel enemy){
        this.player = player;
        this.enemy = enemy;
    }

    public void Attack(CharacterModel attacker,CharacterModel target){
        target.TakeDamage(attacker.attackPower);
    }
    public bool IsBattleOver(){
        return !(player.IsAlive() && enemy.IsAlive());
    }
}
