using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    private void OnEnable()
    {
        StatPoints += Paper.Instance.score;
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
