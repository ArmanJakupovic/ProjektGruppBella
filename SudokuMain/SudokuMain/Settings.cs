﻿using System;
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
        bool _animation;
        int _difficulty;

        //konstruktor för inladdning
        public Settings() { loadSettings(); }

        /************************************************************************************
        Följande fyra metoder används i samband med Settings(), som kallar på loadSettings().
        Notera att det alltså finns två konstruktörer. 
        Settings() används vid läsning av fil.
        Settings(bool, bool, bool, int) används vid skrivning till fil.
        *************************************************************************************/
        public bool getTimer() { return _timer; }
        public bool getHighscore() { return _highScore; }
        public bool getAnimation() { return _animation; }
        public int getDifficulty() { return Convert.ToInt16(_difficulty); }

        //Konstruktor för användarinput
        public Settings(bool time, bool score, bool ani, int diff)
        {
            _timer = time;
            _highScore = score;
            _animation = ani;
            _difficulty = diff; // 0 = beginner, 1 = experienced, 2 = veteran 
        }

        //Sparar inställningar till fil.
        public void saveSettings()
        {
            StreamWriter writer = new StreamWriter(File.Create("settings.sdk"));
            writer.WriteLine(_timer);
            writer.WriteLine(_highScore);
            writer.WriteLine(_animation);
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
                _animation = Convert.ToBoolean(reader.ReadLine());
                _difficulty = Convert.ToInt16(reader.ReadLine());
                reader.Close();
            }
        }
    }
}