using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> gridParent;
    public List<GameObject> roomPref;
    

    void Start()
    {
        RandomRoomPlacement();
    }

    void RandomRoomPlacement()
    {
        List<GameObject> availableRooms = new List<GameObject>(roomPref);
        List<Transform> allGridSlots = new List<Transform>();


        foreach (GameObject grid in gridParent)
        {
            foreach (Transform gridSlot in grid.transform)
            {
                allGridSlots.Add(gridSlot);
            }
        }

        while (availableRooms.Count > 0 && allGridSlots.Count > 0)
        {
            int randomRoomIndex = Random.Range(0, availableRooms.Count);
            GameObject selectedRoom = availableRooms[randomRoomIndex];

            int randomSlotIndex = Random.Range(0, allGridSlots.Count);
            Transform selectedGridSlot = allGridSlots[randomSlotIndex];

            Instantiate(selectedRoom, selectedGridSlot);

            availableRooms.RemoveAt(randomRoomIndex);
            allGridSlots.RemoveAt(randomSlotIndex);
        }
    }

    

}