using UnityEngine;

[CreateAssetMenu(fileName ="New Card", menuName = "SO/Card")]
public class CardSO : ScriptableObject
{
    public string _cardName;
    public CardType cardType;
    public float _value;
    
    public string cardDes;
    public Animator animator;

}
public enum CardType{ATK , DEF, SUP,ATKV2,ATKV3}
