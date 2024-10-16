using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Newtonsoft.Json;
using System;

public class FileDataHandler
{
    private string dataPath="";
    private string fileName="";

    public FileDataHandler(string dataPath, string fileName)
    {
        this.dataPath = dataPath;
        this.fileName = fileName;
    }

    public GameData Load()
    {
        string fullPath=Path.Combine(dataPath, fileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try{
                string dataToLoad="";
                using(FileStream stream = new FileStream(fullPath,FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                JsonSerializerSettings settings = new JsonSerializerSettings();
                // settings.Converters.Add(new Vector3Converter());
                
                loadedData=JsonConvert.DeserializeObject<GameData>(dataToLoad,settings);
            }
            catch (Exception)
            {
                Debug.LogError("error");
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath=Path.Combine(dataPath, fileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            JsonSerializerSettings settings = new JsonSerializerSettings{ReferenceLoopHandling = ReferenceLoopHandling.Ignore};
            // settings.Converters.Add(new Vector3Converter());
            string dataToStore = JsonConvert.SerializeObject(data, Formatting.Indented,settings);
            
            using (FileStream stream = new FileStream(fullPath,FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
            Debug.Log("saved to " + fullPath);
        }
        catch (Exception ex)
        {
            Debug.LogError("error"+ ex.Message);
        }
    }
}