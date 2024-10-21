using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType : MonoBehaviour
{
    public enum Enemytype{
        Fish,
        Knive,
        Folk,
        Spoon,
        Bee,
        Prince,
    }
    public List<List<Enemytype>> enemyGrid = new List<List<Enemytype>>();
}
