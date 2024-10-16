using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleInventory : MonoBehaviour, IInventorable
{
    public PlayerModel player;
    public List<CardSO> cardSOPools;
    public InventorySlot[] slotDisplay;
    public InventorySlot useSlot;
    public GameObject shuffleUI;
    public CardSO cardSelected;
    public BattleController battleController;
    private EnemyHolder[] enemyHolders;
    public EnemyHolder enemyHolder;
    private EnemyUIManager enemyUIManager;
    private PlayerUIManager playerUIManager;
    public void getCard(List<CardSO> cardSOList)
    {
        foreach (CardSO cardSO in cardSOList)
        {
            cardSOPools.Add(cardSO);
        }
    }
    public void Shuffle(List<CardSO> cardSOList)
    {
        int rand = Random.Range(0, cardSOList.Count);
        AddItem(cardSOList[rand]);
    }

    public void AddDeck(int deckLength)
    {
        for (int i = 0; i < deckLength; i++)
        {
            Shuffle(cardSOPools);
        }
    }

    public void Awake()
    {
        enemyHolders = FindObjectsOfType<EnemyHolder>();
        enemyUIManager = battleController.enemyUIManager;
        playerUIManager = battleController.playerUIManager;
        player = battleController.player;
        //getCard(Paper.Instance.cardSOs);
        AddDeck(3);
    }
    public void DeselectedAllHolder()
    {
        for (int i = 0; i < enemyHolders.Length; i++)
        {
            enemyHolders[i].Deselected();
        }
    }

    public void Update()
    {
        if (CheckEmpty(slotDisplay))
        {
            shuffleUI.SetActive(true);
        }
    }

    public void AddItem(CardSO _card)
    {
        for (int i = 0; i < slotDisplay.Length; i++)
        {
            if (!slotDisplay[i].isFull)
            {
                slotDisplay[i].AddItem(_card);
                return;
            }
        }
    }

    public bool CheckEmpty(InventorySlot[] slots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].isFull)
            {
                return false;
            }
        }
        return true;
    }

    public void DeselectedAllSlot()
    {
        foreach (InventorySlot inventorySlot in slotDisplay)
        {
            inventorySlot.OnDeselected();
        }
    }

    public void Use()
    {
        if (cardSelected.cardType == CardType.DEF)
        {
            player.setShield((int)cardSelected._value);
            battleController.ShowDamage((int)cardSelected._value, battleController.playerObject, battleController.TextPopup);
            this.playerUIManager.UpdatePlayerUI(battleController.player);
        }
        if (cardSelected.cardType == CardType.SUP)
        {
            player.HealByAmount((int)cardSelected._value);
            battleController.ShowDamage((int)cardSelected._value, battleController.playerObject, battleController.TextPopup);
            this.playerUIManager.UpdatePlayerUI(battleController.player);
        }
        if (cardSelected.cardType == CardType.ATK)
        {
            // Check if enemyHolder is null for ATK cards
            if (enemyHolder == null)
            {
                //Debug.Log("Selected Enemy");
                return; // Exit the function if no enemy is selected
            }

            
            CharacterBase.Attack((int)cardSelected._value, enemyHolder.enemyContain);
            battleController.ShowDamage((int)cardSelected._value , enemyHolder.gameObject , battleController.TextPopup);
            this.enemyUIManager.updateUI(battleController.enemies);
            enemyHolder.Deselected();
        }
        
        playerUIManager.AttackAnimate();
        // Reset cardSelected and useSlot after use
        useSlot.OnDeselected(); //unselected
        useSlot.RemoveItem(); //remove card from itself
        useSlot = null; //destroy itself
        battleController.isPlayerTurn = false;
        battleController.OnTurnChange(Turn.PlayerAnim);
        Debug.Log("Change Turn");
    }
}
