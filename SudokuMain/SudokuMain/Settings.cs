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
        //TODO
        // klassen ska kunna läsa och skriva till filen settings.sdk
        bool _timer;
        bool _highScore;
        bool _panel;
        int _difficulty;

        //konstruktor för inladdning
        public Settings() { loadSettings(); }

        /************************************************************************************
        Följande fyra metoder används i samband med Settings(), som kallar på loadSettings().
        Settings() används vid läsning av fil.
        Settings(bool, bool, bool, int) används vid skrivning till fil.
        *************************************************************************************/
        public bool getTimer() { return _timer; }
        public bool getHighscore() { return _highScore; }
        public bool getPanel() { return _panel; }
        public int getDifficulty() { return Convert.ToInt16(_difficulty); }

        public void setDifficulty(int diff) { _difficulty = diff; }
        public void SetTimer(bool timer) { _timer = timer; }
        public void SetHighscore(bool highscore) { _highScore = highscore; }
        public void SetPopupPanel(bool panel) { _panel = panel; }

        //Konstruktor för användarinput
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
                reader.Close();
            }
        }
    }
}
