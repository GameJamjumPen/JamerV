using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGSound : MonoBehaviour
{
    private string music;

    void Awake()
    {
        Scene s = SceneManager.GetActiveScene();
        music = s.name;
    }

    void Start()
    {
        SoundManager.Instance.PlayMusic(music);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
