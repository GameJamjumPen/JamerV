using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System.Threading.Tasks;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    //ITEM DATA//
    [Header("Data")]
    public CardSO cardSO;
    public bool isFull;

    //ITEM SLOT//
    [Header("Item Slot")]
    [SerializeField]
    private Image slotImage;
    [SerializeField] private Image slotImageOutline;
    [SerializeField] private Image battleImage;
    [SerializeField] private Image bIOutline;
    [SerializeField] private TextMeshProUGUI slotValueText;
    [SerializeField] private Image slotType;
    public GameObject selectedShader;
    public bool isSelected;
    public Transform slotTypePos;
    //[Header("Reference")]
    [Header("Reference")]
    private Inventory inventory;
    public BattleInventory battleInventory;
    [Tooltip("index 0 is ATK , 1 is DEF, 2 is HEAL")]
    public Sprite[] types;

    [Header("Drag and Drop")]

    public Transform beforeDragPos;
    public InventorySlot inventorySlot;
    public EnemyHolder enemyHolder;
    Transform parentAfterDrag;
    public Transform draggableItem;
    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [SerializeField] private bool isBar;
    private string currentstate;
    private static string NORMAL = "Idle";
    private static string DRAG = "OnDragSlot";
    private static string HOVER = "OnMouseHighlighted";
    private static string SELECTED = "OnSelected";
    private static string BARSELECTED = "OnSelectedBar";
    void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
        battleInventory = FindObjectOfType<BattleInventory>();
        currentstate = NORMAL;
        if (battleInventory != null)
        {
            slotType.transform.position = slotTypePos.position;
        }
    }
    void Start()
    {
        UpdateDisplay();
    }
    #region Add/Remove Item
    public void AddItem(CardSO cardSO)
    {
        slotType.gameObject.SetActive(true);
        this.cardSO = cardSO;
        isFull = true;
        UpdateDisplay();
    }
    public void RemoveItem()
    {
        this.cardSO = null;
        isFull = false;
        UpdateDisplay();
        OnDeselected();
    }
    #endregion

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnSelected();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isSelected)
            {
                OnUse();
            }
        }
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            if (isSelected)
            {
                RemoveItem();
            }
        }
    }

    public void UpdateDisplay()
    {
        if (cardSO != null)
        {
            if (inventory != null)
            {
                slotImage.sprite = cardLoader.Instance.sprites[cardSO._cardName];
                slotImage.gameObject.SetActive(true);
                slotImageOutline.gameObject.SetActive(true);
                slotValueText.text = cardSO._value.ToString();
            }
            if (battleInventory != null)
            {
                battleImage.sprite = cardLoader.Instance.sprites[cardSO._cardName];
                battleImage.gameObject.SetActive(true);
                bIOutline.gameObject.SetActive(true);
                float damage = cardSO._value;
                switch (cardSO.cardType)
                {
                    case CardType.ATK:
                        slotValueText.text = ((int)(damage * (1 + (battleInventory.strength * 0.2)))).ToString();
                        break;
                    case CardType.ATKV2:
                        slotValueText.text = ((int)(damage * (1 + (battleInventory.strength * 0.2)))).ToString();
                        break;
                    case CardType.ATKV3:
                        slotValueText.text = ((int)(damage * (1 + (battleInventory.strength * 0.2)))).ToString();
                        break;
                    case CardType.DEF:
                        slotValueText.text = ((int)(damage * (1 + (battleInventory.defence * 0.2)))).ToString();
                        break;
                    case CardType.SUP:
                        slotValueText.text = ((int)(damage * (1 + (battleInventory.heals * 0.2)))).ToString();
                        break;
                }
            }
            switch (cardSO.cardType)
            {
                case CardType.ATK:
                    slotType.sprite = types[0];
                    break;
                case CardType.ATKV2:
                    slotType.sprite = types[0];
                    break;
                case CardType.ATKV3:
                    slotType.sprite = types[0];
                    break;
                case CardType.DEF:
                    slotType.sprite = types[1];
                    break;
                case CardType.SUP:
                    slotType.sprite = types[2];
                    break;
            }
        }
        else
        {
            slotImage.gameObject.SetActive(false);
            slotType.gameObject.SetActive(false);
            slotImageOutline.gameObject.SetActive(false);
            battleImage.gameObject.SetActive(false);
            bIOutline.gameObject.SetActive(false);
        }
    }
    #region Selected/Deselected Slot
    public void OnSelected()
    {
        if (inventory != null)
        {
            inventory.DeselectedAllSlot();
        }
        if (battleInventory != null)
        {
            battleInventory.DeselectedAllSlot();
        }
        selectedShader.SetActive(true);
        isSelected = true;
        string sound = " ";
        int rand = Random.Range(0, 3);
        switch (rand)
        {
            case 0:
                sound = "PickCard1";
                break;
            case 1:
                sound = "PickCard2";
                break;
            case 2:
                sound = "PickCard3";
                break;
        }
        SoundManager.Instance.PlaySFX(sound);
        if (isBar)
        {
            ChangeAnimationState(BARSELECTED);
        }
        else
        {
            ChangeAnimationState(SELECTED);
        }
        if (inventory != null)
        {
            if (this.cardSO != null)
            {
                inventory.cardSelected = this.cardSO;
                inventory.DisplaySelected();
            }
            else
            {
                inventory.DisplayDeselected();
            }
        }
        if (battleInventory != null)
        {
            if (this.cardSO != null)
            {
                battleInventory.cardSelected = this.cardSO;
                battleInventory.useSlot = this;
            }
            else
            {
                Debug.Log("this.cardSO = null");
            }
        }
    }
    public void OnDeselected()
    {
        selectedShader.SetActive(false);
        isSelected = false;
        if (inventory != null)
        {
            inventory.DisplayDeselected();
        }
        if (battleInventory != null)
        {
            battleInventory.cardSelected = null;
        }
        ChangeAnimationState(NORMAL);
    }
    #endregion
    #region Dragging
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (inventory != null)
        {
            if (!isSelected) return;
            else
            {
                inventorySlot = null;
                parentAfterDrag = transform;
                draggableItem.SetParent(transform.root);
                draggableItem.SetAsLastSibling();
                slotImageOutline.gameObject.SetActive(false);
            }
            // DraggableObject.gameObject.SetActive(true);


            //ChangeAnimationState(DRAG  , slotImage.GetComponent<Animator>());
            Debug.Log("Begin Drag");
        }
        if (battleInventory != null)
        {
            if (!isSelected) return;
            else
            {
                if (cardSO != null)
                {
                    if (cardSO.cardType == CardType.ATK || cardSO.cardType == CardType.ATKV2|| cardSO.cardType == CardType.ATKV3)
                    {
                        parentAfterDrag = transform;
                        draggableItem.SetParent(transform.root);
                        draggableItem.SetAsLastSibling();
                        bIOutline.gameObject.SetActive(false);
                    }
                }
                enemyHolder = null;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (inventory != null)
        {
            if (!isSelected)
            {
                return;
            }
            else
            {
                ChangeAnimationState(DRAG);
                slotImage.transform.position = Input.mousePosition;
            }

            // Use the generic method for InventorySlot
            inventorySlot = GetComponentUnderMouse<InventorySlot>(eventData);
            if (inventorySlot != null)
            {
                Debug.Log("InventorySlot found: " + inventorySlot);
            }
            else
            {
                Debug.Log("No InventorySlot found under mouse.");
            }
        }
        if (battleInventory != null && cardSO != null)
        {
            if (cardSO.cardType == CardType.ATK || cardSO.cardType == CardType.ATKV2 || cardSO.cardType == CardType.ATKV3)
            {
                if (!isSelected)
                {
                    return;
                }
                else
                {
                    ChangeAnimationState(DRAG);
                    battleImage.transform.position = Input.mousePosition;
                }

                // Use the generic method for EnemyHolder
                enemyHolder = GetComponentUnderMouse<EnemyHolder>(eventData);

                if (enemyHolder != null)
                {
                    Debug.Log("enemyHolder found: " + enemyHolder);
                    if(cardSO.cardType != CardType.ATKV2){                        
                    battleInventory.enemyHolder = enemyHolder;
                    battleInventory.enemyHolder.Selected();
                    }else{
                        battleInventory.SelectedAllHolder();
                    }
                }
                else
                {
                    Debug.Log("No enemyHolder found under mouse.");
                    battleInventory.enemyHolder = null;
                    battleInventory.DeselectedAllHolder();
                }
            }
        }
    }

    // Add this generic method to your class
    private T GetComponentUnderMouse<T>(PointerEventData eventData) where T : Component
    {
        List<RaycastResult> results = new List<RaycastResult>();

        // Raycast for UI elements under the mouse
        EventSystem.current.RaycastAll(eventData, results);

        // Iterate through the results to find the component of type T
        foreach (RaycastResult result in results)
        {
            T component = result.gameObject.GetComponent<T>();
            if (component != null)
            {
                return component;
            }
        }
        return null;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (inventory != null)
        {

            if (!isSelected)
            {
                return;
            }
            if (isSelected && cardSO != null)
            {
                if (inventorySlot != null && !inventorySlot.isFull)
                {
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
            UpdateDisplay();
        }
        if (battleInventory != null)
        {
            if (!isSelected) return;
            if (isSelected && cardSO != null)
            {
                if (cardSO.cardType == CardType.ATK || cardSO.cardType == CardType.ATKV2|| cardSO.cardType == CardType.ATKV3)
                {
                    if (enemyHolder != null)
                    {
                        // battleInventory.enemyHolder = enemyHolder;
                        bIOutline.gameObject.SetActive(true);
                        OnUse();
                        battleInventory.enemyHolder = null;
                    }
                    else
                    {
                        bIOutline.gameObject.SetActive(true);
                    }
                    draggableItem.SetParent(parentAfterDrag);
                    draggableItem.transform.position = beforeDragPos.transform.position;
                    battleImage.transform.position = draggableItem.transform.position;
                }
            }

        }
    }
    #endregion
    #region Animation
    public void ChangeAnimationState(string state)
    {
        if (currentstate == state)
        {
            return;
        }
        currentstate = state;
        _animator.CrossFadeInFixedTime(state, 0.1f);
        Debug.Log("Change state to" + state);
    }
    public void ChangeAnimationState(string state, float time)
    {
        if (currentstate == state)
        {
            return;
        }
        currentstate = state;
        _animator.CrossFadeInFixedTime(state, time);
        Debug.Log("Change state to" + state);
    }
    #endregion
    public void OnUse()
    {
        if (battleInventory != null)
        {
            if (battleInventory.cardSelected == this.cardSO)
            {
                battleInventory.Use();
            }
            else
            {
                Debug.Log("battleInventory.cardSelected != this.cardSO");
            }
        }
        else
        {
            Debug.Log("battleInventory = null");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ChangeAnimationState(HOVER, 0.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ChangeAnimationState(NORMAL, 0.5f);
    }
}