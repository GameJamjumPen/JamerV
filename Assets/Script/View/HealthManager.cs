using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthManager : MonoBehaviour
{
    public List<Image> images = new List<Image>();
    public int life;
    public Sprite heart;
    public Sprite noheart;

    public PlayerManager playerManager;
    void Start()
    {
        life = playerManager.Life;

        for (int i = 0; i < images.Count; i++)
        {
            if (i < life)
            {
                images[i].sprite =heart;
            }
            else
            {
                images[i].sprite = noheart;
            }
        }
    }
}
