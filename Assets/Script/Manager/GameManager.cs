using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public List<string> scenes;
    public List<int> wentRoom { get; private set; }
    public int currentRoom;
    public List<GameObject> allrooms;
    [Header("Inventory Related")]
    public Room selectedRoom;
    public Inventory inventory;
    [Tooltip("Card to give to paper instance")]
    public CardSO[] cardSOs;
    public GameObject lockIn;
    public GameObject  bar;

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
    public void Debugger(string a)
    {
        Debug.Log(a);
    }


    public void LoadData(GameData data)
    {
        this.currentRoom = data.currentRoom;
        this.wentRoom = new List<int>(data.wentRoom);

        PlaceRooms(data.wentRoom);
        
    }

    public void SaveData(ref GameData data)
    {
        List<CardSO> slotToSave = new List<CardSO>();
        foreach(CardSO card in cardSOs)
        {
            if(card!=null){
            slotToSave.Add(card);}
        }

        data.loadoutData = DataPersistenceMNG.Instance.ConvertScriptableObjectsToData(slotToSave);

        data.currentRoom = this.currentRoom;
        data.wentRoom = new List<int>(this.wentRoom);
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

    public void GetRoom(int room)
    {
        wentRoom.Add(room);
    }



    public void RoomEnter(int room) //selectedRoomยังหาไม่ถูกห้อง ไม่รู้ทำไม
    {
        if (room == 0) { return; }
        room -= 1;
        Debug.Log("room" + room);
        selectedRoom = allrooms[room].GetComponentInChildren<Room>();
        if (inventory.CheckFull(inventory.actualSlots) && inventory.CheckType(inventory.actualSlots,true,true,false))
        {
            if (cardSOs == null || cardSOs.Length != inventory.actualSlots.Length)
            {
                cardSOs = new CardSO[inventory.actualSlots.Length];
            }

            // Assign cardSOs
            for (int i = 0; i < inventory.actualSlots.Length; i++)
            {
                if (inventory.actualSlots[i] != null)
                {
                    cardSOs[i] = inventory.actualSlots[i].cardSO;
                }
            }
            selectedRoom.OnPlayerAttack();
        }

        else if(selectedRoom.Treasure)
        {
            selectedRoom.OnPlayerAttack();
        }
        else
        { //no item in inventory slot
            if (!inventory.inv.activeSelf && !selectedRoom.Treasure)
            {
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
        if(!inventory.CheckType(inventory.actualSlots,true,true,false)){
            Debug.Log("make all various Type");
        }
        if (cardSOs == null || cardSOs.Length != inventory.actualSlots.Length)
        {
            cardSOs = new CardSO[inventory.actualSlots.Length];
        }

        // Assign cardSOs
        for (int i = 0; i < inventory.actualSlots.Length; i++)
        {
            if (inventory.actualSlots[i] != null)
            {
                cardSOs[i] = inventory.actualSlots[i].cardSO;
            }
        }
        selectedRoom.OnPlayerAttack();
    }

}