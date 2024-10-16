using TMPro;
using UnityEngine.UI;
using UnityEngine;


public class CharacterView : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public TextMeshProUGUI damageTextPrefab;
    public Slider healthBar;
    public float MaxHealth;
    public void setName(string name){
        Name.text = name;
    }
    public void SetHealth(int health){
        healthBar.value = health/MaxHealth;
    }
    
}
