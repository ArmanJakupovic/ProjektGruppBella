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
            _ammountOfFiles = getAmmountOfFiles();
            createPlaylistFromFiles();
        }

        ///<summary>
        /// Spelar playlistan
        ///</summary>
        public void PlayMusic()
        {
            _myPlayer.controls.play();
        }

        ///<summary>
        /// Stoppar playlistan
        ///</summary>
        public void StopMusic()
        {
            _myPlayer.controls.stop();
            _myPlayer.close();
        }

        ///<summary>
        /// Pausar playlistan
        ///</summary>
        public void PauseMusic()
        {
            _myPlayer.controls.pause();
        }

        ///<summary>
        /// Nästa låt i listan
        ///</summary>
        public void NextSong()
        {
            _myPlayer.controls.next();
        }

        ///<summary>
        /// Som det..inte låter, eller låter
        /// true eller false
        ///</summary>
        public void Mute(bool muteCheck)
        {
            _myPlayer.settings.mute = muteCheck;
        }

        ///<summary>
        /// Aktiverar shuffle mode, true eller false
        ///</summary>
        public void Shuffle(bool shuffle)
        {
            _myPlayer.settings.setMode("shuffle", shuffle);
            NextSong();
        }

        ///<summary>
        /// Loopar igenom den aktiva listan
        ///</summary>
        public void Loop(bool loop)
        {
            _myPlayer.settings.setMode("loop", loop);
            NextSong();
        }

        ///<summary>
        /// Hämtar antal filer i mappen
        ///</summary>
        private int getAmmountOfFiles()
        {
            int nr = 0;
            DirectoryInfo dir = new System.IO.DirectoryInfo("Music\\");
            nr = dir.GetFiles().Length;
            return nr;
        }

        ///<summary>
        /// Skapar en playlista utifrån antal filer i mappen
        /// Gör det möjligt att i efterhand lägga till fler låtar.
        /// Allt bör sköta sig självt.
        ///</summary>
        private void createPlaylistFromFiles()
        {
            IWMPMedia myMedia;
            string path;

            for (int i = 0; i < _ammountOfFiles; i++)
            {
                path = String.Format("Music\\{0}.mp3", i + 1);
                myMedia = _myPlayer.newMedia(path);
                _myPlayer.currentPlaylist.appendItem(myMedia);
            }
        }
    }
}

