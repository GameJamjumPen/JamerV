using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public List<GameObject> gridParent;
    public List<GameObject> roomPref;
    public List<string> scenes;

    private List<GameObject> availableRooms;
    private List<Transform> allGridSlots;

    private void Awake()
    {
        availableRooms = new List<GameObject>(roomPref);
        allGridSlots = new List<Transform>();

        foreach (GameObject grid in gridParent)
        {
            foreach (Transform gridSlot in grid.transform)
            {
                allGridSlots.Add(gridSlot);
            }
        }
    }

    public void LoadData(GameData data)
    {
        if (data.roomPlacement == null || data.roomPlacement.Count == 0)
        {
            GenerateRandomLayout();
        }
        else
        {
            PlaceRooms(data.roomPlacement);
        }
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
    }


    private void PlaceRooms(List<RoomPlacement> layout = null)
    {
        if (layout != null)
        {
            for (int i = 0; i < layout.Count; i++)
            {
                int gridSlotIndex = layout[i].gridInd;
                int roomPrefabIndex = layout[i].roomInd;

                Transform slot = allGridSlots[gridSlotIndex];
                GameObject roomPrefab = roomPref[roomPrefabIndex];

                Instantiate(roomPrefab, slot);
            }
        }
        else
        {
            for (int i = 0; i < allGridSlots.Count; i++)
            {
                Instantiate(availableRooms[i], allGridSlots[i]);
            }
        }
    }


    public void GenerateRandomLayout()
    {
        GameObject startRoom = availableRooms[0];
        Transform firstSlot = allGridSlots[0];
        Instantiate(startRoom, firstSlot);

        allGridSlots.RemoveAt(0);
        availableRooms.RemoveAt(0);

        Shuffle(allGridSlots);
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


}