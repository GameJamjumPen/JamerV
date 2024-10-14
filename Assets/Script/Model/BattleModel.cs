using System.Collections.Generic;
using UnityEngine;

public class BattleModel : MonoBehaviour
{
    public PlayerModel player;  // Player object
    public List<EnemyModel> enemies;  // List of enemies

    // Default constructor
    public BattleModel() { }

    // Constructor to initialize the player and enemy list
    public BattleModel(PlayerModel player, List<EnemyModel> enemies)
    {
        this.player = player;
        this.enemies = enemies;
    }

    // Attack method using CharacterBase to handle both player and enemy attacks
    //POLYMORPHISM NA KUB PIPI
    public void Attack(CharacterBase attacker, CharacterBase target)
    {
        if (attacker.IsAlive())
        {
            target.TakeDamage(attacker.AttackPower);
            Debug.Log($"{attacker.Name} attacked {target.Name} for {attacker.AttackPower} damage!");
        }
    }

    // Check if the battle is over
    public bool IsBattleOver()
    {
        // If the player is dead, the battle is over
        if (!player.IsAlive()) return true;

        // Check if all enemies are dead
        foreach (var enemy in enemies)
        {
            if (enemy.IsAlive())
            {
                return false;  // If any enemy is alive, the battle continues
            }
        }

        return true;  // All enemies are dead, battle is over
    }
}
