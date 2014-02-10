﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SudokuMain
{
    class Time
    {
        int _seconds;
        int _minutes;
        int _hours;

        public Time() { loadTime(); }
        public Time(int seconds, int minutes, int hours)
        {
            _seconds = seconds;
            _minutes = minutes;
            _hours   = hours;
        }

        public int getSeconds() { return _seconds; }
        public int getMinutes() { return _minutes; }
        public int getHours() { return _hours; }

        public void loadTime()
        {
            if (File.Exists("savedGame.sdk"))
            {
                StreamReader reader = new StreamReader("savedGame.sdk");
                _seconds = Convert.ToInt16(reader.ReadLine());
                _minutes = Convert.ToInt16(reader.ReadLine());
                _hours = Convert.ToInt16(reader.ReadLine());
                reader.Close();
            }
        }
    }
}