using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using Unity.VisualScripting;
using System.Xml.Serialization;

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
    //[Header("Reference")]
    [Header("Reference")]
    private Inventory inventory;
    [Tooltip("index 0 is ATK , 1 is DEF")]
    public Sprite[] types;

    [Header("Drag and Drop")]

    public Transform beforeDragPos;
    public InventorySlot inventorySlot;
    //public Animator animator;
    public string currentstate;
    public static string NORMAL = "Idle";
    public static string HOVER = "OnDragSlot";
    public static string SELECTED = "OnSelected";
    public static string BARSELECTED = "OnSelectedBar";
    void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
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
            slotValueText.text = cardSO._value.ToString();
            switch (cardSO.cardType)
            {
                case CardType.ATK:
                slotType.sprite = types[0];
                break;
                case CardType.DEF:
                slotType.sprite = types[1];
                break;
            }
        }else{
            slotImage.gameObject.SetActive(false);
            slotType.gameObject.SetActive(false);

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
        }else{
            inventory.DisplayDeselected();
        }
    }
    public void OnDeselected(){
        selectedShader.SetActive(false);
        isSelected = false;
        inventory.DisplayDeselected();
    }
    #endregion
    #region Dragging
    public void OnBeginDrag(PointerEventData eventData)
    {
        // DraggableObject.gameObject.SetActive(true);
        inventorySlot = null;
        //ChangeAnimationState(HOVER  , slotImage.GetComponent<Animator>());
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
        //ChangeAnimationState(NORMAL  , slotImage.GetComponent<Animator>());
        slotImage.transform.position = beforeDragPos.transform.position;
    }
    #endregion

    public void ChangeAnimationState(string state , Animator _animator){
        if(currentstate == state){
            return;
        }
        state = currentstate;
        _animator.CrossFadeInFixedTime(state , 0.5f);
    }
}
