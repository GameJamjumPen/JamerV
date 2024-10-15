using UnityEngine;
using System;
using TMPro;

public class StatUI : MonoBehaviour
{
    public TMP_Text attack;
    public TMP_Text def;
    public TMP_Text heal;
    public TMP_Text point;
    
    public PlayerManager playerManager;

    private void Start()
    {
        SetStat();
    }

    private void OnEnable()
    {
        PlayerManager.ScoreAdded += SetStat;
    }

    private void OnDisable()
    {
        PlayerManager.ScoreAdded -= SetStat;
    }

    void SetStat()
    {
        attack.text = playerManager.stats["Strength"].ToString();
        def.text = playerManager.stats["Defense"].ToString();
        heal.text = playerManager.stats["Heal"].ToString();
        point.text = playerManager.StatPoints.ToString();
    }
}