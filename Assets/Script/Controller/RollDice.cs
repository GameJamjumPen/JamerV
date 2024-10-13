using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollDice : MonoBehaviour ,IDataPersistence
{   
    // public RoomManager roomManager;
    List<int> wentRoom = new List<int>();

    public void LoadData(GameData data)
    {
        wentRoom= new List< int>(data.wentRoom);
    }
    public void SaveData(ref GameData data)
    {
        data.wentRoom = new List<int>(wentRoom);
    }
    public void MainDiceRoll()
    {
        Dice dice = new Dice();
        int rolledRoom = dice.MainRoll(wentRoom);
        Debug.Log("The room rolled is: " + rolledRoom);
        wentRoom.Add(rolledRoom);
        Debug.Log("Went Room is -----------------------------");
        foreach(var x in wentRoom){
            Debug.Log(x);
        }
        Debug.Log("-----------------------------");

        SceneChange.ChangeSceneFunc("TurnBaseCombat");
    }

}
