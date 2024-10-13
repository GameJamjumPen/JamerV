using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;

public class GameData
{
    public string Name;
    public Dictionary<string, int> inventoryData;

    public GameData()
    {
        Name = "Player";
        inventoryData = new Dictionary<string, int>();
    }

}
