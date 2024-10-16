using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Card", menuName = "SO/Card")]
public class CardSO : ScriptableObject
{
    public string _cardName;
    public Sprite _cardSprite;
    public CardType cardType;
    public float _value;

}
public enum CardType{ATK , DEF, SUP,ATKV2,ATKV3}
