using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GameManager : MonoBehaviour, IDataPersistence
{
    [Header("tutorials")]
    public GameObject tutorial;
    public GameObject cardTutorial;

    public Collider2D dice;
    public List<string> scenes;
    public List<int> wentRoom { get; private set; }
    public int currentRoom;
    public List<GameObject> allrooms;
    [Header("Inventory Related")]
    public Room selectedRoom;
    public Inventory inventory;
    public GameObject WarningButton;
    [Tooltip("Card to give to paper instance")]
    public CardSO[] cardSOs;
    public GameObject lockIn;
    public GameObject bar;

    private int roomget;

    public int stat;


    public static GameManager singleton { get; private set; }

    private void Awake()
    {
        if (singleton != null)
        {
            Destroy(gameObject);
            return;
        }
        singleton = this;
    }

    private void Start()
    {
        if(!PlayerPrefs.HasKey(tutorial.gameObject.name))
        {
            tutorial.SetActive(true);
        }
    }

    public void DiceAllow()
    {
        dice.enabled = true;
    }

    public void LoadData(GameData data)
    {
        this.currentRoom = data.currentRoom;
        this.wentRoom = Paper.Instance.wentRoom;
        
        PlaceRooms(data.wentRoom);

    }

    public void SaveData(ref GameData data)
    {
        List<CardSO> slotToSave = new List<CardSO>();
        foreach (CardSO card in cardSOs)
        {
            if (card != null)
            {
                slotToSave.Add(card);
            }
        }

        data.loadoutData = DataPersistenceMNG.Instance.ConvertScriptableObjectsToData(slotToSave);

        data.currentRoom = this.currentRoom;
    }

    public void Place()
    {
        PlaceRooms(wentRoom);
    }


    private void PlaceRooms(List<int> wentRoom = null)
    {
        if (wentRoom != null)
        {
            for (int i = 0; i < allrooms.Count; i++)
            {
                if (wentRoom.Contains(i))
                {
                    GameObject r = allrooms[i].transform.GetChild(0).gameObject;

                    SpriteRenderer roomSprite = r.GetComponent<SpriteRenderer>();
                    roomSprite.color = new Color(.5f, .5f, .5f);
                }
            }
        }
    }


    public void RoomEnter(int room) //selectedRoomยังหาไม่ถูกห้อง ไม่รู้ทำไม
    {
        roomget = room;
        Debug.Log("room" + room);
        selectedRoom = allrooms[room].GetComponentInChildren<Room>();
        if (inventory.CheckType(inventory.actualSlots, true, true, false))
        {
            GetPureCardSOfromArr();
            Paper.Instance.roomNum = room;

            selectedRoom.OnPlayerAttack();
        }

        else if (selectedRoom.Treasure)
        {
            Paper.Instance.roomNum = room;
            selectedRoom.OnPlayerAttack();

        }
        else
        { //no item in inventory slot
            if (!inventory.inv.activeSelf && !selectedRoom.Treasure)
            {
                if(!PlayerPrefs.HasKey(cardTutorial.gameObject.name))
                {
                    cardTutorial.SetActive(true);
                }
                
                inventory.inv.SetActive(true);
                bar.SetActive(true);
                lockIn.SetActive(true);
                inventory.istoggleable = false;
            }
            // if (!inventory.inv.activeSelf)
            // {
            //     inventory.inv.SetActive(true);
            // }


        }
    }

    public void Lock()
    {
        if (!inventory.CheckFull(inventory.actualSlots))
        {
            Debug.Log("make all not null");
            return;
        }

        if (!inventory.CheckType(inventory.actualSlots, true, true, false))
        {
            
            Debug.Log("make all various Type");
            WarningButton.SetActive(true);
            return;
        }
        GetPureCardSOfromArr();
        Paper.Instance.roomNum = roomget;
        selectedRoom.OnPlayerAttack();
    }

    public void GetPureCardSOfromArr()
    {
        if (cardSOs == null || cardSOs.Length != inventory.actualSlots.Length)
        {
            cardSOs = new CardSO[inventory.actualSlots.Length]; // init cardSO arr
        }

        // Assign cardSOs
        for (int i = 0; i < inventory.actualSlots.Length; i++)
        {
            if (inventory.actualSlots[i] != null)
            {
                cardSOs[i] = inventory.actualSlots[i].cardSO;
            }
        }
        // Filter out null values from cardSOs array
        List<CardSO> validCardSOs = new List<CardSO>();

        for (int i = 0; i < cardSOs.Length; i++)
        {
            if (cardSOs[i] != null)
            {
                validCardSOs.Add(cardSOs[i]);
            }
        }

        // Reassign the array with valid values
        cardSOs = validCardSOs.ToArray();

    }

}