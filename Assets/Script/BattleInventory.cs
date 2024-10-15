using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleInventory : MonoBehaviour ,IInventorable
{
    public List<CardSO> cardSOPools;
    public InventorySlot[] slotDisplay;
    public GameObject shuffleUI;
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
        getCard(Paper.Instance.cardSOs);
        AddDeck(3);
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
}
