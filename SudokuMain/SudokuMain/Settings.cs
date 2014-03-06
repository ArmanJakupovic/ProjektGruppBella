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

        public Settings() { loadSettings(); }

        public bool GetTimer() { return _timer; }
        public bool GetHighscore() { return _highScore; }
        public bool GetPanel() { return _panel; }
        public int GetDifficulty() { return Convert.ToInt16(_difficulty); }
        public bool GetOnline() { return _online; }
        public bool GetMusic() { return _musicOn; }

        public void SetDifficulty(int diff) { _difficulty = diff; }
        public void SetTimer(bool timer) { _timer = timer; }
        public void SetHighscore(bool highscore) { _highScore = highscore; }
        public void SetPopupPanel(bool panel) { _panel = panel; }
        public void SetOnline(bool online) { _online = online; }
        public void SetMusic(bool music) { _musicOn = music; }

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
