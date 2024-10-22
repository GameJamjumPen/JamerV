using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUIManager : UIManager
{
    public static PlayerUIManager Instance { get; private set;}
    void Awake()
    {
        if(Instance == null){Instance=this;}
    }

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
        float.TryParse(healthText.text, out float Thealth);
        float.TryParse(player.Health.ToString(), out float Phealth);
        if(Thealth>Phealth)
            {
                ShakeAndFlashRed(playerSpriteRenderer.color);
                SoundManager.Instance.PlaySFX("takeDamage");
            }
        healthText.text = player.Health.ToString();
    }

    public override void ShakeAndFlashRed(Color objColor,GameObject obj=null,float shakeDuration = .8f, float shakeAmount = .5f)
    {
        StartCoroutine(ShakeImage(playerSpriteRenderer.transform, shakeDuration, shakeAmount));
        StartCoroutine(FlashRed(objColor,playerSpriteRenderer, 0.5f));
    }

    public void AttackAnimate()
    {
        animator.SetTrigger("attack");
    }
}
