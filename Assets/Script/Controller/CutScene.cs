using UnityEngine;
using System;
using TMPro;

public class CutScene : MonoBehaviour
{
    private PlayerManager playerManager;
    public GameObject randomButton;
    public TMP_Text lifetext;

    private void Awake(){
        playerManager = GetComponent<PlayerManager>();
    }

    public void OnRandomHealthClicked()
    {
        randomButton.SetActive(false);
        randomHealth();
    }

    private void randomHealth()
    {
        int life = UnityEngine.Random.Range(1,3);
        playerManager.SetLife(life);
        lifetext.text = $"YOU GOT {life} LIVES";
    }
}