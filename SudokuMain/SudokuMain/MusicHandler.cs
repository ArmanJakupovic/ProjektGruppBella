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
    public class MusicHandler
    {
        #region constans
        private Random _newRnd;
        private WindowsMediaPlayer _myPlayer;
        private IWMPPlaylist _playlist;
        private int _ammountOfFiles;
        #endregion

        public MusicHandler()
        {
            _myPlayer = new WindowsMediaPlayer();
            _newRnd = new Random();
            _playlist = _myPlayer.playlistCollection.newPlaylist("arman");
            _ammountOfFiles = getAmmountOfFiles();
            createPlaylistFromFiles();
        }

        //Startar spelaren.
        public void PlayMusic()
        {
            _myPlayer.settings.setMode("shuffle", true);
            _myPlayer.controls.play();
        }

        public void NextSong()
        {
            _myPlayer.controls.next();
        }

        private int getAmmountOfFiles()
        {
            int nr = 0;
            DirectoryInfo dir = new System.IO.DirectoryInfo("Music\\");
            nr = dir.GetFiles().Length;
            return nr;
        }

        private void createPlaylistFromFiles()
        {
            IWMPMedia myMedia;
            string path;
            for (int i = 0; i < _ammountOfFiles; i++)
            {
                path = String.Format("Music\\{0}.mp3", i + 1);
                myMedia = _myPlayer.newMedia(path);
                _playlist.appendItem(myMedia);
            }
            _myPlayer.currentPlaylist = _playlist;
        }
    }
}

