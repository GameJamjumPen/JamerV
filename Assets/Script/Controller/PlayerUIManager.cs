using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public TextMeshProUGUI playerNameText;  // Reference to the player's name UI
    public Slider playerHealthSlider;       // Reference to the player's health slider
    public Image playerImage;               // Reference to the player's image (sprite)

    // Method to update the player's UI
    public void UpdatePlayerUI(PlayerModel player, Sprite playerSprite)
    {
        // Update the player's name
        playerNameText.text = player.Name;

        // Update the player's health slider
        playerHealthSlider.maxValue = player.Health;
        playerHealthSlider.value = player.Health;

        // Update the player's image
        playerImage.sprite = playerSprite;
    }

    // Method to update the player's health slider during gameplay
    public void UpdatePlayerHealth(int newHealth)
    {
        playerHealthSlider.value = newHealth;
    }
}
