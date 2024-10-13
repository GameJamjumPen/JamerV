using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleModel : MonoBehaviour
{
    public CharacterModel player;
    public List<CharacterModel> enemies;
    public BattleModel(){
        
    }

    public BattleModel(CharacterModel player, List<CharacterModel> enemies)
    {
        this.player = player;
        this.enemies = enemies;
    }

    public void Attack(CharacterModel attacker,CharacterModel target){
        target.TakeDamage(attacker.attackPower);
    }
    public bool IsBattleOver()
    {
        if (!player.IsAlive()) return true;
        // return enemies.TrueForAll(enemy => !enemy.IsAlive());
        foreach(var x in enemies){
            if(x.IsAlive()){
                return false;
            }
        }
        return true;
    }
}
