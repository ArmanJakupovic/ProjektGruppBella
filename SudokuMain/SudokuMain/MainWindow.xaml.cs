﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.IO;
using System.Windows.Media.Animation;

namespace SudokuMain
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region constans
        SudokuLevels game = new SudokuLevels();//Spelbräda
        Highscores _highscores = new Highscores();//Alla highscores
        Settings _mainSettings = new Settings();//Inställningar för spelet
        Time _time; //Tid
        MainWindow ourWindow;
        private Keypad _x;//Popup fönster
        private int _gridIndexNr;
        private int _lblIndex;
        private CubeWithLabels prevBlockIx;
        private CubeWithLabels cube;
        private int prevIx = 0;
        private Storyboard _myBoard;
        private bool _gameFinished = false;
        private bool _newGame = false;
        private bool _rightClickMemory;
        private bool _leftClickMemory;
        private bool hasCheated = false; //Visar om du har använt hint eller check
        #endregion


        public MainWindow(bool loadGame = false)
        {
            InitializeComponent();
            _time = new Time(ref lblClock);//Initierar klockan
            if (!loadGame)
            {
                game.ReadFromFile();//Läs in alla banor från filen
                game.SetLevel(_mainSettings.getDifficulty(), 0);//Läser in vilken bana som ska spelas
            }
            else
            {
                game.LoadGame(ref _time, ref hasCheated);//Laddar tidagare spel från fil
            }
            _mainSettings.loadSettings();//läser in inställningar från fil
            gameSettings();//tilldelar inställningarna till spelet
            EventManager.RegisterClassHandler(typeof(Window),
            Keyboard.KeyUpEvent, new KeyEventHandler(CubeWithLabels_KeyDown_1), true);
            
            initBoard();//Fyller spelbrädet
            txtHighScore.Text = _highscores.GetHighScore(game.levels[game.currentLevel].difficulty, game.levels[game.currentLevel].level);//fyller highscore för specifik bana
            ourWindow = this;
            initInfoLabel();//Skriver ut vilken svårighetsgrad och bana som spelas
            _time.StartTime();//startar klockan
            _rightClickMemory = false;
            _leftClickMemory = false;
        }

        //Fyller spelplanen med tecken från currentLevel.Unsolved
        private void initBoard()
        {
            currentLvl.Content = "Level: " + (game.levels[game.currentLevel].level+1);
            for (int y = 0; y < 9; y++)
                for (int x = 0; x < 9; x++)
                    updateBoard(y, x, game.levels[game.currentLevel].Unsolved[y, x]);
                
        }

        //Uppdaterar spelbrädet med ett tecken på rätt position
        private void updateBoard(int y, int x, string value)
        {
            int block, ruta;
            block = (y / 3) * 3 + (x / 3);
            ruta = (y % 3) * 3 + (x % 3);
            CubeWithLabels blockIndex = grdBoard.Children[block] as CubeWithLabels;
            blockIndex.SetLabelContent(ruta, value);
        }


        //Anrop: updateMatrix(5,8,"5");
        //Uppdaterar matrisen (unsolved) med ett tecken på rätt position
        private void updateMatrix(int block, int ix, string value)
        {
            int x, y;
            y = 3 * (block / 3) + (ix / 3);
            x = 3 * (block % 3) + (ix % 3);
            //Obs! Matrisen är y,x
            //MessageBox.Show("X = " + x.ToString() + ", Y = " + y.ToString());
            
            if (game.levels[game.currentLevel].Unsolved[y, x].Substring(0,1) != "-")
            {
                game.levels[game.currentLevel].Unsolved[y, x] = value;
                updateBoard(y, x, value);
            }
            checkFinished();
        }

        //Känner av vilken box man klickar i och hämtar rätt ruta från klassen som också läser av mousebutton
        private void grdBoard_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            string myNr = "X";
            _leftClickMemory = true;
            cube = sender as CubeWithLabels;
            
            int indexnr = -1;
            int lblIndex = -1;

            for (int ix = 0; ix < 9; ix++)
            {
                CubeWithLabels cubeCompair = grdBoard.Children[ix] as CubeWithLabels;
                if (cubeCompair == cube)
                {
                    //MessageBox.Show(ix.ToString());
                    indexnr = ix;
                    lblIndex = cube.FindClickedLabel();
                    if (cube.GetBackground(lblIndex) == Brushes.Moccasin)
                    {
                        //Återställer rektangeln om det finns en gammal position lagrad
                        if (prevBlockIx != null)
                            prevBlockIx.setLabelBorder(prevIx, false);
                        //Gör så att den valda labeln blir markerad
                        if (lblIndex >= 0)
                        {
                            cube.setLabelBorder(lblIndex, true);
                            prevBlockIx = cube;
                            prevIx = lblIndex;
                        }
                        _gridIndexNr = indexnr;
                        _lblIndex = lblIndex;
                        if (_mainSettings.getPanel())
                        {
                            openPopup(ref myNr, ref _gridIndexNr, ref _lblIndex, ref ourWindow);
                        }
                    }
                    break;
                }
            }
        }

        //Raderar där markören är vid dubbel högerklick.
        private void grdBoard_RightButtonDown(object sender, MouseButtonEventArgs e)
        {
                cube = sender as CubeWithLabels;
                int indexnr = -1;
                int lblIndex = -1;
                
                for (int ix = 0; ix < 9; ix++)
                {
                    CubeWithLabels cubeCompair = grdBoard.Children[ix] as CubeWithLabels;
                    if (cubeCompair == cube)
                    {
                        indexnr = ix;
                        lblIndex = cube.FindClickedLabel();
                        if (cube.GetBackground(lblIndex) == Brushes.Moccasin)
                        {
                            if (prevBlockIx != null)
                                prevBlockIx.setLabelBorder(prevIx, false);
                            //Gör så att den valda labeln blir markerad
                            if (lblIndex >= 0)
                            {
                                cube.setLabelBorder(lblIndex, true);
                                prevBlockIx = cube;
                                prevIx = lblIndex;
                            }
                        }
                        break;
                    }
                }

            //Nedanstående kod kontrollerar om det är OK att radera vid dubbel högerklick
                if ((_rightClickMemory || _leftClickMemory) && _gridIndexNr == indexnr)
                {
                    if (_lblIndex == lblIndex)
                    {
                        if (_lblIndex >= 0)
                        {
                            updateMatrix(_gridIndexNr, _lblIndex, " ");
                            _rightClickMemory = false;
                            _leftClickMemory = false;
                        }
                    }
                }
                else
                {
                    _rightClickMemory = true;
                }

                if (cube.GetBackground(lblIndex) == Brushes.Moccasin)
                {
                    _gridIndexNr = indexnr;
                    _lblIndex = lblIndex;
                }
        }

        //Kallas på när knapp på keypaden trycks. Uppdaterar gridden.
        public void markedGridPosUpdate(int indexnr, int lblFound , string myStr)
        {
            if (lblFound >= 0)
            {
                updateMatrix(indexnr, lblFound, myStr);
            }
        }

        //Hantering av knappen Check som ska rätta spelplanen
        //Den hämtar en lista med alla positioner som är felaktiga och
        //lägger till ett "/" framför den felaktiga siffran
        private void Button_Check_Click(object sender, RoutedEventArgs e)
        {
            if (!hasCheated)
            {
                string btnClicked = SdkMsgBox.ShowBox("Check for errors is cheating and your time won't be registered. Play as a cheater?",
                            "No...", "Cheating is bad!");
                if (btnClicked == "left")
                    hasCheated = true;
                else
                    return;
            }
            List<int> checkErrors = game.CheckMatch();
            int x, y;
            string valueBefore;

            for (int ix = 0; ix < checkErrors.Count; ix++)
            {
                x = checkErrors[ix] % 9;
                y = checkErrors[ix] / 9;
                valueBefore = game.levels[game.currentLevel].Unsolved[y,x];
                if(valueBefore.Substring(0,1) != "/")
                    game.levels[game.currentLevel].Unsolved[y, x] = "/" + valueBefore;
                updateBoard(y, x, game.levels[game.currentLevel].Unsolved[y, x]);
            }

        }
        
        //Hämtar en korrekt siffra och stoppar in den på en slumpmässigt 
        //vald position som är tom.
        private void Button_Hint_Click(object sender, RoutedEventArgs e)
        {
            if (!hasCheated)
            {
                string btnClicked = SdkMsgBox.ShowBox("This is cheating and your time won't be registered. Play as a cheater?",
                            "No...", "Cheating is bad!");
                if (btnClicked == "left")
                    hasCheated = true;
                else
                    return;
            }

            int fusk = game.GetHint();
            if (fusk >= 0)
            {
                int x = fusk % 9;
                int y = fusk / 9;
                int block = (y / 3) * 3 + (x / 3);
                int ruta = (y % 3) * 3 + (x % 3);
                string value = game.levels[game.currentLevel].Solved[y, x];
                
                //Tar bort fokus från den förra rutan
                if (prevBlockIx != null)
                    prevBlockIx.setLabelBorder(prevIx, false);

                updateMatrix(block, ruta, value);
                
                CubeWithLabels blockIndex = grdBoard.Children[block] as CubeWithLabels;
                if (!_newGame)
                {
                    blockIndex.animateCell(ruta);
                    //Sätter fokus på den nya rutan
                    blockIndex.setLabelBorder(ruta, true);
                }
                else//Hindrar animationen från att dyka upp i nästkommande bana
                    _newGame = false;

                prevBlockIx = blockIndex;
                _gridIndexNr = block;
                prevIx = ruta;
                _lblIndex = ruta;
            }
        }

        //Klick på Inställningar (kugghjulet), laddar in inställningar och utför min animation.
        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            _mainSettings.loadSettings();
            showTimerMain.IsChecked = _mainSettings.getTimer();
            enableHighscoreMain.IsChecked = _mainSettings.getHighscore();
            enablePanelMain.IsChecked = _mainSettings.getPanel();

            _myBoard = (Storyboard)this.Resources["showSettings"];
            _myBoard.Begin();
            btnHint.IsEnabled = false;
            btnCheck.IsEnabled = false;
        }

        //Sparar de nya inställningarna medan spelet är igång, återgå till spelet.
        private void btnSettings_Apply_Click(object sender, RoutedEventArgs e)
        {
            bool time = showTimerMain.IsChecked == true;
            bool score = enableHighscoreMain.IsChecked == true;
            bool panel = enablePanelMain.IsChecked == true;

            _mainSettings.SetTimer(time);
            _mainSettings.SetHighscore(score);
            _mainSettings.SetPopupPanel(panel);
            _mainSettings.saveSettings();

            _mainSettings.loadSettings();
            gameSettings();
            //TODO nya inställningar ska appliceras på klocka, scoreboard och sifferpanelen.
            _myBoard = (Storyboard)this.Resources["hideSettings"];
            _myBoard.Begin();
            btnHint.IsEnabled = true;
            btnCheck.IsEnabled = true;
        }

        //Avbryter menyn Settings och återgår till spelet utan förändring av inställningar
        private void btnSettings_Cancel_Click(object sender, RoutedEventArgs e)
        {
            //TODO trigga animering etc, EJ spara Settings
            _myBoard = (Storyboard)this.Resources["hideSettings"];
            _myBoard.Begin();
            btnHint.IsEnabled = true;
            btnCheck.IsEnabled = true;
        }

        //Visar eller gömmer Klocka, highscore och sifferpanel
        private void gameSettings()
        {
            if (_mainSettings.getTimer())
            {
                lblClock.Visibility = Visibility.Visible;
                btnPause.Visibility = Visibility.Visible;
                gridClockRow.Height = new GridLength('*');
                gridRedArea.Height = new GridLength(80);
            }
            else
            {
                lblClock.Visibility = Visibility.Collapsed;
                btnPause.Visibility = Visibility.Hidden;
                gridClockRow.Height = new GridLength(0);
                gridRedArea.Height = new GridLength(40);
                
            }
            if (_mainSettings.getHighscore())
                grpHighScore.Visibility = Visibility.Visible;
            else grpHighScore.Visibility = Visibility.Collapsed;
            if (!_mainSettings.getPanel())
                keyPad_static.Visibility = Visibility.Visible;
            else keyPad_static.Visibility = Visibility.Collapsed;
        }
        
        /*Kalla på nedanstående metod för att keypaden ska poppa upp.
         Skicka in ref till den sträng du vill ändra. I detta fallet
         borde det vara den sträng som som X skrivs till. Det kommer
         då att skrivas över av en siffra*/
        private void openPopup(ref string myNr, ref int indexNr, ref int lblFound, ref MainWindow thisWin)
        {
            _x = new Keypad(ref myNr, ref indexNr, ref lblFound, ref thisWin);
        }

        //Hanterar klick i keypad på MainWindow
        public void returnNumpadValue(object sender, RoutedEventArgs e)
        {
            Button x = sender as Button;
            markedGridPosUpdate(_gridIndexNr, _lblIndex, x.Content.ToString());
        }

        
        //Låter användaren skriva in siffor med tangentbordet
        //Det går att göra det möjligt att navigera med piltangenterna men det
        //blir komplicerat eftersom MainWindow består av block
        private void CubeWithLabels_KeyDown_1(object sender, KeyEventArgs e)
        {
            string value=" ";

            switch (e.Key)
            {
                case Key.NumPad1:
                case Key.D1:
                    {
                        value = "1";
                        break;
                    }
                case Key.NumPad2:
                case Key.D2:
                    {
                        value = "2";
                        break;
                    }
                case Key.NumPad3:
                case Key.D3:
                    {
                        value = "3";
                        break;
                    }
                case Key.NumPad4:
                case Key.D4:
                    {
                        value = "4";
                        break;
                    }
                case Key.NumPad5:
                case Key.D5:
                    {
                        value = "5";
                        break;
                    }
                case Key.NumPad6:
                case Key.D6:
                    {
                        value = "6";
                        break;
                    }
                case Key.NumPad7:
                case Key.D7:
                    {
                        value = "7";
                        break;
                    }
                case Key.NumPad8:
                case Key.D8:
                    {
                        value = "8";
                        break;
                    }
                case Key.NumPad9:
                case Key.D9:
                    {
                        value = "9";
                        break;
                    }
                case Key.P:
                case Key.Pause:
                    {
                        if(_time.GetDispatcher().IsEnabled)
                            btnPausePlay_Click(this.btnPause, null);
                        else
                            btnPausePlay_Click(this.btnPlay, null);
                        break;
                    }
                default:
                    {
                        value = " ";
                        break;
                    }
                    
            }

            if (_x != null && _x.keypad_Popup.IsOpen)
                _x.keypad_Popup.IsOpen = false;
            
            if (prevBlockIx != null && prevIx >= 0)
            {
                updateMatrix(_gridIndexNr, _lblIndex, value);
            }
        } //CubeWithLabels_KeyDown_1
        
        //Kontrollerar om spelet är färdigspelat eller inte
        private void checkFinished()
        {
            bool isOk = true;
            string chkValue;
            for (int y = 0; y < 9; y++)
                for (int x = 0; x < 9; x++)
                {
                    chkValue = game.levels[game.currentLevel].Unsolved[y, x];
                    if (game.levels[game.currentLevel].Unsolved[y, x].Length > 1)
                        chkValue = game.levels[game.currentLevel].Unsolved[y, x].Substring(1, 1);
                    if (game.levels[game.currentLevel].Solved[y, x] != chkValue)
                    {
                        isOk = false;
                        break;
                    }
                }
            
            if (isOk)
            {
             /*   //Här blir det ett anrop till GameOver-Form eller nåt
                MessageBox.Show("Game Over!", "Den här skylten ska givetvis bytas ut...");*/
                int level = game.levels[game.currentLevel].level;
                int diff = game.levels[game.currentLevel].difficulty;
                _time.StopTime(); //Stannar klockan
                string mess;

                if (hasCheated)
                    mess = "Cheater";
                else
                    mess = "Too slow";

                File.Delete("savedGame.sdk");//Tar bort eventuellt sparat spel
                _gameFinished = true;//Indikerar att spelet är avslutat (kommer inte spara om man trycker X)

                int score = (_time.GetHours() * 3600) + (_time.GetMinutes() * 60) + _time.GetSeconds();

                int placement = _highscores.CompareScore(score, diff, level);//Ev highscore

                if (placement != -1 && !hasCheated)//Om highscore och ej har fuskat
                {
                    string name = SdkMsgBox.showHighScoreBox("You made it to the highscore!", "Highscore!", "Type your name:", "Images\\goodJobFace.png", "Message");
                    _highscores.InsertToHighscore(name.ToUpper().Substring(0, 3), score, diff, level, placement);
                    txtHighScore.Text = _highscores.GetHighScore(diff, level);
                    mess = "Winner!";
                }
                
                    //Visar den nya boxen. 
                    //Det går kalla på en highsScoreBox också med SdkMsgBox.showHighScoreBox.
                    //Den returnerar ett namn istället. 
                    
                    
                    string btnClicked = SdkMsgBox.ShowBox("Play this level again or try next?", "Game over", mess,
                        "Images\\WhyYouNo.png", "Message", "Retry", "Next", true, true);
                    if (btnClicked == "left")
                    {
                        
                    }
                    else
                    {
                        level++;
                        if (level > 4)
                        {
                            level = 0;
                            diff++;
                            if (diff > 2)
                                diff = 0;
                            _mainSettings.setDifficulty(diff);
                        }
                    }
                    //Tömmer arrayen med lösningar
                    for (int y = 0; y < 9; y++)
                        for (int x = 0; x < 9; x++)
                        {
                            if (game.levels[game.currentLevel].Unsolved[y, x].Length > 1)
                            {
                                if (game.levels[game.currentLevel].Unsolved[y, x].Substring(0, 1) == "/")
                                    game.levels[game.currentLevel].Unsolved[y, x] = " ";
                            }
                            else
                                game.levels[game.currentLevel].Unsolved[y, x] = " ";
                        }
                    newGame(diff, level);
            }
        }

        //Hanterar stänging med hjälp av X uppe i högra hörnet
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if(!_gameFinished)//Om spelet är färdigspelat sparas det inte ner
                game.SaveGame(_mainSettings, _time, hasCheated);
            MenuWindow menu = new MenuWindow();
            menu.Show();
            base.OnClosing(e);
        }

        //Förklaring till hur spelet fungerar hamnar här.
        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            string rules = " A sudoku board consists of a 9x9 grid. The grid is divided into 3x3 blocks called regions." 
            + " A region may or may not have cells which are considered unchangable, in our game they are represented by a darker background color."
            + " The purpose of the game is to make sure that each block contains the numbers  1 trough 9. Each cell number must be unique for that specific row and column."
            + "||"
            + " --Click on desired cell with left mouse button to show input menu (if chosen in options). Choose a number.||"
            + " --Double click with right mouse button on desired cell to erase previously chosen number";
            SdkMsgBox.ShowBox(rules.Replace("|", "\n"), "How to", "I know how to play...", "Images\\LearningFace.png", "The rules of Sudoku", "Got it!", "", true, false, 1.8, 1.6);
        }

        //Hämtar information om difficulty och level labels och initierar dem.
        private void initInfoLabel()
        {
            int x = _mainSettings.getDifficulty();
            switch (x)
            {
                case 0:
                    currentDiff.Content = "Difficulty: Beginner";
                    break;
                case 1:
                    currentDiff.Content = "Difficulty: Experienced";
                    break;
                case 2:
                    currentDiff.Content = "Difficulty: Veteran";
                    break;
                default:
                    currentDiff.Content = "ERROR";
                    break;
            }
        }

        //Pausar spelet och startar pausat spel
        private void btnPausePlay_Click(Object sender, RoutedEventArgs args)
        {
            Button button = (Button)sender;
            if (button.Name == "btnPlay" || button.Name == "btnPausePlay")
            {
                _time.StartTime();
                btnPlay.Visibility = Visibility.Hidden;
                btnPause.Visibility = Visibility.Visible;
                btnCheck.IsEnabled = true;
                btnHint.IsEnabled = true;
                keyPad_static.IsEnabled = true;
                _myBoard = (Storyboard)this.Resources["hidePause"];
                _myBoard.Begin();
            }
            else
            {
                _time.StopTime();
                btnPlay.Visibility = Visibility.Visible;
                btnPause.Visibility = Visibility.Hidden;
                btnCheck.IsEnabled = false;
                btnHint.IsEnabled = false;
                keyPad_static.IsEnabled = false;
                _myBoard = (Storyboard)this.Resources["showPause"];
                _myBoard.Begin();
            }
        }

        //Initierar ett nytt spel
        private void newGame(int diff, int level)
        {
            if (game.levels.Count < 5) //Läser in alla banor om det inte redan är gjort
                game.ReadFromFile();
            game.SetLevel(diff, level);
            initBoard();//Fyller spelbrädet
            txtHighScore.Text = _highscores.GetHighScore(game.levels[game.currentLevel].difficulty, game.levels[game.currentLevel].level);//fyller highscore för specifik bana
            ourWindow = this;
            initInfoLabel();//Skriver ut vilken svårighetsgrad och bana som spelas
            _time.setTime(0,0,0);
            hasCheated = false;
            _gameFinished = false;
            _newGame = true;
            _time.StartTime();
        }
    }
}
