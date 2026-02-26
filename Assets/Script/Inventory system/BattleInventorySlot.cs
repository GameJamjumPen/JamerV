using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleInventorySlot : MonoBehaviour, IPointerEnterHandler, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerExitHandler, IPointerClickHandler
{
    //ITEM DATA//
    [Header("Data")]
    public CardSO cardSO;
    public bool hasCard;
    public bool isSelected;

    //ITEM SLOT//
    [Header("Item Slot")]
    [SerializeField] private Image slotImage;
    [SerializeField] private Image slotOutline;
    [SerializeField] private TextMeshProUGUI slotValueText;
    [SerializeField] private Image slotType;
    //[Header("Reference")]
    [Header("Reference")]
    [Tooltip("index 0 is ATK , 1 is DEF, 2 is HEAL")]
    public Sprite[] types;

    [Header("Drag and Drop")]
    private Canvas rootCanvas;
    private Transform originalItemParent;
    private Vector2 originalItemAnchoredPosition; // Local slot position for snap-back
    private static BattleInventorySlot draggingSlot; // Shared drag source so any slot can accept a drop
    public EnemyHolder enemyHolder;
    // Transform parentAfterDrag;
    // public Transform draggableItem;
    [Header("Animation")]
    [SerializeField] private Animator _animator;
    private string currentstate;
    private static string NORMAL = "Idle";
    private static string DRAG = "OnDragSlot";
    private static string HOVER = "OnMouseHighlighted";
    private static string SELECTED = "OnSelected";
    void Awake()
    {
        rootCanvas = GetComponentInParent<Canvas>();
        currentstate = NORMAL;
    }
    void Start()
    {
        UpdateDisplay();
    }
    #region Add/Remove Item
    public void AddItem(CardSO cardSO)
    {
        this.cardSO = cardSO;
        hasCard = true;
        UpdateDisplay();
    }
    public void RemoveItem()
    {
        this.cardSO = null;
        hasCard = false;
        UpdateDisplay();
        OnDeselected();
    }
    #endregion

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ToggleSelected();
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
        // Debug.Log($"[UpdateDisplay] hasCard: {hasCard}");

        if (slotImage == null || slotType == null || slotOutline == null)
        {
            Debug.LogError("[UpdateDisplay] Missing UI references! Check Inspector for slotImage, slotType, or slotOutline");
            return;
        }

        slotImage.gameObject.SetActive(hasCard);
        slotType.gameObject.SetActive(hasCard);
        slotOutline.gameObject.SetActive(isSelected);

        if (hasCard)
        {
            if (cardSO == null) return;

            if (slotValueText == null) return;

            if (types == null || types.Length == 0) return;

            if (BattleInventory.instance == null)
            {
                // Debug.LogError("[UpdateDisplay] BattleInventory.instance is null!");
                return;
            }

            slotImage.sprite = cardLoader.Instance.sprites[cardSO._cardName];

            // Consolidated switch for both damage calculation and type sprite
            float damage = cardSO._value;
            // Debug.Log($"[UpdateDisplay] Card: {cardSO._cardName}, Type: {cardSO.cardType}, BaseDamage: {damage}");

            switch (cardSO.cardType)
            {
                case CardType.ATK:
                case CardType.ATKV2:
                case CardType.ATKV3:
                    int atkValue = (int)(damage * (1 + (BattleInventory.instance.strength * 0.2)));
                    slotValueText.text = atkValue.ToString();
                    slotType.sprite = types[0];
                    // Debug.Log($"[UpdateDisplay] ATK Card - Strength: {BattleInventory.instance.strength}, Calculated Value: {atkValue}");
                    break;
                case CardType.DEF:
                    int defValue = (int)(damage * (1 + (BattleInventory.instance.defence * 0.2)));
                    slotValueText.text = defValue.ToString();
                    // Debug.Log($"[UpdateDisplay] DEF Card - Defence: {BattleInventory.instance.defence}, Calculated Value: {defValue}");
                    slotType.sprite = types[1];
                    break;
                case CardType.SUP:
                    int supValue = (int)(damage * (1 + (BattleInventory.instance.heals * 0.2)));
                    slotValueText.text = supValue.ToString();
                    // Debug.Log($"[UpdateDisplay] SUP Card - Heals: {BattleInventory.instance.heals}, Calculated Value: {supValue}");
                    slotType.sprite = types[2];
                    break;
            }
        }
    }

    #region Selected/Deselected Slot
    public void ToggleSelected()
    {
        if (isSelected)
        {
            OnDeselected();
            return;
        }
        OnSelected();
    }
    public void OnSelected()
    {
        BattleInventory.instance.DeselectedAllSlot();
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
        if (hasCard)
            ChangeAnimationState(SELECTED);
        BattleInventory.instance.cardSelected = this.cardSO;
        BattleInventory.instance.useSlot = this;
        UpdateDisplay();
    }
    public void OnDeselected()
    {
        isSelected = false;
        if (BattleInventory.instance != null)
        {
            BattleInventory.instance.cardSelected = null;
        }
        UpdateDisplay();
        if (hasCard)
            ChangeAnimationState(NORMAL);
    }
    #endregion
    #region Dragging
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isSelected) return;
        if (!hasCard) return;
        if (cardSO.cardType == CardType.ATK || cardSO.cardType == CardType.ATKV2 || cardSO.cardType == CardType.ATKV3)
        {
            draggingSlot = this;
            originalItemParent = slotImage.transform.parent;
            originalItemAnchoredPosition = slotImage.rectTransform.anchoredPosition;
            if (rootCanvas != null)
            {
                slotImage.transform.SetParent(rootCanvas.transform, true);
            }
        }
        enemyHolder = null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggingSlot != this || slotImage == null || rootCanvas == null) return;
        if (!isSelected) return;
        if (!hasCard) return;


        // can drag only attack type
        if (cardSO.cardType == CardType.ATK || cardSO.cardType == CardType.ATKV2 || cardSO.cardType == CardType.ATKV3)
        {

            ChangeAnimationState(DRAG);
            // Convert screen pointer to canvas local point and move the icon there
            RectTransform canvasRect = rootCanvas.transform as RectTransform;
            if (canvasRect != null)
            {
                Vector2 localPoint;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        canvasRect,
                        eventData.position,
                        eventData.pressEventCamera,
                        out localPoint))
                {
                    slotImage.rectTransform.localPosition = localPoint;
                }
            }

            // Use the generic method for EnemyHolder
            enemyHolder = GetComponentUnderMouse<EnemyHolder>(eventData);

            if (enemyHolder != null)
            {
                // Debug.Log("enemyHolder found: " + enemyHolder);
                if (cardSO.cardType != CardType.ATKV2)
                {
                    BattleInventory.instance.enemyHolder = enemyHolder;
                    BattleInventory.instance.enemyHolder.Selected();
                }
                else
                {
                    BattleInventory.instance.SelectedAllHolder();
                }
            }
            else
            {
                // Debug.Log("No enemyHolder found under mouse.");
                BattleInventory.instance.enemyHolder = null;
                BattleInventory.instance.DeselectedAllHolder();
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
        if (draggingSlot != this) return;
        if (!isSelected) return;
        if (!hasCard) return;

        if (cardSO.cardType == CardType.ATK || cardSO.cardType == CardType.ATKV2 || cardSO.cardType == CardType.ATKV3)
        {
            // If dropped on a valid enemy holder, use the card
            if (enemyHolder != null)
            {
                OnUse();
            }
            else
            {
                // Deselect all if not dropped on enemy
                BattleInventory.instance.DeselectedAllHolder();
            }

            // Restore card image to original parent and position
            if (originalItemParent != null && slotImage != null)
            {
                slotImage.transform.SetParent(originalItemParent, false);
                slotImage.rectTransform.anchoredPosition = Vector2.zero;
                Debug.Log("Reset inventory slot image position");
            }

            // Clear drag state
            enemyHolder = null;
            BattleInventory.instance.enemyHolder = null;
            draggingSlot = null;

            UpdateDisplay();
        }
    }
    #endregion
    #region Animation working perfectly fine don't touch
    public void ChangeAnimationState(string state)
    {
        if (currentstate == state)
        {
            return;
        }
        currentstate = state;
        _animator.CrossFadeInFixedTime(state, 0.1f);
        // Debug.Log("Change state to" + state);
    }
    public void ChangeAnimationState(string state, float time)
    {
        if (currentstate == state)
        {
            return;
        }
        currentstate = state;
        _animator.CrossFadeInFixedTime(state, time);
        // Debug.Log("Change state to" + state);
    }
    #endregion
    public void OnUse()
    {
        if (BattleInventory.instance.cardSelected == this.cardSO)
        {
            BattleInventory.instance.Use();
        }
        else
        {
            Debug.Log("BattleInventory.instance.cardSelected != this.cardSO");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hasCard)
            ChangeAnimationState(HOVER, 0.01f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!hasCard) return;

        if (!isSelected)
        {
            ChangeAnimationState(NORMAL, 0.01f);
        }
        else
        {
            ChangeAnimationState(SELECTED, 0.01f);
        }

    }
}