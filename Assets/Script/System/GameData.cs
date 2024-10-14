using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string Name;

    public List<int> wentRoom;
    public List<CardSO> inventoryData;
    public int life;
    public int strength;
    public int defense;
    public int luck;
    public int statPoints;
    public List<RoomPlacement> roomPlacement;


    public GameData()
    {
        Name = "Player";
        inventoryData = new List<CardSO>();
        life = 3;
        strength = 1;
        defense = 1;
        luck = 1;
        statPoints = 0;
        wentRoom = new List<int>();
        roomPlacement = new List<RoomPlacement>();
    }
}

    [System.Serializable]
    public class RoomPlacement
    {
        public int gridInd;
        public int roomInd;

        public RoomPlacement(int gridInd, int roomInd)
        {
            this.gridInd = gridInd;
            this.roomInd = roomInd;
        }
    }
