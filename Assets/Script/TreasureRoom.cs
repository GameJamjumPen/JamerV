using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureRoom : Room
{
    //reward
    public CardSO _cardReward;

    public override void OnPlayerAttack()
    {
        Inventory inventory = FindObjectOfType<Inventory>();
        // inventory.AddItem(_cardReward);
        Debug.Log("GO to Treasure");
        inventory.istoggleable = true;
    }
}
