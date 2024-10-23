// using System.Collections.Generic;
// using NaughtyAttributes;
// using UnityEngine;

// public class RoomManager : MonoBehaviour
// {
//     [Header("Hover to see more information")]
//     [Tooltip("Room Prefab that have the size equal to RoomHolder size")]
//     public List<Room> roomsContainer;
//     [Tooltip("Each Room position in the Actual game")]
//     public Transform[] roomHolders;
//     [Tooltip("Actual room use for playing and rolling")]
//     public Room[] rooms; //actual room use for playing and rolling
//     [Tooltip("on index 0 is easy , 1 is medium , 2 is hard")]
//     [Scene]
//     public string[] Scenes;
    
//     /// <summary>
//     /// Generate Room when player enter the scene for the first time
//     /// </summary>
//     public void OnRoomGenerate(){
//         for(int i = 0;i<= roomHolders.Length;i++){
//             int rand = Random.Range(0,roomsContainer.Count);
//             Room instance = Instantiate(roomsContainer[rand] , roomHolders[i].transform.position , Quaternion.identity);
//             rooms[i] = instance;
//             roomsContainer.Remove(roomsContainer[rand]);
//         }
//     }
// }
