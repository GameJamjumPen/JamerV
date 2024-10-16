using System.Collections.Generic;
using UnityEngine;

public class cardLoader : MonoBehaviour
{
    public Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();
    public List<string> spriteNames = new List<string>();
    public List<Sprite> spritesList = new List<Sprite>();
    public static cardLoader Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        sprites.Clear();
        SetDict();
    }

    private void SetDict()
    {
        for (int i = 0;i<spriteNames.Count;i++){
            sprites.Add(spriteNames[i], spritesList[i]);
        }
    }
}