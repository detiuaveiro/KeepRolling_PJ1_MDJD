using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager
{
    [System.Serializable]
    public class Save
    {
        public string selectedChair;
        public string selectedPerson;
        public Dictionary<string, float> scores;

        public Save(string selectedChair, string selectedPerson, Dictionary<string, float> scores)
        {
            this.selectedChair = selectedChair;
            this.selectedPerson = selectedPerson;
            this.scores = scores;
        }
    }

    public static void SaveGame(Save saveGame) {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, saveGame);
        file.Close();
    }

    public static void SaveGame(string selectedChair, string selectedPerson, Dictionary<string, float> scores)
    {
        Save saveGame = new(selectedChair, selectedPerson, scores);
        SaveGame(saveGame);
    }

    public static Save LoadSaveGame()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            return save;
        } else
        {
            return new Save("azul","rapaz",new Dictionary<string, float>());
        }
    }
}
