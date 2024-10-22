using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnemyUIManager : UIManager
{
    public static EnemyUIManager Instance { get; private set;}
    void Awake()
    {
        if(Instance == null){Instance=this;}
    }
    public List<Transform> enemySlots;
    public TextMeshProUGUI waveText;
    public List<SpriteRenderer> spriteRenderers;

    public void DisplayNewWave(int waveNumber, List<EnemyModel> enemies, List<Sprite> sprites)
    {
        waveText.text = $"Wave {waveNumber + 1}";
        ClearEnemySlots();

        for (int i = 0; i < enemies.Count; i++)
        {
            Transform slot = enemySlots[i];

            Slider healthSlider = slot.Find("HealthSlider").GetComponent<Slider>();
            Slider shieldSlider = slot.Find("ShieldSlider").GetComponent<Slider>();
            TextMeshProUGUI healthText = healthSlider.GetComponentInChildren<TextMeshProUGUI>();

            spriteRenderers[i].sprite = sprites[i];
            healthSlider.maxValue = enemies[i].Health;
            healthSlider.value = enemies[i].Health;
            shieldSlider.maxValue = 100;
            shieldSlider.value = 0;
            healthText.text = enemies[i].Health.ToString();

            spriteRenderers[i].gameObject.SetActive(true);
            slot.gameObject.SetActive(true);
        }

        for (int j = enemies.Count; j < enemySlots.Count; j++)
        {
            enemySlots[j].gameObject.SetActive(false);
        }
    }

    public void UpdateUI(List<EnemyModel> enemies)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (!enemies[i].IsAlive())
            {
                enemySlots[i].gameObject.SetActive(false);
                spriteRenderers[i].gameObject.SetActive(false);
            }

            Slider healthSlider = enemySlots[i].Find("HealthSlider").GetComponent<Slider>();
            Slider shieldSlider = enemySlots[i].Find("ShieldSlider").GetComponent<Slider>();
            TextMeshProUGUI healthText = healthSlider.GetComponentInChildren<TextMeshProUGUI>();
            healthSlider.value = enemies[i].Health;
            shieldSlider.value = enemies[i].Shield;
            if(enemies[i].IsAlive() && healthText.text!=enemies[i].Health.ToString())
            {
                ShakeAndFlashRed(spriteRenderers[i].color,spriteRenderers[i].gameObject);
            }

            healthText.text = enemies[i].Health.ToString();
        }
    }

    private void ClearEnemySlots()
    {
        foreach (Transform slot in enemySlots)
        {
            Slider healthSlider = slot.GetComponentInChildren<Slider>();
            Image enemyImage = slot.GetComponentInChildren<Image>();
            healthSlider.value = 0;
            healthSlider.maxValue = 1;
            enemyImage.sprite = null;
        }
    }

    public override void ShakeAndFlashRed(Color objColor,GameObject enemyGameobject=null, float shakeDuration = .8f, float shakeAmount = .5f)
    {
        StartCoroutine(ShakeImage(enemyGameobject.transform, shakeDuration, shakeAmount));
        StartCoroutine(FlashRed(objColor, enemyGameobject.GetComponentInChildren<SpriteRenderer>(), 0.5f));
    }
}
