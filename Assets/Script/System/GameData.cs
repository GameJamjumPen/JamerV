using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;

public class GameData
{
    public string Name;
    public Dictionary<string, int> inventoryData;
    public int life;
    public int strength;
    public int defense;
    public int luck;
    public int statPoints;

    public GameData()
    {
        Name = "Player";
        inventoryData = new Dictionary<string, int>();
        life = 3;
        strength = 1;
        defense = 1;
        luck = 1;
        statPoints = 0;
    }

}
