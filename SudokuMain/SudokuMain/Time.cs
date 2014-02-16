using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Threading;
using System.Windows.Controls;

namespace SudokuMain
{
    class Time
    {
        private int _seconds;
        private int _minutes;
        private int _hours;
        DispatcherTimer _dispatch;//hanterar tick
        Label lblClock;//klockan i spelet

        public Time()
        {
            _dispatch = new DispatcherTimer();
            _seconds = 0;
            _minutes = 0;
            _hours = 0;
            _dispatch.Tick += dispatch_Tick;
            _dispatch.Interval = new TimeSpan(0, 0, 0, 1);// en sekunds intervall.
        }

        public Time( ref Label clock)
        {
            _dispatch = new DispatcherTimer();
            _seconds = 0;
            _minutes = 0;
            _hours = 0;
            lblClock = clock;
            _dispatch.Tick += dispatch_Tick;
            _dispatch.Interval = new TimeSpan(0, 0, 0, 1);// en sekunds intervall.
        }

        public int GetSeconds() { return _seconds; }//hämtar sekunder
        public int GetMinutes() { return _minutes; }// hämtar minuter
        public int GetHours() { return _hours; }// hämtar timmar
        public void SetSeconds(int sec) { _seconds = sec; } // sätter sekunder
        public void SetMinutes(int min) { _minutes = min; }// sätter minuter
        public void SetHours(int hour) { _hours = hour; }// sätter timmar
        public void StartTime() { _dispatch.Start(); }//Startar klockan
        public void StopTime() { _dispatch.Stop(); }//Stoppar klockan
        public DispatcherTimer GetDispatcher() { return _dispatch; }//hämtar dispatchen

        //Konverterar tiden till int och returnerar denna
        public int ConvertToScore()
        {
            return ( (_hours * 3600) + (_minutes * 60) + _seconds );
        }

        //sätter nya värden i tiden.
        public void setTime(int sec, int min, int h)
        {
            _seconds = sec;
            _minutes = min;
            _hours = h;
        }

        //Uppdaterar klockan vid varje tick (mycket kod...)
        private void dispatch_Tick(object sender, EventArgs e)
        {
            _seconds++;
            if (_seconds > 59)
            {
                _seconds = 0;
                _minutes++;
            }
            if (_minutes > 59)
            {
                _minutes = 0;
                _hours++;
            }
            //Logik för utfyllnadsnollor.
            if (_seconds <= 9 && //0
               _minutes <= 9 &&
               _hours <= 9)
               lblClock.Content = "0" + _hours + ":0" + _minutes + ":0" + _seconds;
            else if (_seconds > 9 && //1
                     _minutes <= 9 &&
                     _hours <= 9)
                lblClock.Content = "0" + _hours + ":0" + _minutes + ":" + _seconds;
            else if (_seconds <= 9 && //2
                    _minutes > 9 &&
                    _hours <= 9)
                lblClock.Content = "0" + _hours + ":" + _minutes + ":0" + _seconds;
            else if (_seconds > 9 && //3
                    _minutes > 9 &&
                    _hours <= 9)
                lblClock.Content = "0" + _hours + ":" + _minutes + ":" + _seconds;
            else if (_seconds <= 9 && //4
                    _minutes <= 9 &&
                    _hours > 9)
                lblClock.Content = "" + _hours + ":0" + _minutes + ":0" + _seconds;
            else if (_seconds > 9 && //5
                     _minutes <= 9 &&
                     _hours > 9)
                lblClock.Content = "" + _hours + ":0" + _minutes + ":" + _seconds;
            else // 6
                lblClock.Content = "" + _hours + ":" + _minutes + ":" + _seconds;
        }
    }
}
