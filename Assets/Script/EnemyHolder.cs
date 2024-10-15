using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHolder : MonoBehaviour
{
    public EnemyModel enemyContain;
    public void AddEnemy(EnemyModel enemy){
        enemyContain = enemy;
    }
    public void ClearEnemy(){
        enemyContain = null;
    }
}
