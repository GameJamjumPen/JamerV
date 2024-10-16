using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class Paper : MonoBehaviour
{
    public static Paper Instance;
    public int score;
    public string sceneName;
    public EnemyDifficulty enemyDifficulty;
    public bool isVictory;
    public List<CardSO> cardSOs;
    public Sprite sprite;
    private void Awake()
    {
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
    }

    public void SetVictory(bool victory)
    {
        isVictory = victory;
    }

    public void SetScore(int score)
    {
        this.score=score;
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

}
