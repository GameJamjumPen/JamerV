using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class Paper : MonoBehaviour
{
    public static Paper Instance;
    public EnemyDifficulty enemyDifficulty;
    public bool isVictory;
    public CardSO[] cardSOs;
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
    public void SetVictory(bool victory){
        isVictory = victory;
    }
    public void SetEnemyDifficulty(EnemyDifficulty Diff){
        enemyDifficulty = Diff;
    }
    public void SetCard(CardSO[] cardinput){
        for(int i = 0;i< cardinput.Length;i++){
            cardSOs[i] = cardinput[i];
        }
    }
    public void ClearCard(){
        for(int i = 0;i< cardSOs.Length;i++){
            cardSOs[i] = null;
        }
    }
    public bool GetResult(){
        if(isVictory) return true;
        else return false;
    }
    public void SetBackground(Sprite background){
        sprite = background;
    }

}
