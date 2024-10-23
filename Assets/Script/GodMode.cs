using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;  // Required to access Editor-specific functionality
#endif

public class GodMode : Singleton<GodMode>
{
    public bool isGod = false;
    public List<CardSO> godPools;
    
    public void Update(){
        if(Input.GetKeyDown(KeyCode.Tab)){
            TogglePause();
        }
    }

    void TogglePause(){
        #if UNITY_EDITOR
        EditorApplication.isPaused = true;  // Pause the Unity Editor
        Debug.Log("Editor Paused");
        #else
        // If running in a build, do nothing or handle differently
        Debug.Log("Pause only works in the Unity Editor.");
        #endif
    }
}