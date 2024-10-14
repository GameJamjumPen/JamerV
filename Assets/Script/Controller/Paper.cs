using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paper : MonoBehaviour
{
    public static Paper Instance;
    public EnemyDifficulty enemyDifficulty;
    public bool isVictory;
    public CardSO[] cardSOs;
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
    public void SetVictory(bool victory){
        isVictory = victory;
    }
    public void SetEnemyDifficulty(EnemyDifficulty Diff){
        enemyDifficulty = Diff;
    }

}
