using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class EnemyUIManager : MonoBehaviour
{
    public List<Transform> enemySlots;  // UI Slots for displaying enemies
    public TextMeshProUGUI waveText;    // Displays the current wave number

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
            TextMeshProUGUI nameText = slot.GetComponentInChildren<TextMeshProUGUI>();
            Slider healthSlider = slot.GetComponentInChildren<Slider>();
            Image enemyImage = slot.Find("EnemyImage").GetComponent<Image>();

            nameText.text = enemies[i].Name;
            healthSlider.maxValue = enemies[i].Health;
            healthSlider.value = enemies[i].Health;
            enemyImage.sprite = sprites[i];

            // Ensure the slot is active
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
            Slider healthSlider = enemySlots[i].GetComponentInChildren<Slider>();
            healthSlider.value = enemies[i].Health;
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
}
