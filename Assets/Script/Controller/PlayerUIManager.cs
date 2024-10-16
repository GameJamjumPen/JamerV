using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PlayerUIManager : MonoBehaviour
{
    public TextMeshProUGUI playerNameText;  // Reference to the player's name UI
    public TextMeshProUGUI healthText;
    public Slider playerHealthSlider;       // Reference to the player's health slider
    public Slider playerShieldSlider;
    public Image playerImage;               // Reference to the player's image (sprite)

    
    // Method to update the player's UI
    public void UpdatePlayerUI(PlayerModel player, Sprite playerSprite)
    {
        // Update the player's name
        playerNameText.text = player.Name;

        // Update the player's health slider
        playerHealthSlider.maxValue = player.Health;
        playerHealthSlider.value = player.Health;
        playerShieldSlider.maxValue = 100;
        playerShieldSlider.value   = 0;
        healthText.text = player.Health.ToString();
        // Update the player's image
        playerImage.sprite = playerSprite;
    }

    // Method to update the player's health slider during gameplay
    public void UpdatePlayerUI(PlayerModel player)
    {
        playerHealthSlider.value = player.Health;
        playerShieldSlider.value = player.Shield;
        healthText.text = player.Health.ToString();
    }
    /// <summary>
    /// Shakes the enemy image and flashes it red to simulate damage feedback.
    /// </summary>
    /// <param name="shakeDuration">The duration of the shake effect, in seconds.</param>
    /// <param name="shakeAmount">The amount of shake (intensity).</param>
    public void ShakeAndFlashRed(float shakeDuration = 1f, float shakeAmount = 1f)
    {
        StartCoroutine(ShakeImage(shakeDuration, shakeAmount));
        StartCoroutine(FlashRed(0.5f)); // Flash red for 0.5 seconds
    }

    private IEnumerator ShakeImage(float duration, float amount)
    {
        Vector3 originalPosition = playerImage.rectTransform.localPosition;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            // Create a small random offset for shaking effect
            float offsetX = Random.Range(-amount, amount);
            float offsetY = Random.Range(-amount, amount);

            // Apply the offset to the playerImage's position
            playerImage.rectTransform.localPosition = new Vector3(
                originalPosition.x + offsetX,
                originalPosition.y + offsetY,
                originalPosition.z
            );

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Reset to the original position after shaking
        playerImage.rectTransform.localPosition = originalPosition;
    }

    private IEnumerator FlashRed(float duration)
    {
        Color originalColor = playerImage.color;

        // Set the playerImage to red
        playerImage.color = Color.red;

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Reset the playerImage to its original color
        playerImage.color = originalColor;
    }
}
