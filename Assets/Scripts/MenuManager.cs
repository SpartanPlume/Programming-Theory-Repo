using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; } // ENCAPSULATION

    [SerializeField] private TMP_InputField _playerNameInput;

    private SaveData _saveData;

    [System.Serializable]
    private class Highscore
    {
        public Highscore(string name, float time)
        {
            Name = name;
            Time = time;
        }
        public string Name;
        [SerializeField] private float _time;
        public float Time // ENCAPSULATION
        {
            get { return _time; }
            private set { if (value < 0.0f) { Debug.LogError("Time cannot be negative"); } else { _time = value; } }
        }
    }

    [System.Serializable]
    private class SaveData
    {
        public SaveData()
        {
            Highscores = new List<Highscore>();
        }
        public string LastPlayerName;
        public List<Highscore> Highscores;

        public void AddHighscore(Highscore highscore)
        {
            bool hasAddedHighscore = false;
            for (int i = 0; i < Highscores.Count; ++i)
            {
                if (highscore.Time < Highscores[i].Time)
                {
                    Highscores.Insert(i, highscore);
                    hasAddedHighscore = true;
                    break;
                }
            }
            if (!hasAddedHighscore)
            {
                Highscores.Add(highscore);
            }
            if (Highscores.Count > 5)
            {
                Highscores.RemoveAt(5);
            }
        }

        public void AddHighscore(string name, float time)
        {
            AddHighscore(new Highscore(name, time));
        }
    }

    private void Start()
    {
        if (Instance)
        {
            // Destroy old gameObject to have only one MenuManager object
            Destroy(Instance.gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Load();

        // Initialize default values with data from the savefile
        _playerNameInput.text = _saveData.LastPlayerName;
    }

    private void Save()
    {
        string json = JsonUtility.ToJson(_saveData);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    private void Load()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            _saveData = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            // Create a new SaveData if there is no file to load
            _saveData = new SaveData();
        }
    }

    public void StartGame()
    {
        _saveData.LastPlayerName = _playerNameInput.text;
        Save();
    }
}
