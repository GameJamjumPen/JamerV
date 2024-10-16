using System.Collections.Generic;
using UnityEngine;

public class BattleInventory : MonoBehaviour, IInventorable , IDataPersistence
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
    public void getCard(List<CardSO> cardSOList)
    {
        foreach (CardSO cardSO in cardSOList)
        {
            cardSOPools.Add(cardSO);
        }
    }
    public void Shuffle(List<CardSO> cardSOList)
    {
        int rand = Random.Range(0, cardSOList.Count);
        AddItem(cardSOList[rand]);
    }

    public void AddDeck(int deckLength)
    {
        for (int i = 0; i < deckLength; i++)
        {
            Shuffle(cardSOPools);
        }
    }

    public void Awake()
    {
        enemyHolders = FindObjectsOfType<EnemyHolder>();
        enemyUIManager = battleController.enemyUIManager;
        playerUIManager = battleController.playerUIManager;
        player = battleController.player;
        getCard(Paper.Instance.cardSOs);
        // AddDeck(3);
    }
    public void DeselectedAllHolder()
    {
        for (int i = 0; i < enemyHolders.Length; i++)
        {
            enemyHolders[i].Deselected();
        }
    }

    public void Update()
    {
        if (CheckEmpty(slotDisplay))
        {
            shuffleUI.SetActive(true);
        }else{
            shuffleUI.SetActive(false);
        }
    }

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

    public bool CheckEmpty(InventorySlot[] slots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].isFull)
            {
                return false;
            }
        }
        return true;
    }

    public void DeselectedAllSlot()
    {
        foreach (InventorySlot inventorySlot in slotDisplay)
        {
            inventorySlot.OnDeselected();
        }
    }

    public void Use()
    {
        if(attackable){
            switch (cardSelected.cardType)
            {
                case CardType.DEF: 
                    player.setShield((int)cardSelected._value);
                    battleController.popUpUI.ShowDamage((int)(cardSelected._value+(defence*0.2)), battleController.playerObject, battleController.defence);
                    this.playerUIManager.UpdatePlayerUI(battleController.player);
                break;
                case CardType.SUP:
                    player.HealByAmount((int)cardSelected._value);
                    battleController.popUpUI.ShowDamage((int)(cardSelected._value+(heals*0.2)), battleController.playerObject, battleController.heal);
                    this.playerUIManager.UpdatePlayerUI(battleController.player);
                break;
                case CardType.ATK:
                    if (enemyHolder == null)
                    {
                        return; // Exit the function if no enemy is selected
                    }
                    CharacterBase.Attack((int)cardSelected._value, enemyHolder.enemyContain);
                    battleController.popUpUI.ShowDamage((int)(cardSelected._value+(strength*0.2)) , enemyHolder.transform , battleController.attack);
                    this.enemyUIManager.updateUI(battleController.enemies);
                    enemyHolder.Deselected();
                    break;
                case CardType.ATKV2:
                    for(int i =0;i<enemyHolders.Length;i++){
                        enemyHolders[i].enemyContain.TakeDamage((int)cardSelected._value);
                        battleController.popUpUI.ShowDamage((int)(cardSelected._value+(strength*0.2)) , enemyHolder.transform , battleController.attack);
                        this.enemyUIManager.updateUI(battleController.enemies);
                        enemyHolder.Deselected();
                    }
                break;
                case CardType.ATKV3:
                    enemyHolder.enemyContain.TakeDamageHealth((int)cardSelected._value);
                    battleController.popUpUI.ShowDamage((int)(cardSelected._value+(strength*0.2)) , enemyHolder.transform , battleController.attack);
                    this.enemyUIManager.updateUI(battleController.enemies);
                    enemyHolder.Deselected();
                break;
                default:
                    break;
            }
            playerUIManager.AttackAnimate();
            // Reset cardSelected and useSlot after use
            useSlot.OnDeselected(); //unselected
            useSlot.RemoveItem(); //remove card from itself
            useSlot = null; //destroy itself
            battleController.isPlayerTurn = false;
            battleController.StartCoroutine(battleController.ChangeTurn(Turn.PlayerAnim));
            attackable = false;
            Debug.Log("Change Turn");
        }else{
            Debug.Log("UnAttackable");
        }
    }

    public void LoadData(GameData data)
    {
        strength = data.strength;
        defence = data.defense;
        heals = data.heal;
    }

    public void SaveData(ref GameData data)
    {
        throw new System.NotImplementedException();
    }
}
