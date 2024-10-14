using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> gridParent;
    public List<GameObject> roomPref;
    public GameObject startRoom;
    public List<string> scenes;

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

        Transform firstRoom = allGridSlots[0];
        Instantiate(startRoom, firstRoom);
        allGridSlots.RemoveAt(0);

        Shuffle(allGridSlots);
        Shuffle(availableRooms);

        for(int i=0; i<allGridSlots.Count;i++)
        {
            Instantiate(availableRooms[i],allGridSlots[i]);
        }
    }

    void Shuffle<T>(List<T> arr)
    {
        for(int i=0; i<arr.Count; i++)
        {
            int rdIndex = Random.Range(i,arr.Count);
            T temp = arr[i];
            arr[i] = arr[rdIndex];
            arr[rdIndex] = temp;
        }
    }
    

}