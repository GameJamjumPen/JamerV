using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // Find existing instances of the Singleton in the scene
                T[] objs = FindObjectsOfType<T>();

                if (objs.Length > 0)
                    _instance = objs[0];

                if (objs.Length > 1)
                {
                    Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
                }

                // If no instance is found, create a new one
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name; // Name the new GameObject after the Singleton type
                    _instance = obj.AddComponent<T>();
                    obj.hideFlags = HideFlags.HideAndDontSave; // Prevent it from showing in the scene and saving
                }
            }
            return _instance;
        }
    }

    // Optional: Ensure the Singleton persists across scenes
    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject); // Destroy duplicates
        }
    }
}
