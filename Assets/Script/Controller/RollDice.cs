using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollDice : MonoBehaviour
{
    public void MainDiceRoll()
    {
        List<int> wentRoom = new List<int>() { 0, 5, 10 };
        Dice dice = new Dice();
        int rolledRoom = dice.MainRoll(wentRoom);
        Debug.Log("The room rolled is: " + rolledRoom);
    }
}
