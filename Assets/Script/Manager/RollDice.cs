using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class RollDice : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public List<int> thiswentRoom;
    public Animator animator;
    public TMP_Text rolledTextUI;
    [SerializeField] private bool canRoll;
    private int rolledRoom;
    private bool isRolling; // New flag to track the dice rolling state

    private string currentstate;
    private static string HOVER = "OnMouseHover";
    private static string PRESS = "OnMousePress";
    private static string NORMAL = "Idle";
    private static string ROLL = "dice";

    private void Start()
    {
        canRoll = true;
        isRolling = false; // Initialize the rolling flag
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
        isRolling = false; // Reset the rolling flag when a new roll can happen
    }

    private void Move()
    {
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        playerMovement.PlayerMove(rolledRoom);
    }

    private void OnMouseDown()
    {
        if (canRoll)
        {
            ChangeAnimationState(PRESS , 0.00001f);
            ChangeAnimationState(ROLL, 0.00001f);
            SoundManager.Instance.PlaySFX("Dice");
            canRoll = false;
            isRolling = true; // Set the rolling flag when rolling starts
        }
    }

    public int Roll()
    {
        Dice dice = new Dice();
        thiswentRoom = GameManager.singleton.wentRoom;
        int rolledRooom = dice.MainRoll(thiswentRoom);
        return rolledRooom;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (canRoll && !isRolling) // Check if not rolling
        {
            ChangeAnimationState(HOVER);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isRolling) // Prevent exit state change if the dice is rolling
        {
            ChangeAnimationState(NORMAL);
        }
    }

    public void ChangeAnimationState(string state)
    {
        if (currentstate == state)
        {
            return;
        }
        currentstate = state;
        animator.CrossFadeInFixedTime(state, 0.1f);
        Debug.Log("Change state to " + state);
    }

    public void ChangeAnimationState(string state, float time)
    {
        if (currentstate == state)
        {
            return;
        }
        currentstate = state;
        animator.CrossFadeInFixedTime(state, time);
        Debug.Log("Change state to " + state);
    }
}