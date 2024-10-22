using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBtn : MonoBehaviour
{
    public string objName { get; private set; }
    public TutorialBtn Next;

    void Awake()
    {
        objName = gameObject.name;
    }


    private void OnEnable()
    {
        PlayerPrefs.SetFloat(objName, 1);
    }


    public void Press()
    {
        if (Next != null && !PlayerPrefs.HasKey(Next.objName)) { Next.gameObject.SetActive(true); }
        this.gameObject.SetActive(false);
    }
}