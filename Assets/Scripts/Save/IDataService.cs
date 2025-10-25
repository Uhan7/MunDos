
using System.Collections;
using UnityEngine;


//Saves data in a certain location

//namespace Systems.Persistence
//{
//    public interface IDataService
//    {
//        void Save(GameData data, bool overwrite = true)
//        {
//            GameData load(string name);
//            void Delete(string name);
//            void DeleteAll();
//            IEnumerable<string> listSaves();
//        }
//    }
//}
public interface IDataService
{
    bool SaveData<T>(string relativePath, T data, bool encrypted);

    T LoadData<T>(string relativePath, bool encrypted);
}
