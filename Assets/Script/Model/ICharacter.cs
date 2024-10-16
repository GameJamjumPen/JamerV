
public interface ICharacter
{
    string Name { get; }
    int Health { get; }
    int AttackPower { get; }
    int MaxHealth {get;}
    int Shield {get;}
    void Attack(ICharacter target);
    void TakeDamage(int damage);
    bool IsAlive();
}
