using System.Collections.Generic;
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

    private void Start()
    {
        
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

    public void RoomEnter(int room)
    {

    }

}