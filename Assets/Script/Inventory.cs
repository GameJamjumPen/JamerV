using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public CardSO[] cardSOs;
    public void AddItem(CardSO card){
        Debug.Log(card._cardName);
    }
}
