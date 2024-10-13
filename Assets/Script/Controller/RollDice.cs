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
        int rolledRoom = Roll();
        SceneChange.ChangeSceneFunc("TurnBaseCombat");
    }

    private int Roll()
    {
        Dice dice = new Dice();
        int rolledRooom = dice.MainRoll(wentRoom);
        return rolledRooom;
    }

}
