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
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        playerMovement.PlayerMove(rolledRoom);
        GameManager.singleton.RoomEnter(rolledRoom); //Playerควรรอให้เดินถึงก่อน
        Debug.Log("Row");
        //SceneChange.ChangeSceneFunc("TurnBaseCombat");
    }

    public int Roll()
    {
        Dice dice = new Dice();
        int rolledRooom = dice.MainRoll(wentRoom);
        wentRoom.Add(rolledRooom);
        return rolledRooom;
    }

}
