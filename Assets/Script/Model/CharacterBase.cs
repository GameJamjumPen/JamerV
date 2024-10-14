using UnityEngine;

public abstract class CharacterBase : ICharacter
{
    public string Name { get; protected set; }
    public int Health { get; set; }
    public int AttackPower { get; protected set; }

    public CharacterBase(string name, int health, int attackPower)
    {
        Name = name;
        Health = health;
        AttackPower = attackPower;
    }

    public abstract void Attack(ICharacter target);

    public bool IsAlive()
    {
        return Health > 0;
    }
    public void TakeDamage(int x){
        this.Health -= x;
    }
}
