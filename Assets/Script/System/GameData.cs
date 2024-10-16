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
    public List<CardData> inventoryData;
    public List<CardData> loadoutData;
    public int life;
    public int strength;
    public int defense;
    public int heal;
    public int statPoints;
    public int currentRoom;
    public List<RoomPlacement> roomPlacement;


    public GameData()
    {
        Name = "Player";
        inventoryData = new List<CardData>(){};
        loadoutData = new List<CardData>(){};
        life = 3;
        strength = 1;
        defense = 1;
        heal = 1;
        statPoints = 0;
        wentRoom = new List<int>(){};
        roomPlacement = new List<RoomPlacement>();
        currentRoom = 0;
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

    [System.Serializable]
    public class CardData
    {
        public string cardName;
        public string cardType;
        public float cardValue;
        public string sprite;

        public CardData(CardSO card)
    {
        if (card == null)
        {
            return;
        }

        cardName = card._cardName;
        cardValue = card._value;
        sprite = UnityEditor.AssetDatabase.GetAssetPath(card._cardSprite);
        cardType = card.cardType.ToString();
    }
    }
