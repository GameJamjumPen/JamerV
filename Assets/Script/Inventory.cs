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

    [Header("Display")]
    public TextMeshProUGUI cardName;
    public Image cardImage;
    public Image cardType;
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
        foreach (InventorySlot slot in itemSlots)
        {
            slot.OnDeselected();
        } 
    }

    public void DisplaySelected(){
        cardName.text = cardSelected._cardName;
        cardImage.sprite = cardSelected._cardSprite;
        if(cardSelected.cardType == CardType.ATK){
            cardType.sprite = types[0];
        }else{
            cardType.sprite = types[1];
        }
    }
    #endregion


}
