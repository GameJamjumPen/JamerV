using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollDice : MonoBehaviour
{
    public List<int> thiswentRoom;

    public void MainDiceRoll()
    {
        int rolledRoom = Roll();
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        playerMovement.PlayerMove(rolledRoom);
        //SceneChange.ChangeSceneFunc("TurnBaseCombat");
    }

    public int Roll()
    {
        Dice dice = new Dice();
        thiswentRoom = GameManager.singleton.wentRoom;
        int rolledRooom = dice.MainRoll(thiswentRoom);
        GameManager.singleton.GetRoom(rolledRooom);

        return rolledRooom;
    }

}
