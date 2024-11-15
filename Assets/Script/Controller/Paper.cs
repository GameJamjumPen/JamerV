
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Paper : MonoBehaviour, IDataPersistence
{
    public static Paper Instance;
    public List<int> wentRoom;
    private Scene scene;
    public int roomNum;
    public int score;
    public string sceneName;
    public EnemyDifficulty enemyDifficulty;
    public bool isVictory;
    public bool isplayed = false;
    public List<CardSO> cardSOs;
    public Sprite sprite;
    public List<List<EnemyType.Enemytype> > enemyTypes = new List<List<EnemyType.Enemytype> >();
    private void Awake()
    {
        Scene scene = SceneManager.GetActiveScene();


        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSceneName(string sceneName)
    {
        this.sceneName = sceneName;
        if (sceneName == "treasure")
        {
            wentRoom.Add(roomNum);
        }
    }

    public void SetVictory(bool victory)
    {
        isplayed = true;
        isVictory = victory;
        if (victory)
        {
            wentRoom.Add(roomNum);
        }
        ClearCard();
    }

    public void SetScore(int score)
    {
        this.score = score;
    }

    public void SetEnemyDifficulty(EnemyDifficulty Diff)
    {
        enemyDifficulty = Diff;
    }

    public void SetCard(CardSO[] cardinput)
    {
        for (int i = 0; i < cardinput.Length; i++)
        {
            cardSOs.Add(cardinput[i]);
        }
    }

    public void ClearCard()
    {
        cardSOs.Clear();
    }
    public bool GetResult()
    {
        if (isVictory) return true;
        else return false;
    }
    public void SetBackground(Sprite background)
    {
        sprite = background;
    }

    public void LoadData(GameData data)
    {
        wentRoom = data.wentRoom;
        if (scene.name == "GameOver" || scene.name == "Win")
        {
            wentRoom = new List<int>();
        }
    }

    public void SaveData(ref GameData data)
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName != "Win" && currentSceneName != "GameOver")
        {
            data.wentRoom = wentRoom;
        }
    }
}
