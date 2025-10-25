using Newtonsoft.Json;
using NUnit.Framework.Constraints;
using System;
using System.IO;
using UnityEngine;

public class JSonDataService : IDataService
{
    public bool SaveData<T>(string relativePath, T data, bool encrypted)
    {
        string path = Application.persistentDataPath + relativePath;

        if (File.Exists(path))
        {
            try
            {
                Debug.Log("Data already exists. Overwriting");
                File.Delete(path);
                using FileStream stream = File.Create(path);
                stream.Close();
                File.WriteAllText(path, JsonConvert.SerializeObject(data));
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error saving data due to {e.Message} {e.StackTrace}");
                return false;
            }
        }
        else
        {
            try
            {
                Debug.Log("Creating new file");
                using FileStream stream = File.Create(path);
                stream.Close();
                File.WriteAllText(path, JsonConvert.SerializeObject(data));
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error saving data due to {e.Message} {e.StackTrace}");
                return false;
            }
        }
    }
    public T LoadData<T>(string relativePath, bool encrypted)
    {
        string path = Application.persistentDataPath + relativePath;

        if (!File.Exists(path))
        {
            Debug.LogError($"Error loading file at {path}. File does not exist");
            throw new FileNotFoundException($"{path} does not exist");
        }

        try
        {
            T data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading data due to {e.Message} {e.StackTrace}");
            throw e;
        }
    }

}
