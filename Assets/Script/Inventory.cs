using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    #region Declare Variable
    public KeyCode openCloseKey = KeyCode.Tab;
    public GameObject backPackSystem;
    public GameObject pocketSystem;

    [Header("Slots")]
    public CardSO cardSelected;
    public InventorySlot[] itemSlots;
    public InventorySlot[] actualSlots;
    public InventorySlot[] allSlots;

    [Header("Display")]
    public TextMeshProUGUI cardName;
    public Image cardImage;
    public Image cardType;
    public TextMeshProUGUI cardStat;
    [Tooltip("Types[0] will be ATK Types[1] will be DEF card")]
    public Sprite[] types;

    #endregion
    public void Update(){
        if(Input.GetKeyDown(openCloseKey)){
            if(backPackSystem.activeSelf){
                backPackSystem.SetActive(false);
            }else backPackSystem.SetActive(true);
        }
    }
    // public void Update(){
    //     if()
    // }
    public void AddItem(CardSO _card){
        for(int i = 0;i < itemSlots.Length;i++){
            if(!itemSlots[i].isFull){
                itemSlots[i].AddItem(_card);
                return;
            }
        }
    }
    #region SlotSelected
    public void DeselectedAllSlot(){
        foreach (InventorySlot slot in allSlots)
        {
            slot.OnDeselected();
        } 
    }

    public void DisplaySelected(){
        if(!cardName.gameObject.activeSelf) cardName.gameObject.SetActive(true);
        if(!cardImage.gameObject.activeSelf) cardImage.gameObject.SetActive(true);
        if(!cardType.gameObject.activeSelf) cardType.gameObject.SetActive(true);
        if(!cardStat.gameObject.activeSelf) cardStat.gameObject.SetActive(true);
        cardName.text = cardSelected._cardName;
        cardImage.sprite = cardSelected._cardSprite;
        cardStat.text = cardSelected._value.ToString();
        if(cardSelected.cardType == CardType.ATK){
            cardType.sprite = types[0];
        }else{
            cardType.sprite = types[1];
        }
    }
    public void DisplayDeselected(){
        cardName.gameObject.SetActive(false);
        cardImage.gameObject.SetActive(false);
        cardType.gameObject.SetActive(false);
        cardStat.gameObject.SetActive(false);
    }
    #endregion


}