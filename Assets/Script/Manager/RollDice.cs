using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RollDice : MonoBehaviour
{
    public List<int> thiswentRoom;
    public Animator animator;
    public TMP_Text rolledTextUI;
    private bool canRoll;
    private int rolledRoom;
    private void Start()
    {
        canRoll=true;
    }
    public void MainDiceRoll()
    {
        //SceneChange.ChangeSceneFunc("TurnBaseCombat");
    }

    public void SetTextUI()
    {
        rolledRoom = Roll();
        rolledTextUI.text = rolledRoom.ToString();
    }

    public void SetRoll()
    {
        canRoll = true;
    }

    private void Move()
    {
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        playerMovement.PlayerMove(rolledRoom);
    }

    private void OnMouseDown() {
        if(canRoll){
        animator.SetTrigger("roll");
        SoundManager.Instance.PlaySFX("Dice");
        canRoll=false;}
    }

    public int Roll()
    {
        Dice dice = new Dice();
        thiswentRoom = GameManager.singleton.wentRoom;
        int rolledRooom = dice.MainRoll(thiswentRoom);
        GameManager.singleton.GetRoom(rolledRooom); //add to went room in gmng

        return rolledRooom;
    }

}
