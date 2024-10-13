using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit" , menuName ="SO/Unit")]
public class Unit : ScriptableObject , IDamagable
{
    public string _name;
    public Type type = Type.easy;
    public int _health;
    public int _attackPower;
    public GameObject _unitPrefab;
    public void TakeDamage(int damage){
        _health -= damage;
        if(_health < 0) _health = 0;
    }
    public bool IsAlive()
    {
        return _health > 0;
    }
}
public enum Type{
    player,
    easy,
    medium,
    hard
}
