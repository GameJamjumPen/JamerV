using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyHolder : MonoBehaviour ,IPointerClickHandler
{
    public EnemyModel enemyContain;
    public bool isSelected;
    public BattleInventory battleInventory;
    public GameObject selectedShader;

    public void Awake(){
        battleInventory = FindObjectOfType<BattleInventory>();
    }
    public void Deselected(){
        isSelected = false;
        DisplaySelected();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left){
            Selected();
        }
    }

    public void Selected(){
        battleInventory.DeselectedAllHolder();
        isSelected = true;
        battleInventory.enemyHolder = this;
        DisplaySelected();
        Debug.Log($"Selected with {enemyContain.Name}");
    }
    public void DisplaySelected(){
        if(isSelected){
            selectedShader.SetActive(true);
        }
        else{
            selectedShader.SetActive(false);
        }
    }
}
