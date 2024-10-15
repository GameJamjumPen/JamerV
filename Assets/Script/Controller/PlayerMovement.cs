using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerMovement : MonoBehaviour
{
    private List<GameObject> rooms;
    private int currentRoom;
    public int nextRoom;
    public Vector3 destination;
    [SerializeField] private float moveSpeed;
    public static event Action<int> SceneEnter;


    private void Start()
    {
        rooms = GameManager.singleton.allrooms;
    }

    public void Update()
    {

    }

    public void PlayerMove(int pos)
    {
        currentRoom = GameManager.singleton.currentRoom;
        StartCoroutine(Move(pos));
    }

    IEnumerator Move(int roomtomove)
    {
        while(currentRoom!=roomtomove){

        int nextRoom = currentRoom+1;
        if(nextRoom == 20){nextRoom = 0;}
        Vector3 nextpos = rooms[nextRoom].transform.position;
        Debug.Log(nextRoom);
        yield return StartCoroutine(MoveToNextRoom(nextpos));
        currentRoom = nextRoom;
        }

        Debug.Log("finished");
        GameManager.singleton.currentRoom = roomtomove;
        SceneEnter?.Invoke(roomtomove);
    }

    IEnumerator MoveToNextRoom(Vector3 targetPosition)
    {
        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
    
}