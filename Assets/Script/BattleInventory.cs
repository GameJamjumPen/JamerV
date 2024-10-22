using System.Collections.Generic;
using UnityEngine;

public class BattleInventory : MonoBehaviour, IInventorable, IDataPersistence
{
    public PlayerModel player;
    public List<CardSO> cardSOPools;
    public InventorySlot[] slotDisplay;
    public InventorySlot useSlot;
    public GameObject shuffleUI;
    public CardSO cardSelected;
    public BattleController battleController;
    private EnemyHolder[] enemyHolders;
    public EnemyHolder enemyHolder;
    private EnemyUIManager enemyUIManager;
    private PlayerUIManager playerUIManager;
    public bool attackable = true;

    public float strength;
    public float defence;
    public float heals;

    /// <summary>
    /// Adds cards from a provided list to the cardSOPools list.
    /// </summary>
    /// <param name="cardSOList">List of cards to add.</param>
    public void getCard(List<CardSO> cardSOList)
    {
        foreach (CardSO cardSO in cardSOList)
        {
            cardSOPools.Add(cardSO);
        }
    }

    /// <summary>
    /// Randomly picks a card from the list and adds it to the inventory.
    /// </summary>
    /// <param name="cardSOList">List of cards to shuffle from.</param>
    public void Shuffle(List<CardSO> cardSOList)
    {
        int rand = Random.Range(0, cardSOList.Count);
        AddItem(cardSOList[rand]);
    }

    /// <summary>
    /// Adds a random selection of cards to the deck based on the given deck size.
    /// </summary>
    /// <param name="deckLength">The number of cards to add.</param>
    public void AddDeck(int deckLength)
    {
        for (int i = 0; i < deckLength; i++)
        {
            Shuffle(cardSOPools);
        }
    }

    /// <summary>
    /// Initializes key references for enemy holders, UI managers, and the player, and adds cards from a singleton source to the pool.
    /// </summary>
    public void Awake()
    {
        enemyHolders = FindObjectsOfType<EnemyHolder>();
        enemyUIManager = battleController.enemyUIManager;
        playerUIManager = battleController.playerUIManager;
        player = battleController.player;
        getCard(Paper.Instance.cardSOs);
        // AddDeck(3);
    }

    /// <summary>
    /// Deselects all enemy holders.
    /// </summary>
    public void DeselectedAllHolder()
    {
        for (int i = 0; i < enemyHolders.Length; i++)
        {
            enemyHolders[i].Deselected();
        }
    }

    /// <summary>
    /// Toggles the shuffle UI based on whether the slots are full or not.
    /// </summary>
    public void Update()
    {
        if (!CheckFull(slotDisplay))
        {
            shuffleUI.SetActive(false);
        }
        else
        {
            shuffleUI.SetActive(true);
        }
    }

    /// <summary>
    /// Adds a card to the first available slot in the inventory.
    /// </summary>
    /// <param name="_card">The card to add to the slot.</param>
    public void AddItem(CardSO _card)
    {
        for (int i = 0; i < slotDisplay.Length; i++)
        {
            if (!slotDisplay[i].isFull)
            {
                slotDisplay[i].AddItem(_card);
                return;
            }
        }
    }

    /// <summary>
    /// Returns true if more than one slot is empty.
    /// </summary>
    /// <param name="slots">The array of slots to check.</param>
    /// <returns>True if more than one slot is empty, otherwise false.</returns>
    public bool CheckFull(InventorySlot[] slots)
    {
        int c = 0;
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].isFull)
            {
                c += 1;
            }
        }
        if (c > 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Deselects all inventory slots.
    /// </summary>
    public void DeselectedAllSlot()
    {
        foreach (InventorySlot inventorySlot in slotDisplay)
        {
            inventorySlot.OnDeselected();
        }
    }

    /// <summary>
    /// Executes the selected card’s action (attack, defend, heal, etc.) on the appropriate target based on the card type.
    /// </summary>
    public void Use()
    {
        if (attackable)
        {
            float damage = cardSelected._value;
            switch (cardSelected.cardType)
            {
                case CardType.DEF:
                    player.setShield((int)(damage * (1 + (defence * 0.2))));
                    battleController.popUpUI.ShowDamage((int)(damage * (1 + (defence * 0.2))), battleController.playerObject, battleController.defence);
                    this.playerUIManager.UpdatePlayerUI(battleController.player);
                    break;
                case CardType.SUP:
                    player.HealByAmount((int)(damage * (1 + (heals * 0.2))));
                    battleController.popUpUI.ShowDamage((int)(damage * (1 + (heals * 0.2))), battleController.playerObject, battleController.heal);
                    this.playerUIManager.UpdatePlayerUI(battleController.player);
                    break;
                case CardType.ATK:
                    if (enemyHolder == null)
                    {
                        return; // Exit the function if no enemy is selected
                    }
                    CharacterBase.Attack((int)(damage * (1 + (strength * 0.2))), enemyHolder.enemyContain);
                    battleController.popUpUI.ShowDamage((int)(damage * (1 + (strength * 0.2))), enemyHolder.transform, battleController.attack);
                    this.enemyUIManager.UpdateUI(battleController.enemies);
                    enemyHolder.Deselected();
                    break;
                case CardType.ATKV2:
                    for (int i = 0; i < enemyHolders.Length; i++)
                    {
                        enemyHolders[i].enemyContain.TakeDamage((int)(damage * (1 + (strength * 0.2))));
                        enemyHolder = enemyHolders[i];
                        battleController.popUpUI.ShowDamage((int)(damage * (1 + (strength * 0.2))), enemyHolder.transform, battleController.attack);
                        this.enemyUIManager.UpdateUI(battleController.enemies);
                        enemyHolder.Deselected();
                    }
                    break;
                case CardType.ATKV3:
                    if (enemyHolder == null)
                    {
                        return;
                    }
                    enemyHolder.enemyContain.TakeDamageHealth((int)(damage * (1 + (strength * 0.2))));
                    battleController.popUpUI.ShowDamage((int)(damage * (1 + (strength * 0.2))), enemyHolder.transform, battleController.attack);
                    this.enemyUIManager.UpdateUI(battleController.enemies);
                    enemyHolder.Deselected();
                    break;
                default:
                    break;
            }
            playerUIManager.AttackAnimate();
            SoundManager.Instance.PlaySFX("useCard");
            useSlot.OnDeselected(); //unselected
            useSlot.RemoveItem(); //remove card from itself
            useSlot = null; //destroy itself
            battleController.isPlayerTurn = false;
            battleController.StartCoroutine(battleController.ChangeTurn(Turn.PlayerAnim));
            attackable = false;
        }
        else
        {
            Debug.Log("UnAttackable");
        }
    }

    /// <summary>
    /// Loads player attributes (strength, defense, heals) from saved data.
    /// </summary>
    /// <param name="data">The game data to load.</param>
    public void LoadData(GameData data)
    {
        strength = data.strength;
        defence = data.defense;
        heals = data.heal;
    }

    /// <summary>
    /// Empty method, presumably for saving game data. (To be implemented)
    /// </summary>
    /// <param name="data">The game data to save.</param>
    public void SaveData(ref GameData data)
    {
    }
}