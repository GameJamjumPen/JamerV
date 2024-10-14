using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterModel : MonoBehaviour , IDamagable
{
    public string Name;
    public int health;
    public int attackPower;

    public CharacterModel(string name, int health, int attackPower)
    {
        this.Name = name;
        this.health = health;
        this.attackPower = attackPower;
    }
    public void TakeDamage(int damage){
        health -= damage;
        if(health < 0) health = 0;
    }
    public bool IsAlive()
    {
        return health > 0;
    }
}
