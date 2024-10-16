using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
public class BattleController : MonoBehaviour
{
    [Header("Assignable")]
    public int shieldprop = 30;
    public int healprop = 10;

    [Header("For Checking")]
    public List<EnemyHolder> enemyHolders;
    [Header("Turns")]
    public Turn turn;
    [Header("Animation")]
    public TextMeshProUGUI TextPopup;
    public Transform playerObject;
    public Transform[] enemysPos;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private Animator[] _enemyAnim;
    private static string ATTACK = "attack";
    public PopUpUI popUpUI;
    public Color attack;
    public Color heal;
    public Color defence;
    [Header("Etc...")]
    public EnemyUIManager enemyUIManager;
    public PlayerUIManager playerUIManager;
    public Sprite playerSprite;
    public List<Sprite> FishSprites = new List<Sprite>();
    public List<Sprite> KniveSprites = new List<Sprite>();
    public List<Sprite> FolkSprites = new List<Sprite>();
    public List<Sprite> SpoonSprites = new List<Sprite>();
    public List<Sprite> BeeSprites = new List<Sprite>();
    public List<List<EnemyModel>> waves = new List<List<EnemyModel>>();
    public GameObject playerPrefab;
    private BattleModel battleModel;
    public bool isPlayerTurn = true;
    public PlayerModel player;
    public List<EnemyModel> enemies = new List<EnemyModel>(); //POLYMORPHISM NA KUB PIPI
    private List<CharacterView> enemyViews = new List<CharacterView>();
    public TextMeshProUGUI ShowTurn;
    //public Image background;
    private int currentWave = -1;
    private List<EnemyAttackInfo> enemyAttackInfos = new List<EnemyAttackInfo>();
    public BattleInventory battleInventory;


