using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class EnemyUIManager : MonoBehaviour
{
    public List<Transform> enemySlots;  // UI Slots for displaying enemies
    public TextMeshProUGUI waveText;
    public List<GameObject> enemyObjects;    // Displays the current wave number

    public List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    // Initialize the UI elements for a new wave
    public void DisplayNewWave(int waveNumber, List<EnemyModel> enemies, List<Sprite> sprites)
    {
        // Update the wave text
        waveText.text = $"Wave {waveNumber + 1}";

        // Clear the previous enemy slots
        ClearEnemySlots();

        // Display each enemy in the correct slot with its sprite and stats
        int i;
        for (i = 0; i < enemies.Count; i++)
        {
            Transform slot = enemySlots[i];

            // Set the name and health
            //TextMeshProUGUI nameText = slot.GetComponentInChildren<TextMeshProUGUI>();
            Slider healthSlider = slot.Find("HealthSlider").GetComponent<Slider>();
            Slider shieldSlider = slot.Find("ShieldSlider").GetComponent<Slider>();
            TextMeshProUGUI healthText = healthSlider.GetComponentInChildren<TextMeshProUGUI>();
            // Sprite enemyImage = enemyObjects[i].GetComponent<Sprite>();ss
            spriteRenderers[i].sprite = sprites[i];
            //nameText.text = enemies[i].Name;
            healthSlider.maxValue = enemies[i].Health;
            healthSlider.value = enemies[i].Health;
            // enemyImage = sprites[i];
            shieldSlider.maxValue = 100;
            shieldSlider.value = 0;
            healthText.text = enemies[i].Health.ToString();
            // Ensure the slot is active
            spriteRenderers[i].gameObject.SetActive(true);
            slot.gameObject.SetActive(true);
        }

        // Deactivate any unused slots
        for (int j = i; j < enemySlots.Count; j++)
        {
            enemySlots[j].gameObject.SetActive(false);
        }
    }
    public void updateUI(List<EnemyModel> enemies){
        for (int i = 0; i < enemies.Count; i++)
        {
            if(!enemies[i].IsAlive()){
                enemySlots[i].gameObject.SetActive(false);
                spriteRenderers[i].gameObject.SetActive(false);
            }
            Slider healthSlider = enemySlots[i].Find("HealthSlider").GetComponent<Slider>();
            Slider shieldSlider = enemySlots[i].Find("ShieldSlider").GetComponent<Slider>();
            TextMeshProUGUI healthText = healthSlider.GetComponentInChildren<TextMeshProUGUI>();
            healthSlider.value = enemies[i].Health;
            shieldSlider.value = enemies[i].Shield;
            healthText.text = enemies[i].Health.ToString();
        }
    }

    // Clear all enemy slots to prepare for the next wave
    private void ClearEnemySlots()
    {
        foreach (Transform slot in enemySlots)
        {
            TextMeshProUGUI nameText = slot.GetComponentInChildren<TextMeshProUGUI>();
            Slider healthSlider = slot.GetComponentInChildren<Slider>();
            Image enemyImage = slot.GetComponentInChildren<Image>();

            nameText.text = "Empty";
            healthSlider.value = 0;
            healthSlider.maxValue = 1;
            enemyImage.sprite = null; // Reset the sprite
        }
    }
    public void SetActiveFalseOf(int i){
        enemySlots[i].gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Shakes the enemy image and flashes it red to simulate damage feedback.
    /// </summary>
    /// <param name="enemyGameObject">The enemy GameObject containing the image to shake and flash.</param>
    /// <param name="shakeDuration">The duration of the shake effect, in seconds.</param>
    /// <param name="shakeAmount">The amount of shake (intensity).</param>
    public void ShakeAndFlashRed(GameObject enemyGameobject,float shakeDuration = 1f, float shakeAmount = 1f)
    {
        StartCoroutine(ShakeImage(shakeDuration, shakeAmount,enemyGameobject));
        StartCoroutine(FlashRed(0.5f,enemyGameobject)); // Flash red for 0.5 seconds
    }

    private IEnumerator ShakeImage(float duration, float amount,GameObject enemyGameobject)
    
    {
        Image enemyImage = enemyGameobject.GetComponentInChildren<Image>();
        Vector3 originalPosition = enemyImage.rectTransform.localPosition;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            // Create a small random offset for shaking effect
            float offsetX = Random.Range(-amount, amount);
            float offsetY = Random.Range(-amount, amount);

            // Apply the offset to the enemyImage's position
            enemyImage.rectTransform.localPosition = new Vector3(
                originalPosition.x + offsetX,
                originalPosition.y + offsetY,
                originalPosition.z
            );

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Reset to the original position after shaking
        enemyImage.rectTransform.localPosition = originalPosition;
    }

    private IEnumerator FlashRed(float duration,GameObject enemyGameobject)
    {
        Image enemyImage = enemyGameobject.GetComponentInChildren<Image>();
        Color originalColor = enemyImage.color;

        // Set the enemyImage to red
        enemyImage.color = Color.red;

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Reset the enemyImage to its original color
        enemyImage.color = originalColor;
    }
}
