using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureRoom : Room
{
    //reward
    public CardSO _cardReward;

    void Awake()
    {
        Treasure = true;
    }

    public override void OnPlayerAttack()
    {
        Inventory inventory = FindObjectOfType<Inventory>();
        inventory.AddItem(_cardReward,_cardReward._cardSprite);
        Debug.Log("GO to Treasure");
        inventory.istoggleable = true;
        RollDice dice = FindObjectOfType<RollDice>();
        dice.canRoll = true;
    }

    //UI display
    public void UIDisplay()
    {

    }
}
