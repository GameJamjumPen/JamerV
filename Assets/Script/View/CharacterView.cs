using System.Collections;
using System.Collections.Generic;
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
    public void ShowDamage(int damage,GameObject gameObjectPos,TextMeshProUGUI damageTextPrefab)
    {
        // Instantiate a damage text object at the specified gameObjectPos
        TextMeshProUGUI damageText = Instantiate(damageTextPrefab,gameObjectPos.transform.position , Quaternion.identity, transform);

        // Set the damage amount and color
        damageText.text = $"-{damage}";
        damageText.color = new Color(1, 0, 0, 1); // Red color with full alpha

        // Start the fade-out animation
        StartCoroutine(FadeAndDestroy(damageText));
    }

    // Coroutine to fade out the damage text and destroy it
    private IEnumerator FadeAndDestroy(TextMeshProUGUI damageText)
    {
        float duration = 3.0f;  // Duration of the fade-out
        float elapsedTime = 0f;

        Color initialColor = damageText.color;

        while (elapsedTime < duration)
        {
            // Gradually reduce the alpha value
            float alpha = Mathf.Lerp(1, 0, elapsedTime / duration);
            damageText.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            
            elapsedTime += Time.deltaTime;
            yield return null;  // Wait for the next frame
        }

        Destroy(damageText.gameObject);  // Destroy the damage text after fading out
    }
}
