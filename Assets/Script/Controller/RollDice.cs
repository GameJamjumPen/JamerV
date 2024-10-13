using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollDice : MonoBehaviour
{
    public RoomManager roomManager;
    public void MainDiceRoll()
    {
        List<int> wentRoom = new List<int>(){roomManager.rooms.Length};
        Dice dice = new Dice();
        int rolledRoom = dice.MainRoll(wentRoom);
        Debug.Log("The room rolled is: " + rolledRoom);
        roomManager.rooms[rolledRoom].OnPlayerAttack();
    }
}