    void Awake()
    {
        battleInventory = FindObjectOfType<BattleInventory>();
        enemyUIManager = FindObjectOfType<EnemyUIManager>();
        playerUIManager = FindObjectOfType<PlayerUIManager>();
        popUpUI = FindObjectOfType<PopUpUI>();

        if (enemyUIManager == null)
        {
            Debug.LogError("EnemyUIManager not found in the scene! Make sure it's attached to a GameObject.");
            return;
        }
        if (GenerateEnemyWave.Instance == null)
        {
            Debug.LogError("WTF Singleton bug");
        }
        GenerateEnemyWave.Instance.CreateEnemyWaves();
        waves = GenerateEnemyWave.Instance.GetEnemyWaves();
        initPlayer();
        NewWave();
        isPlayerTurn = true;
        OnTurnChange(Turn.PlayerAttack);
        //background.sprite = Paper.Instance.sprite;
        // background.sprite = Paper.Instance.sprite;
    }
    public void OnTurnChange(Turn newTurn)
    {
        Debug.Log($"Attempting to change turn from {turn} to {newTurn}");

        if (newTurn == turn)
        {
            Debug.Log("Turn already set, skipping.");
            return;
        }

        turn = newTurn; // Change the turn here

        switch (turn)
        {
            case Turn.PlayerAttack:
                Debug.Log("Player's Attack Turn");
                return;
            case Turn.PlayerAnim:
                Debug.Log("Player's Animation Turn");
                StartCoroutine(WaitforAnim(20f));
                OnTurnChange(Turn.EnemyThink);
                break;
            case Turn.EnemyThink:
                Debug.Log("Enemy's Thinking Turn");
                StartCoroutine(WaitforDebug(50f)); // Coroutine waiting
                OnTurnChange(Turn.Enemyattack);
                break;
            case Turn.Enemyattack:
                Debug.Log("Enemy's Attack Turn");
                EnemyTurn();
                break;
            case Turn.EnemyAnim:
                Debug.Log("Enemy's Animation Turn");
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i].IsAlive() && enemyAttackInfos!=null)
                    {
                        if (enemyAttackInfos[i] != null && enemyAttackInfos[i].isAtk)
                        {
                            popUpUI.ShowDamage(enemyAttackInfos[i].valueStat,enemysPos[i],attack);
                            Debug.Log("Enemy atk");
                        }
                        else if (enemyAttackInfos[i] != null)
                        {
                            if(enemyAttackInfos[i].isDef){
                                popUpUI.ShowDamage(enemyAttackInfos[i].valueStat,enemysPos[i],defence);
                            }else if(enemyAttackInfos[i].isHeal){
                                popUpUI.ShowDamage(enemyAttackInfos[i].valueStat,enemysPos[i],heal);
                            }
                            Debug.Log("Enemy Defence or heal"); //heal and def show the same so we merge it together
                        }
                    }
                }
                battleInventory.attackable = true;
                OnTurnChange(Turn.PlayerAttack);
                break;
        }
    }
    private IEnumerator WaitforDebug(float Second)
    {
        Debug.Log("Thinking");
        yield return new WaitForSeconds(Second);
    }
    private IEnumerator WaitforAnim(float second)
    {
        Debug.Log("Waiting For Animation");
        yield return new WaitForSeconds(second);

    }
    #region UIAnim
    public void ShowDamage(int damage, GameObject gameObjectPos, TextMeshProUGUI damageTextPrefabs)
    {
        // Instantiate a damage text object at the specified gameObjectPos
        TextMeshProUGUI damageText = Instantiate(damageTextPrefabs, gameObjectPos.transform.position, Quaternion.identity, transform);

        // Set the damage amount and color
        damageText.text = $"-{damage}";
        damageText.color = new Color(1, 0, 0, 1); // Red color with full alpha

        // Start the fade-out animation
        StartCoroutine(FadeAndDestroy(damageText));
        Debug.Log("Show UI");
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
    #endregion

    void Update()
    {
        // Debug.Log(turn);
        // if (!isPlayerTurn)
        // {
        //     ShowCurrentTurn();
        //     EnemyTurn();
        // }
        ShowCurrentTurn();
    }
    void initPlayer()
    {
        player = new PlayerModel();
        playerUIManager.UpdatePlayerUI(player, playerSprite);
    }

    void PlayerTurn()
    {
        FindEnemyAddAttack();
        Debug.Log("PlayerTurn");
        GameOver();
        isPlayerTurn = false;
        OnTurnChange(Turn.EnemyThink);
    }
    public void PlayerClickAttack()
    {
        if (isPlayerTurn)
        {
            BattleModel.ResetShield(player);
            FindEnemyAddAttack();
            playerUIManager.UpdatePlayerUI(player);
            isPlayerTurn = false;
            OnTurnChange(Turn.EnemyThink);
        }

    }
    public void PlayerShield()
    {
        if (isPlayerTurn)
        {
            BattleModel.ResetShield(player);
            player.setShield(30);
            playerUIManager.UpdatePlayerUI(player);
            isPlayerTurn = false;
            OnTurnChange(Turn.EnemyThink);
        }

    }
    public void PlayerHealing()
    {
        if (isPlayerTurn)
        {
            BattleModel.ResetShield(player);
            player.HealByAmount(30);
            playerUIManager.UpdatePlayerUI(player);
            isPlayerTurn = false;
            BattleModel.ResetShield(enemies);
            OnTurnChange(Turn.EnemyThink);
        }
    }
    void EnemyTurn()
    {
        BattleModel.ResetShield(enemies);
        enemyAttackInfos = EnemyModel.AttackPlayer(enemies, player, shieldprop, healprop);
        playerUIManager.UpdatePlayerUI(player);
        enemyUIManager.updateUI(enemies);
        GameOver();
        isPlayerTurn = true;
        ShowCurrentTurn();
        OnTurnChange(Turn.EnemyAnim);
    }

    void AttackCharacter(ICharacter attacker, ICharacter target)
    {
        if (attacker.IsAlive() && target.IsAlive())
        {
            attacker.Attack(target);
            Debug.Log(target.Name + " current health is " + target.Health.ToString());
        }
    }
    void FindEnemyAddAttack()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].IsAlive())
            {
                Debug.Log("Attack");
                AttackCharacter(player, enemies[i]);
                if (!enemies[i].IsAlive())
                {
                    Debug.Log("Is Dead");
                    enemyUIManager.SetActiveFalseOf(i);
                    // enemyInstances[i].SetActive(false);
                }
                enemyUIManager.updateUI(enemies);
                break;
            }
        }
    }
    void GameOver()
    {
        if (battleModel == null)
        {
            Debug.LogError("BattleModel is NULL");
            return;
        }
        if (battleModel.player == null || battleModel.enemies == null)
        {
            Debug.LogError("BattleModel's player or enemies are NULL");
            return;
        }
        if (battleModel.IsBattleOver())
        {
            Debug.Log("Wave Over");
            if (currentWave < 2 && battleModel.player.IsAlive())
            {
                NewWave();
            }
            else
            {
                //check win or lose
                if (battleModel.player.IsAlive())
                {
                    Paper.Instance.SetVictory(true);
                }
                else
                {
                    Paper.Instance.SetVictory(false);
                }
                SceneChange.ChangeSceneFunc("MainBoard");
            }
            return;
        }
    }

    void ClearLists()
    {
        enemies.Clear();
        enemyViews.Clear();
        // enemyInstances.Clear();
    }
    void NewWave()
    {
        Debug.Log("NEW WAVE !!!");
        currentWave++;
        ClearLists();
        RenderEnemy();
    }
    void RenderEnemy()

    {
        enemies = waves[currentWave];

        List<Sprite> enemySprites = new List<Sprite>();

        foreach (var enemy in enemies)
        {
            enemySprites.Add(GetRandomSpriteOf(enemy.Name));
        }
        enemyUIManager.DisplayNewWave(currentWave, enemies, enemySprites);
        AddEnemyToHolder(enemies);

        battleModel = new BattleModel(player, enemies);

        if (battleModel == null)
        {
            Debug.LogError("BattleModel is NULL after init");
            return;
        }
        if (battleModel.player == null || battleModel.enemies == null)
        {
            Debug.LogError("BattleModel's player or enemies are NULL");
            return;
        }
    }
    public void AddEnemyToHolder(List<EnemyModel> enemies)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (!enemies[i].IsAlive())
            {
                continue;
            }
            enemyHolders[i].enemyContain = enemies[i];
        }
    }

    Sprite GetRandomSpriteOf(string enemyName)
    {
        switch (enemyName)
        {
            case "Fish":
                return FishSprites[UnityEngine.Random.Range(0, FishSprites.Count)];
            case "Knive":
                return KniveSprites[UnityEngine.Random.Range(0, KniveSprites.Count)];
            case "Folk":
                return FolkSprites[UnityEngine.Random.Range(0, FolkSprites.Count)];
            case "Spoon":
                return SpoonSprites[UnityEngine.Random.Range(0, SpoonSprites.Count)];
            case "Bee":
                return BeeSprites[UnityEngine.Random.Range(0, BeeSprites.Count)];
            default:
                return FishSprites[0]; // Default fallback sprite
        }
    }
    void ShowCurrentTurn()
    {
        if (isPlayerTurn)
        {
            ShowTurn.text = "Player Turn !";
            return;
        }
        ShowTurn.text = "Enemy Turn !";
    }
}

public enum Turn
{
    PlayerAnim,
    EnemyThink,
    Enemyattack,
    EnemyAnim,
    PlayerAttack,
}
