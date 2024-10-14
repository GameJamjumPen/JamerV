using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    string Name { get; }
    int Health { get; }
    int AttackPower { get; }

    void Attack(ICharacter target);
    void TakeDamage(int damage);
    bool IsAlive();
}
