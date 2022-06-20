using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; } // ENCAPSULATION

    [System.Serializable]
    private class Highscore
    {
        public Highscore(string name, float time)
        {
            Name = name;
            Time = time;
        }
        public string Name { get; private set; } // ENCAPSULATION
        private float _time;
        public float Time // ENCAPSULATION
        {
            get { return _time; }
            private set { if (value < 0.0f) { Debug.LogError("Time cannot be negative"); } else { _time = value; } }
        }
    }

    [System.Serializable]
    private class SaveData
    {
        public SaveData(string lastPlayerName, List<Highscore> highscores)
        {
            LastPlayerName = lastPlayerName;
            Highscores = highscores;
        }
        public string LastPlayerName { get; set; }
        public List<Highscore> Highscores { get; private set; } // ENCAPSULATION

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

    private SaveData _saveData;

    private void Start()
    {
        if (Instance)
        {
            // Destroy old gameObject to have only one MenuManager object
            Destroy(Instance.gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
