using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHolder : MonoBehaviour
{
    public EnemyModel enemyContain;
    public bool isSelected;
    public BattleInventory battleInventory;

    public void Awake(){
        battleInventory = FindObjectOfType<BattleInventory>();
    }
    public void Deselected(){
        isSelected = false;
    }
    public void Selected(){
        battleInventory.DeselectedAllHolder();
        isSelected = true;
        battleInventory.enemyHolder = this;
    }
}
