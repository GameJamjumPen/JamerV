using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour , IPointerClickHandler , IBeginDragHandler , IDragHandler , IEndDragHandler
{
    //ITEM DATA//
    public CardSO cardSO;
    public bool isFull;

    //ITEM SLOT//
    [Header("Item Slot")]
    [SerializeField]
    private Image slotImage;
    [SerializeField]private TextMeshProUGUI slotValueText;
    [SerializeField]private Image slotType;
    public GameObject selectedShader;
    public bool isSelected;
    public Transform _transform;
    //[Header("Reference")]
    [Header("Reference")]
    private Inventory inventory;
    public BattleInventory battleInventory;
    [Tooltip("index 0 is ATK , 1 is DEF, 2 is HEAL")]
    public Sprite[] types;

    [Header("Drag and Drop")]

    public Transform beforeDragPos;
    public InventorySlot inventorySlot;
    Transform parentAfterDrag;
    public Transform draggableItem;
    [Header("Animation")]
    [SerializeField]private Animator _animator;
    [SerializeField]private bool isBar;
    private string currentstate;
    private static string NORMAL = "Idle";
    private static string HOVER = "OnDragSlot";
    private static string SELECTED = "OnSelected";
    private static string BARSELECTED = "OnSelectedBar";
    void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
        battleInventory = FindObjectOfType<BattleInventory>();
        currentstate = NORMAL;
    }
    #region Add/Remove Item
    public void AddItem(CardSO cardSO){
        slotImage.gameObject.SetActive(true);
        slotType.gameObject.SetActive(true);
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
            if(isSelected){
            OnUse();
            }
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
            slotValueText.text = cardSO._value.ToString();
            switch (cardSO.cardType)
            {
                case CardType.ATK:
                slotType.sprite = types[0];
                break;
                case CardType.DEF:
                slotType.sprite = types[1];
                break;
                case CardType.SUP:
                slotType.sprite = types[2];
                break;
            }
        }else{
            slotImage.gameObject.SetActive(false);
            slotType.gameObject.SetActive(false);

        }
    }
    #region Selected/Deselected Slot
    public void OnSelected(){
        if(inventory != null){
            inventory.DeselectedAllSlot();
        }
        if(battleInventory != null){
            battleInventory.DeselectedAllSlot();
        }
        selectedShader.SetActive(true);
        isSelected = true;
        if(isBar){
            ChangeAnimationState(BARSELECTED);
        }else{
            ChangeAnimationState(SELECTED);
        }
        if(inventory != null){
            if(this.cardSO != null){
                inventory.cardSelected = this.cardSO;
                inventory.DisplaySelected();
            }else{
                inventory.DisplayDeselected();
            }
        }
        if(battleInventory != null){
            if(this.cardSO != null){
                battleInventory.cardSelected = this.cardSO;
                battleInventory.useSlot = this;
            }else{
                Debug.Log("this.cardSO = null");
            }
        }
    }
    public void OnDeselected(){
        selectedShader.SetActive(false);
        isSelected = false;
        if(inventory != null){
        inventory.DisplayDeselected();
        }
        if(battleInventory != null){
            battleInventory.cardSelected = null;
        }
        ChangeAnimationState(NORMAL);
    }
    #endregion
    #region Dragging
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(inventory!= null){
        if(!isSelected) return;
        else{
            inventorySlot = null;
            parentAfterDrag = transform;
            draggableItem.SetParent(transform.root);
            draggableItem.SetAsLastSibling();
        }
        // DraggableObject.gameObject.SetActive(true);
        
        
        //ChangeAnimationState(HOVER  , slotImage.GetComponent<Animator>());
        Debug.Log("Begin Drag");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {   
        if(inventory != null){

        if(!isSelected){
            return;
        }else{
            ChangeAnimationState(HOVER);
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
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if(inventory != null){

        if(!isSelected){
            return;
        }
        if(isSelected && cardSO != null){
            if(inventorySlot != null && !inventorySlot.isFull){
                inventorySlot.AddItem(cardSO);
                RemoveItem();
                ChangeAnimationState(NORMAL);
                Debug.Log("Added");
            }
        }
        Debug.Log("End Drag");
        //ChangeAnimationState(NORMAL  , slotImage.GetComponent<Animator>());
        draggableItem.SetParent(parentAfterDrag);
        draggableItem.transform.position = beforeDragPos.transform.position;
        slotImage.transform.position = draggableItem.transform.position;
        }
    }
    #endregion

    public void ChangeAnimationState(string state){
        if(currentstate == state){
            return;
        }
        currentstate = state;
        _animator.CrossFadeInFixedTime(state , 0.1f);
        Debug.Log("Change state to" + state);
    }
    public void OnUse(){
        if(battleInventory != null){
            if(battleInventory.cardSelected == this.cardSO){
                battleInventory.Use();
            }else{
                Debug.Log("battleInventory.cardSelected != this.cardSO");
            }
        }else{
            Debug.Log("battleInventory = null");
        }
    }
}