using UnityEngine;

public class TreasureRoom : Room
{
    //reward
    public CardSO _cardReward;

    void Awake()
    {
        Treasure = true;
    }

    public override void OnPlayerAttack()
    {
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
