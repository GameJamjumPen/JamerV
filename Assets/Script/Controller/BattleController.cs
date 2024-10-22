using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
public class BattleController : MonoBehaviour
{
    public GameObject tutorial;

    [Header("Assignable")]
    public int shieldprop = 30;
    public int healprop = 10;

    [Header("For Checking")]
    public List<EnemyHolder> enemyHolders;
    [Header("Turns")]
    public Turn turn;
    [Header("Animation")]
    public Transform playerObject;
    public Transform[] enemysPos;
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
    public List<Sprite> PrinceSprites = new List<Sprite>();
    public List<List<EnemyModel>> waves = new List<List<EnemyModel>>();
    public GameObject playerPrefab;
    private BattleModel battleModel;
    public bool isPlayerTurn = true;
    public PlayerModel player;
    public List<EnemyModel> enemies = new List<EnemyModel>(); //POLYMORPHISM NA KUB PIPI
    private List<CharacterView> enemyViews = new List<CharacterView>();
    public TextMeshProUGUI ShowTurn;
    public SpriteRenderer background;
    public SpriteRenderer playerRenderer;
    private int currentWave = -1;
    private List<EnemyAttackInfo> enemyAttackInfos = new List<EnemyAttackInfo>();
    public BattleInventory battleInventory;
    public int score;


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
        StartCoroutine(ChangeTurn(Turn.PlayerAttack));
        SoundManager.Instance.PlayMusic(Paper.Instance.sceneName);
        score = Paper.Instance.score;
        background.sprite = Paper.Instance.sprite;
        // background.sprite = Paper.Instance.sprite;
    }

    void Start()
    {
        if(!PlayerPrefs.HasKey(tutorial.gameObject.name))
        {
            tutorial.SetActive(true);
        }
    }
    
    public IEnumerator ChangeTurn(Turn newTurn)
    {
        Debug.Log($"Attempting to change turn from {turn} to {newTurn}");

        if (newTurn == turn)
        {
            Debug.Log("Turn already set, skipping.");
            yield break;
        }

        turn = newTurn; // Change the turn here

        switch (turn)
        {
            case Turn.PlayerAttack:
                Debug.Log("Player's Attack Turn");
                ShowTurn.text = "Player Turn !";
                battleInventory.attackable = true;
                break;
            case Turn.PlayerAnim:
                Debug.Log("Player's Animation Turn");
                ShowTurn.text = "Enemy Turn !";
                yield return StartCoroutine(WaitforAnim(1f));
                StartCoroutine(ChangeTurn(Turn.EnemyThink));
                break;
            case Turn.EnemyThink:
                Debug.Log("Enemy's Thinking Turn");
                ShowTurn.text = "Enemy Turn !";
                yield return StartCoroutine(WaitforDebug(1f)); // Coroutine waiting
                StartCoroutine(ChangeTurn(Turn.Enemyattack));
                break;
            case Turn.Enemyattack:
                Debug.Log("Enemy's Attack Turn");
                ShowTurn.text = "Enemy Turn !";
                yield return StartCoroutine(EnemyTurn());
                break;
            case Turn.EnemyAnim:
                Debug.Log("Enemy's Animation Turn");
                ShowTurn.text = "Enemy Turn !";
                yield return StartCoroutine(HandleEnemyAnimations());
                StartCoroutine(ChangeTurn(Turn.PlayerAttack));
                break;
        }
    }
    private IEnumerator WaitforDebug(float Second)
    {
        Debug.Log("Thinking");
        yield return new WaitForSeconds(Second);
    }
    private IEnumerator HandleEnemyAnimations()
    {
        for (int i = 0; i < enemies.Count && i < enemyAttackInfos.Count; i++)
        {
            if (enemies[i].IsAlive() && enemyAttackInfos != null)
            {
                if (enemyAttackInfos[i] != null && enemyAttackInfos[i].isAtk)
                {
                    popUpUI.ShowDamage(enemyAttackInfos[i].valueStat, enemysPos[i], attack);
                    Debug.Log("Enemy atk");
                }
                else if (enemyAttackInfos[i] != null)
                {
                    if (enemyAttackInfos[i].isDef)
                    {
                        popUpUI.ShowDamage(enemyAttackInfos[i].valueStat, enemysPos[i], defence);
                    }
                    else if (enemyAttackInfos[i].isHeal)
                    {
                        popUpUI.ShowDamage(enemyAttackInfos[i].valueStat, enemysPos[i], heal);
                    }
                    Debug.Log("Enemy Defence or heal");
                }
                // Optional delay between animations
                yield return new WaitForSeconds(0.3f);
            }
        }
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
        // ShowCurrentTurn();
    }
    void initPlayer()
    {
        player = new PlayerModel();
        playerUIManager.UpdatePlayerUI(player, playerSprite);
    }

    private IEnumerator EnemyTurn()
    {
        BattleModel.ResetShield(enemies);
        enemyAttackInfos = EnemyModel.AttackPlayer(enemies, player, shieldprop, healprop,this);
        playerUIManager.UpdatePlayerUI(player);
        enemyUIManager.UpdateUI(enemies);
        GameOver();
        isPlayerTurn = true;
        // ShowCurrentTurn();
        yield return new WaitForSeconds(1f); // Add delay before continuing
        StartCoroutine(ChangeTurn(Turn.EnemyAnim));
    }

    void AttackCharacter(ICharacter attacker, ICharacter target)
    {
        if (attacker.IsAlive() && target.IsAlive())
        {
            attacker.Attack(target);
            Debug.Log(target.Name + " current health is " + target.Health.ToString());
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
            if (currentWave < waves.Count-1 && battleModel.player.IsAlive())
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
        isPlayerTurn = true;
        ChangeTurn(Turn.PlayerAttack);
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
            case "Prince":
                return PrinceSprites[UnityEngine.Random.Range(0, PrinceSprites.Count)];
            default:
                return FishSprites[0]; // Default fallback sprite
        }
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
