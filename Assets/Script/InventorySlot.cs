using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    //ITEM DATA//
    public CardSO cardSO;
    public bool _isFull;

    //ITEM SLOT//
    [SerializeField]
    private Image slotImage;

    public void AddItem(CardSO cardSO){
        this.cardSO = cardSO;
        _isFull = true;
    }
    public void UpdateDisplay(){
        slotImage.sprite = cardSO._cardSprite;
    }
}
