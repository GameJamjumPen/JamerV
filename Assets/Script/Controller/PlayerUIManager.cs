using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUIManager : UIManager
{
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI healthText;
    public Slider playerHealthSlider;
    public Slider playerShieldSlider;
    public SpriteRenderer playerSpriteRenderer;
    public Animator animator;

    public void UpdatePlayerUI(PlayerModel player, Sprite playerSprite)
    {
        playerNameText.text = player.Name;
        playerHealthSlider.maxValue = player.MaxHealth;
        playerHealthSlider.value = player.Health;
        playerShieldSlider.maxValue = 100;
        playerShieldSlider.value = player.Shield; // Update shield value as needed
        healthText.text = player.Health.ToString();

        playerSpriteRenderer.sprite = playerSprite;
    }

    public void UpdatePlayerUI(PlayerModel player)
    {
        playerHealthSlider.value = player.Health;
        playerShieldSlider.value = player.Shield;
        healthText.text = player.Health.ToString();
    }

    public void ShakeAndFlashRed(float shakeDuration = 1f, float shakeAmount = 1f)
    {
        StartCoroutine(ShakeImage(playerSpriteRenderer.transform, shakeDuration, shakeAmount));
        StartCoroutine(FlashRed(playerSpriteRenderer, 0.5f));
    }

    public void AttackAnimate()
    {
        animator.SetTrigger("attack");
    }
}
