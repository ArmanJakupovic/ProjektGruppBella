using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace SudokuMain
{
    public class Settings
    {
        bool _timer;
        bool _highScore;
        bool _panel;
        int _difficulty;
        bool _online;
        bool _musicOn;

        //konstruktor för inladdning
        public Settings() { loadSettings(); }

        public bool getTimer() { return _timer; }
        public bool getHighscore() { return _highScore; }
        public bool getPanel() { return _panel; }
        public int getDifficulty() { return Convert.ToInt16(_difficulty); }
        public bool getOnline() { return _online; }
        public bool getMusic() { return _musicOn; }

        public void setDifficulty(int diff) { _difficulty = diff; }
        public void SetTimer(bool timer) { _timer = timer; }
        public void SetHighscore(bool highscore) { _highScore = highscore; }
        public void SetPopupPanel(bool panel) { _panel = panel; }
        public void SetOnline(bool online) { _online = online; }
        public void SetMusic(bool music) { _musicOn = music; }

        //Konstruktor
        public Settings(bool time, bool score, bool panel, int diff)
        {
            _timer = time;
            _highScore = score;
            _panel = panel;
            _difficulty = diff; // 0 = beginner, 1 = experienced, 2 = veteran 
        }

        //Konstruktor för MenuWindow (kan vara en dålig idé)
        public Settings(bool time, bool score, bool panel)
        {
            _timer = time;
            _highScore = score;
            _panel = panel;
        }
        
        //Sparar inställningar till fil.
        public void saveSettings()
        {
            StreamWriter writer = new StreamWriter(File.Create("settings.sdk"));
            writer.WriteLine(_timer);
            writer.WriteLine(_highScore);
            writer.WriteLine(_panel);
            writer.WriteLine(_difficulty);
            writer.WriteLine(_online);
            writer.WriteLine(_musicOn);
            writer.Close();
        }

        //Laddar inställningar från fil.
        public void loadSettings()
        {
            if (File.Exists("settings.sdk"))
            {
                StreamReader reader = new StreamReader("settings.sdk");
                _timer = Convert.ToBoolean(reader.ReadLine());
                _highScore = Convert.ToBoolean(reader.ReadLine());
                _panel = Convert.ToBoolean(reader.ReadLine());
                _difficulty = Convert.ToInt16(reader.ReadLine());
                _online = Convert.ToBoolean(reader.ReadLine());
                _musicOn = Convert.ToBoolean(reader.ReadLine());
                reader.Close();
            }
        }
    }
}
