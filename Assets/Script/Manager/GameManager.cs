using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEditor.SearchService;
using UnityEngine;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public List<GameObject> roomPref;
    public List<string> scenes;
    private List<int> wentRoom;
    public int currentRoom;
    public GameObject went;

    private List<GameObject> availableRooms;
    public List<GameObject> allrooms;
    [Header("Inventory Related")]
    public Room selectedRoom;
    public Inventory inventory;
    public CardSO[] cardSOs;
    public GameObject lockIn;

    public static GameManager singleton{get; private set;}

    private void Awake()
    {
        if(singleton != null)
        {
            Destroy(gameObject);
            return;
        }
        singleton = this;

        availableRooms = new List<GameObject>(roomPref);
    }
    public void Debugger(string a){
        Debug.Log(a);
    }

    public void LoadData(GameData data)
    {
        this.currentRoom = data.currentRoom;
        if (data.roomPlacement == null || data.roomPlacement.Count == 0)
        {
            GenerateRandomLayout();
        }
        else
        {
            wentRoom = new List<int>(data.wentRoom);
            PlaceRooms(data.roomPlacement);
        }
        transform.position = allrooms[currentRoom].transform.position;
    }

    public void SaveData(ref GameData data)
    {
        data.roomPlacement.Clear();

        RoomPlacement startRoomPlacement = new RoomPlacement(0, 0);
        data.roomPlacement.Add(startRoomPlacement);

        for (int i = 1; i < availableRooms.Count; i++)
        {
            RoomPlacement placementData = new RoomPlacement(i, roomPref.IndexOf(availableRooms[i]));
            data.roomPlacement.Add(placementData);
        }

        data.currentRoom = this.currentRoom;
    }


    private void PlaceRooms(List<RoomPlacement> layout = null)
    {
        if (layout != null)
        {
            for (int i = 0; i < layout.Count; i++)
            {   
                int gridSlotIndex = layout[i].gridInd;
                int roomPrefabIndex = layout[i].roomInd;
                
                Transform roomSlot = allrooms[gridSlotIndex].transform;
                GameObject roomPrefab = roomPref[roomPrefabIndex];

                if (wentRoom.Contains(i))
                {
                    Instantiate(went, roomSlot);
                }
                else {
                    Instantiate(roomPrefab, roomSlot);

                }
            }
            
        }
        else
        {
            for (int i = 0; i < availableRooms.Count; i++)
            {
                Transform slotTransform = allrooms[i+1].transform;
                Instantiate(availableRooms[i], slotTransform);
            }
        }
    }


    public void GenerateRandomLayout()
    {
        GameObject startRoom = availableRooms[0];
        Transform firstSlot = allrooms[0].transform;
        Instantiate(startRoom, firstSlot);

        availableRooms.RemoveAt(0);

        Shuffle(availableRooms);

        PlaceRooms();
    }


    void Shuffle<T>(List<T> arr)
    {
        for (int i = 0; i < arr.Count; i++)
        {
            int rdIndex = Random.Range(i, arr.Count);
            T temp = arr[i];
            arr[i] = arr[rdIndex];
            arr[rdIndex] = temp;
        }
    }

    public void RoomEnter(int room) //selectedRoomยังหาไม่เจอ
    {
        selectedRoom = allrooms[room].GetComponent<Room>();
        if(inventory.CheckFull(inventory.actualSlots)){
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
        }else{
            if(!inventory.pocketSystem.activeSelf){
                inventory.pocketSystem.SetActive(true);
            }
            if(!inventory.backPackSystem.activeSelf){
                inventory.backPackSystem.SetActive(true);
            }
            inventory.istoggleable = false;
            lockIn.SetActive(true);
        }
    }

    public void Lock(){
        if(!inventory.CheckFull(inventory.actualSlots)){
            Debug.Log("make all not null");
            return;
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