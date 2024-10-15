using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInventory : MonoBehaviour ,IInventorable
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
    public void getCard(List<CardSO> cardSOList){
        foreach(CardSO cardSO in cardSOList){
            cardSOPools.Add(cardSO);
        }
    }
    public void Shuffle(List<CardSO> cardSOList){
        int rand = Random.Range(0,cardSOList.Count);
        AddItem(cardSOList[rand]);
    }

    public void AddDeck(int deckLength){
        for(int i = 0; i< deckLength;i++){
            Shuffle(cardSOPools);
        }
    }

    public void Awake()
    {
        enemyHolders = FindObjectsOfType<EnemyHolder>();
        this.enemyUIManager = battleController.enemyUIManager;
        this.playerUIManager = battleController.playerUIManager;
        getCard(Paper.Instance.cardSOs);
        AddDeck(3);
    }
    public void DeselectedAllHolder(){
        for(int i = 0;i< enemyHolders.Length;i++){
            enemyHolders[i].Deselected();
        }
    }

    public void Update(){
        if(CheckEmpty(slotDisplay)){
            shuffleUI.SetActive(true);
        }
    }

    public void AddItem(CardSO _card)
    {
        for(int i = 0;i < slotDisplay.Length;i++){
            if(!slotDisplay[i].isFull){
                slotDisplay[i].AddItem(_card);
                return;
            }
        }
    }

    public bool CheckEmpty(InventorySlot[] slots)
    {
        for(int i = 0;i< slots.Length;i++){
            if(slots[i].isFull){
                return false;
            }
        }
        return true;
    }

    public void DeselectedAllSlot()
    {
        foreach(InventorySlot inventorySlot in slotDisplay){
            inventorySlot.OnDeselected();
        }
    }

    public void Use(){
        if(cardSelected != null && enemyHolder != null){
            switch (cardSelected.cardType)
            {
                case CardType.ATK:
                CharacterBase.Attack((int)cardSelected._value , enemyHolder.enemyContain);
                this.enemyUIManager.updateUI(battleController.enemies);
                break;
                case CardType.DEF:
                player.setShield((int)cardSelected._value);
                this.playerUIManager.UpdatePlayerUI(battleController.player);
                break;
                case CardType.SUP:
                player.HealByAmount((int)cardSelected._value);
                this.playerUIManager.UpdatePlayerUI(battleController.player);
                break;
            }
            cardSelected = null;
            useSlot.RemoveItem();
            useSlot = null;
        }
    }
} 
