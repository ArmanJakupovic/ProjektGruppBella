using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Media;
using WMPLib;
using System.Threading;

namespace SudokuMain
{
    class MusicHandler
    {
        Random _newRnd;
        WindowsMediaPlayer _myPlayer;
        Thread _myThread;
        public MusicHandler()
        {
            _myPlayer = new WindowsMediaPlayer();
            _newRnd = new Random();
        }

        public void PlayMusic()
        {
            int x = _newRnd.Next(1, 5);
            string path = String.Format("Music\\{0}.wav", x.ToString());
            _myPlayer.URL = path;
            _myThread = new Thread(_myPlayer.controls.play);
            _myThread.Start();
        }
    }
}

