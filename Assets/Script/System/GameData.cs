using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;

public class GameData
{
    public string Name;

    public List<int> wentRoom;
    public Dictionary<string, int> inventoryData;

    public GameData()
    {
        Name = "Player";
        inventoryData = new Dictionary<string, int>();
        wentRoom = new List<int>();
    }

}
