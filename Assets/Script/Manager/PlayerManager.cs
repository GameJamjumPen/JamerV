using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour, IDataPersistence
{
    #region Stats

    public int Life { get; private set; }
    public int StatPoints;
    public Dictionary<string, int> stats {get; private set;}

    public static event Action ScoreAdded;

    #endregion
    private void Awake()
    {
        stats = new Dictionary<string, int>
        {
            { "Strength", 0 },
            { "Defense", 0 },
            { "Heal", 0 }
        };
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AddScoreFromPaper();
    }


    public void LoadData(GameData data)
    {
        Life = data.life;
        StatPoints = data.statPoints;
        stats["Strength"] = data.strength;
        stats["Defense"] = data.defense;
        stats["Heal"] = data.heal;
    }

    public void SaveData(ref GameData data)
    {
        data.life = Life;
        data.statPoints = StatPoints;
        data.strength = stats["Strength"];
        data.defense = stats["Defense"];
        data.heal = stats["Heal"];
    }

    public void AddScoreFromPaper()
    {
        if (Paper.Instance != null)
        {
            StatPoints += Paper.Instance.score;
            Debug.Log("Added score from Paper. New StatPoints: " + StatPoints);
            ScoreAdded?.Invoke();
        }
    }


    public void SetLife(int life)
    {
        this.Life = life;
    }

    //add the button to use stat points
    public void AddPoint()
    {
        if (StatPoints > 0)
        {
            SoundManager.Instance.PlaySFX("AddStat");
            System.Random random = new System.Random();
            List<string> statList = stats.Keys.ToList();
            string randomStat = statList[random.Next(statList.Count)];
            stats[randomStat] += 1;
            StatPoints -= 1;
            ScoreAdded?.Invoke();
        }
    }

    public void Die()
    {
        Life-=1;
    }

}
