using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;

public class GameData
{
    // public Vector3 playerPos;
    public int Coin;
    public Dictionary<string, int> inventoryData;

    public GameData()
    {
        // playerPos = Vector3.zero;
        Coin = 100;
        inventoryData = new Dictionary<string, int>();
    }

}
