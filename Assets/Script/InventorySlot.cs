using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.VisualScripting;

public class InventorySlot : MonoBehaviour , IPointerClickHandler , IBeginDragHandler , IDragHandler , IEndDragHandler
{
    //ITEM DATA//
    public CardSO cardSO;
    public bool isFull;

    //ITEM SLOT//
    [SerializeField]
    private Image slotImage;
    public GameObject selectedShader;
    public bool isSelected;
    //[Header("Reference")]
    private Inventory inventory;

    [Header("Drag and Drop")]

    public Transform beforeDragPos;
    public InventorySlot inventorySlot;
    void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
    }
    #region Add/Remove Item
    public void AddItem(CardSO cardSO){
        slotImage.gameObject.SetActive(true);
        this.cardSO = cardSO;
        isFull = true;
        UpdateDisplay();
    }
    public void RemoveItem(){
        this.cardSO = null;
        isFull = false;
        UpdateDisplay();
        OnDeselected();
    }
    #endregion

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left){
            OnSelected();
        }
        if(eventData.button == PointerEventData.InputButton.Right){
            OnDeselected();
        }
        if(eventData.button == PointerEventData.InputButton.Middle){
            if(isSelected){
                RemoveItem();
            }
        }        
    }

    public void UpdateDisplay(){
        if(cardSO != null){
            slotImage.sprite = cardSO._cardSprite;
        }else{
            slotImage.gameObject.SetActive(false);
        }
    }
    #region Selected/Deselected Slot
    public void OnSelected(){
        inventory.DeselectedAllSlot();
        selectedShader.SetActive(true);
        isSelected = true;
        if(this.cardSO != null){
            inventory.cardSelected = this.cardSO;
            inventory.DisplaySelected();
        }
    }
    public void OnDeselected(){
        selectedShader.SetActive(false);
        isSelected = false;
    }
    #endregion

    public void OnBeginDrag(PointerEventData eventData)
    {
        // DraggableObject.gameObject.SetActive(true);
        inventorySlot = null;
        Debug.Log("Begin Drag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!isSelected){
            return;
        }else{
            slotImage.transform.position = Input.mousePosition;
        }
        // List to hold all raycast hits
        List<RaycastResult> results = new List<RaycastResult>();

        // Raycast for UI elements under the mouse
        EventSystem.current.RaycastAll(eventData, results);

        // Iterate through the results to find an InventorySlot component
        foreach (RaycastResult result in results)
        {
            // Try to get the InventorySlot component on the hit object;
            InventorySlot slot;
            if (result.gameObject.TryGetComponent<InventorySlot>(out slot))
            {
                // If the InventorySlot component is found, assign it and break
                inventorySlot = slot;
                Debug.Log("InventorySlot found: " + inventorySlot);
                break;
            }
        }
        if (inventorySlot == null)
        {
            Debug.Log("No InventorySlot found under mouse.");
        }
        //Debug.Log("Dragging");
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if(isSelected && cardSO != null){
            if(inventorySlot != null && !inventorySlot.isFull){
                inventorySlot.AddItem(cardSO);
                RemoveItem();
                Debug.Log("Added");
            }
        }
        Debug.Log("End Drag");
        slotImage.transform.position = beforeDragPos.transform.position;
    }
}
