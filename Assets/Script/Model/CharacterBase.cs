using UnityEngine;

public abstract class CharacterBase : ICharacter
{
    public string Name { get; protected set; }
    public int Health { get; set; }
    public int AttackPower {get; set;}
    public int MaxHealth { get; protected set; }

    public int Shield { get; protected set; }
    public CharacterBase(string name, int health, int attackPower)
    {
        Name = name;
        Health = health;
        MaxHealth = health;
        AttackPower = attackPower;
        Shield = 0;
    }

    public abstract void Attack(ICharacter target);

    public bool IsAlive()
    {
        return Health > 0;
    }
    public virtual void TakeDamage(int damage)
    {
        if (Shield > 0)
        {
            // Reduce the shield first
            int shieldDamage = Mathf.Min(Shield, damage);
            Shield -= shieldDamage;
            damage -= shieldDamage;
        }

        if (damage > 0)
        {
            Health -= damage;
            Debug.Log($"Heal by{damage}");
        }
    }
    public void TakeDamageHealth(int damage){
        if (damage > 0)
        {
            Health -= damage;
            Debug.Log($"Heal by{damage}");
        }
    }
    public void HealByAmount(int amount)
    {
        Health = Mathf.Min(MaxHealth, Health + amount);
        Debug.Log($"Heal by{amount}");
    }
    public void HealByPercentage(float percentage)
    {
        int healAmount = Mathf.RoundToInt(MaxHealth * percentage / 100f);
        Health = Mathf.Min(MaxHealth, Health + healAmount);
    }
    public void setShield(int amount){
        Shield =amount;
        Debug.Log($"Heal by{amount}");
    }
    public static void Attack(int attackDamage,ICharacter target){
        target.TakeDamage(attackDamage);
    }
    
}
