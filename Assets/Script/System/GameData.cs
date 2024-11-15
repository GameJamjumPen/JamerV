using System.Collections.Generic;

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



    public GameData()
    {
        Name = "Princess";
        inventoryData = new List<CardData>(){};
        loadoutData = new List<CardData>(){};
        life = 3;
        strength = 1;
        defense = 1;
        heal = 1;
        statPoints = 0;
        wentRoom = new List<int>(){};
        currentRoom = 0;
    }
}


    [System.Serializable]
    public class CardData
    {
        public string cardName;
        public string cardType;
        public float cardValue;
        public string cardDes;

        public CardData(CardSO card) //PolyMorph
    {
        if (card == null)
        {
            return;
        }

        cardName = card._cardName;
        cardValue = card._value;
        cardType = card.cardType.ToString();
        cardDes  = card.cardDes;
        
    }
}
