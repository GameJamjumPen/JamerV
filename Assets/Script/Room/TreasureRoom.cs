using UnityEngine;

public class TreasureRoom : Room
{
    //reward
    public CardSO _cardReward;
    private GameObject inventoryObj;

    void Awake()
    {
        Treasure = true;
        inventoryObj = GameObject.FindGameObjectWithTag("inventory");
    }

    public override void OnPlayerAttack()
    {
        if(inventoryObj.activeSelf)
        {inventoryObj.SetActive(false);}

        Inventory inventory = FindObjectOfType<Inventory>();
        SoundManager.Instance.PlaySFX("GetCard");
        Paper.Instance.SetSceneName("treasure");
        inventory.AddItem(_cardReward,cardLoader.Instance.sprites[_cardReward._cardName]);
        Debug.Log("GO to Treasure");
        inventory.istoggleable = true;
    }

    //UI display
    public void UIDisplay()
    {

    }
}
