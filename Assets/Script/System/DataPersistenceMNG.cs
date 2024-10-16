using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEditor;
using System;

public class DataPersistenceMNG : MonoBehaviour
{
    [Header("god mode")]
    [SerializeField] private bool instantdata = false;
    // to play in any scene when debugging

    [Header("File Storafe")]
    [SerializeField] private string fileName;

    private FileDataHandler fileDataHandler;
    private GameData gameData;

    public static DataPersistenceMNG Instance { get; private set; }

    private List<IDataPersistence> dataPersistenceObject; //POLYMORPHISM NA KUB PIPI

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        Debug.Log("load");
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        Debug.Log("unload");
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObject = FindDataPersistenceObjects();
        LoadGame();
    }


    public void OnSceneUnloaded(Scene scene)
    {
        SaveGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = fileDataHandler.Load();
        if (this.gameData == null && instantdata)
        {
            NewGame();
        }

        if (this.gameData == null)
        {
            return;
        }

        foreach (IDataPersistence dataPersistenceobj in dataPersistenceObject)
        {
            dataPersistenceobj.LoadData(gameData);
        }
        Debug.Log("Loaded");
    }

    public void SaveGame()
    {
        if (this.gameData == null)
        {
            Debug.LogWarning("no data");
            return;
        }

        foreach (IDataPersistence dataPersistenceobj in dataPersistenceObject)
        {
            dataPersistenceobj.SaveData(ref gameData);
        }
        fileDataHandler.Save(gameData);
        Debug.Log("Saved");
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public List<CardData> ConvertScriptableObjectsToData(List<CardSO> cardSOs)
    {
        List<CardData> cardDataList = new List<CardData>();
        foreach (var card in cardSOs)
        {
            CardData cardData = new CardData(card);
            cardDataList.Add(cardData);
        }
        return cardDataList;
    }
    public List<CardSO> ConvertDataToScriptableObjects(List<CardData> cardDataList)
    {
    List<CardSO> cardSOList = new List<CardSO>();

    foreach (var cardData in cardDataList)
    {
        // Create a new CardSO instance
        CardSO newCard = ScriptableObject.CreateInstance<CardSO>();

        newCard._cardName = cardData.cardName;
        newCard._value = cardData.cardValue;

        if (!string.IsNullOrEmpty(cardData.sprite))
        {
            Sprite loadedSprite = AssetDatabase.LoadAssetAtPath<Sprite>(cardData.sprite);
            newCard._cardSprite = loadedSprite;
        }

        if (Enum.TryParse(cardData.cardType, out CardType parsedCardType))
        {
            newCard.cardType = parsedCardType;
        }

        cardSOList.Add(newCard);
    }

    return cardSOList;
}



    private List<IDataPersistence> FindDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceobjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceobjects);
    }
}
