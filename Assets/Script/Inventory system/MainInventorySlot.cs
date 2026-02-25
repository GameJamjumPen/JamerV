using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System.Threading.Tasks;
using JetBrains.Annotations;

public class MainInventorySlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    //ITEM DATA//
    [Header("Data")]
    public CardSO cardSO;
    public bool hasCard;
    public bool isSelected;

    //ITEM SLOT//
    [Header("Item Slot")]
    [SerializeField]
    private Image slotImage;

    // private Image slotImageOutline; comes after already dragging ended
    [SerializeField] private TextMeshProUGUI slotValueText;
    [SerializeField] private Image slotType;
    public GameObject selectedShader;
    [Tooltip("index 0 is ATK , 1 is DEF, 2 is HEAL")]
    public Sprite[] types;

    [Header("Drag and Drop")]
    private Canvas rootCanvas; // Cached canvas for coordinate conversion and drag visuals
    private Transform originalItemParent; // Where the icon returns after drag
    private Vector2 originalItemAnchoredPosition; // Local slot position for snap-back
    private static MainInventorySlot draggingSlot;

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
        UpdateDisplay();
        rootCanvas = FindAnyObjectByType<Canvas>();
        currentstate = NORMAL;
    }
    void Start()
    {
        if (Inventory.instance == null) Debug.LogError("Inventory instance is not found. unable to run inventory system!");
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
        OnDeselected();
    }
    #endregion

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnSelected();
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
        slotImage.gameObject.SetActive(hasCard);
        slotType.gameObject.SetActive(hasCard);
        selectedShader.SetActive(isSelected);
        if (cardSO != null)
        {
            slotImage.sprite = cardLoader.Instance.sprites[cardSO._cardName];
            slotValueText.text = cardSO._value.ToString();


            // Consolidated switch for both damage calculation and type sprite
            float damage = cardSO._value;
            switch (cardSO.cardType)
            {
                case CardType.ATK:
                case CardType.ATKV2:
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
    }

    #region Selected/Deselected Slot
    public void OnSelected()
    {
        Inventory.instance.DeselectedAllSlot();
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
        UpdateDisplay();
        SoundManager.Instance.PlaySFX(sound);
        if (isBar)
        {
            // ChangeAnimationState(BARSELECTED);
        }
        else
        {
            // ChangeAnimationState(SELECTED);
        }
        if (hasCard) Inventory.instance.cardSelected = this.cardSO;
        Inventory.instance.DisplaySelected();
    }
    public void OnDeselected()
    {
        isSelected = false;
        Inventory.instance.DisplayDeselected();
        UpdateDisplay();
        // ChangeAnimationState(NORMAL);
    }
    #endregion
    #region Dragging
    // TODO: เปลี่ยน Ondrag, OnBeginDrag, OnEndDrag, OnDrop

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isSelected) return;
        if (slotImage == null) return;
        if (!hasCard) return;

        draggingSlot = this;
        originalItemParent = slotImage.transform.parent;
        originalItemAnchoredPosition = slotImage.rectTransform.anchoredPosition;
        slotImage.raycastTarget = false;
        if (rootCanvas != null)
        {
            slotImage.transform.SetParent(rootCanvas.transform, true);
        }



        // ChangeAnimationState(DRAG, slotImage.GetComponent<Animator>());
        // Debug.Log("Begin Drag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggingSlot != this) return;
        if (slotImage == null) return;
        if (rootCanvas == null) return;

        RectTransform canvasRect = rootCanvas.transform as RectTransform;
        if (canvasRect == null) return;
        // ChangeAnimationState(DRAG);

        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                eventData.position,
                eventData.pressEventCamera,
                out localPoint))
        {
            // ทำให้รูปภาพ ตรงกับ mouse ที่ชี้
            slotImage.rectTransform.localPosition = localPoint;
        }
        // Use the generic method for InventorySlot

    }

    // // Add this generic method to your class
    // private T GetComponentUnderMouse<T>(PointerEventData eventData) where T : Component
    // {
    //     List<RaycastResult> results = new List<RaycastResult>();

    //     // Raycast for UI elements under the mouse
    //     EventSystem.current.RaycastAll(eventData, results);

    //     // Iterate through the results to find the component of type T
    //     foreach (RaycastResult result in results)
    //     {
    //         T component = result.gameObject.GetComponent<T>();
    //         if (component != null)
    //         {
    //             return component;
    //         }
    //     }
    //     return null;
    // }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggingSlot != this) return;
        if (slotImage == null) return;

        slotImage.transform.SetParent(originalItemParent);
        slotImage.rectTransform.anchoredPosition = originalItemAnchoredPosition;
        slotImage.raycastTarget = true;
        draggingSlot = null;
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (draggingSlot == this) return;
        if (draggingSlot == null) return;
        if (!draggingSlot.hasCard) return;

        if (!hasCard)
        {
            AddItem(draggingSlot.cardSO);
            draggingSlot.RemoveItem();
            OnSelected();
            return;
        }
        CardSO tmpCard = this.cardSO;
        AddItem(draggingSlot.cardSO);
        draggingSlot.AddItem(tmpCard);
        OnSelected();
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        // ChangeAnimationState(HOVER, 0.01f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected)
        {
            // ChangeAnimationState(NORMAL, 0.01f);

        }
        else
        {
            // ChangeAnimationState(SELECTED, 0.01f);
        }
    }
}