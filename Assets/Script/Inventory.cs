using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, IInventorable, IDataPersistence
{
    #region Declare Variable
    public KeyCode openCloseKey = KeyCode.Tab;
    public bool istoggleable;
    public GameObject backPackSystem;
    public GameObject pocketSystem;
    public GameObject stats;
    public Collider2D dice;
    public GameObject inv;
    public Image cardAddedTreasure;
    public GameObject newCardPopup;

    [Header("Slots")]
    public CardSO cardSelected;
    public InventorySlot[] itemSlots;
    public InventorySlot[] actualSlots;
    public InventorySlot[] allSlots;
    public List<CardSO> StarterPack;

    [Header("Display")]
    public Image image;
    public TextMeshProUGUI cardName;
    public Image cardImage;
    public Image cardType;
    public TextMeshProUGUI cardStat;
    public TextMeshProUGUI cardDes;

    [Tooltip("Types[0] will be ATK Types[1] will be DEF Types[2] will be SUP card")]
    public Sprite[] types;

    #endregion
    public void Awake()
    {
        istoggleable = true;
        inv.SetActive(false);
    }

    public void LoadData(GameData data)
    {
        List<CardSO> slotToLoad = new List<CardSO>(DataPersistenceMNG.Instance.ConvertDataToScriptableObjects(data.inventoryData));
        foreach (CardSO card in slotToLoad)
        {
            AddItem(card);
        }

        List<CardSO> loadoutToSlot = new List<CardSO>(DataPersistenceMNG.Instance.ConvertDataToScriptableObjects(data.loadoutData));
        foreach (CardSO card in loadoutToSlot)
        {
            AddItem(card);
        }

        if (loadoutToSlot.Count == 0 && slotToLoad.Count == 0)
        {
            foreach (CardSO card in StarterPack)
            {
                AddItem(card);
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        List<CardSO> slotToSave = new List<CardSO>();
        foreach (InventorySlot card in itemSlots)
        {
            if (card.cardSO != null)
            {
                slotToSave.Add(card.cardSO);
            }
        }

        data.inventoryData = DataPersistenceMNG.Instance.ConvertScriptableObjectsToData(slotToSave);
    }

    public void Update()
    {
        if (istoggleable)
        {
            if (Input.GetKeyDown(openCloseKey))
            {
                if (image.enabled)
                {
                    image.enabled = false;
                    inv.SetActive(false);
                    dice.enabled = true;
                }
                else
                {
                    image.enabled = true;
                    inv.SetActive(true);
                    dice.enabled = false;
                }
            }
        }
    }
    // public void Update(){
    //     if()
    // }
    public void AddItem(CardSO _card)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (!itemSlots[i].isFull)
            {
                itemSlots[i].AddItem(_card);
                return;
            }
        }
    }

    public bool CheckFull(InventorySlot[] slots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].isFull)
            {
                return false;
            }
        }
        return true;
    }

    public void AddItem(CardSO _card, Sprite cardPic)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (!itemSlots[i].isFull)
            {
                itemSlots[i].AddItem(_card);
                newCardPopup.SetActive(true);
                Debug.Log("TTTT");
                cardAddedTreasure.sprite = cardPic;
                return;
            }
        }
    }

    public void CloseUI()
    {
        newCardPopup.SetActive(false);
    }

    public bool CheckType(InventorySlot[] slots, bool isAttack, bool isDefence, bool isSupport)
    {
        bool ATKalr = false;
        bool DEFalr = false;
        bool SUPalr = false;

        // Check the slots to set the flags for each card type
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].cardSO == null)
            {
                continue;
            }
            if (slots[i].cardSO.cardType == CardType.ATK)
            {
                ATKalr = true;
            }
            if (slots[i].cardSO.cardType == CardType.DEF)
            {
                DEFalr = true;
            }
            if (slots[i].cardSO.cardType == CardType.SUP)
            {
                SUPalr = true;
            }
        }

        // Only check the required card types based on the parameters
        bool attackCheck = !isAttack || ATKalr;
        bool defenceCheck = !isDefence || DEFalr;
        bool supportCheck = !isSupport || SUPalr;

        // If all required conditions are met, return true
        return attackCheck && defenceCheck && supportCheck;
    }
    #region SlotSelected
    public void DeselectedAllSlot()
    {
        foreach (InventorySlot slot in allSlots)
        {
            slot.OnDeselected();
        }
    }

    public void DisplaySelected()
    {
        if (!cardName.gameObject.activeSelf) cardName.gameObject.SetActive(true);
        if (!cardImage.gameObject.activeSelf) cardImage.gameObject.SetActive(true);
        if (!cardType.gameObject.activeSelf) cardType.gameObject.SetActive(true);
        if (!cardStat.gameObject.activeSelf) cardStat.gameObject.SetActive(true);
        if (!cardDes.gameObject.activeSelf) cardDes.gameObject.SetActive(true);

        cardName.text = cardSelected._cardName;
        cardImage.sprite = cardLoader.Instance.sprites[cardSelected._cardName];
        cardStat.text = cardSelected._value.ToString();
        this.cardDes.text = cardSelected.cardDes;
        if (cardSelected.cardType == CardType.ATK || cardSelected.cardType == CardType.ATKV2 || cardSelected.cardType == CardType.ATKV3)
        {
            cardType.sprite = types[0];
        }
        else if (cardSelected.cardType == CardType.DEF)
        {
            cardType.sprite = types[1];
        }
        else
        {
            cardType.sprite = types[2];
        }
    }
    public void DisplayDeselected()
    {
        cardName.gameObject.SetActive(false);
        cardImage.gameObject.SetActive(false);
        cardType.gameObject.SetActive(false);
        cardStat.gameObject.SetActive(false);
        cardDes.gameObject.SetActive(false);
    }


    #endregion


}
