using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterView : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public Slider healthBar;
    public void setName(string name){
        Name.text = name;
    }
    public void SetHealth(int health){
        healthBar.value = health;
    }
}
