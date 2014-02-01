using System;
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
        SudokuLevels game = new SudokuLevels();
        Highscores _highscores = new Highscores();
        MainWindow ourWindow;
        private Keypad _x;
        private int _gridIndexNr;
        private int _lblFound;
        private CubeWithLabels prevBlockIx;
        private int prevIx = 0;

        


        public MainWindow()
        {
            InitializeComponent();
            EventManager.RegisterClassHandler(typeof(Window),
            Keyboard.KeyUpEvent, new KeyEventHandler(CubeWithLabels_KeyDown_1), true);
            game.SetLevel(0, 3);
            initBoard();
            txtHighScore.Text =  _highscores.GetHighScore(0,3);
            settingButtonsActivation(false);
            ourWindow = this;
        }

        //Fyller spelplanen med tecken från currentLevel.Unsolved
        private void initBoard()
        {
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
        }

        //Känner av vilken box man klickar i och hämtar rätt ruta från klassen som också läser av mousebutton
        private void grdBoard_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            string myNr = "X";
            CubeWithLabels cube = sender as CubeWithLabels;

            int indexnr = -1;
            int lblFound = -1;

            //Återställer rektangeln om det finns en gammal position lagrad
            if (prevBlockIx != null)
                prevBlockIx.setLabelBorder(prevIx, false);
            
            for (int ix = 0; ix < 9; ix++)
            {
                CubeWithLabels cubeCompair = grdBoard.Children[ix] as CubeWithLabels;
                if (cubeCompair == cube)
                {
                    //MessageBox.Show(ix.ToString());
                    indexnr = ix;
                    lblFound = cube.FindClickedLabel();
                    //Gör så att den valda labeln blir markerad
                    if (lblFound >= 0)
                    {
                        cube.setLabelBorder(lblFound, true);
                        prevBlockIx = cube;
                        prevIx = lblFound;
                    }
                    break;
                }
            }

            _gridIndexNr = indexnr;
            _lblFound = lblFound;
            openPopup(ref myNr, ref _gridIndexNr, ref _lblFound, ref ourWindow);

            
    /*        if (lblFound >= 0)
            {
                string nummer = cube.GetLabelContent(lblFound);
                //MessageBox.Show("Ruta: " + indexnr.ToString() + ", Label: " + lblFound.ToString() + ", Innehåll: " + nummer);
                updateMatrix(indexnr, lblFound, myNr);
            }*/
        }

        //Kallas på när knapp på keypaden trycks. Uppdaterar gridden.
        public void markedGridPosUpdate(int indexnr, int lblFound , string myStr)
        {
            if (lblFound >= 0)
            {
                updateMatrix(indexnr, lblFound, myStr);
            }
        }
        //Sparar ner spelet och dess lösning
        private void saveGame()
        {
            StreamWriter writer = new StreamWriter(File.Create("savedGame.sdk"));
            writer.WriteLine("[" + game.levels[game.currentDifficulty].difficulty.ToString() + "," + game.levels[game.currentLevel].level.ToString() + "]");
            string unsolved = string.Empty;
            string solved = string.Empty;

            //Skriver ner nuvarande spel
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if (game.levels[game.currentLevel].Unsolved[y, x].ToString() == " ")
                        unsolved += ".";
                    else if (game.levels[game.currentLevel].Unsolved[y, x].Substring(0, 1) == "-")
                        unsolved += game.levels[game.currentLevel].Unsolved[y, x].Substring(1, 1);
                    else
                        unsolved += game.levels[game.currentLevel].Unsolved[y, x].ToString();
                }
                writer.WriteLine(unsolved);
                unsolved = string.Empty;
            }
            writer.WriteLine();//Tom rad

            //Skriver ner lösningen
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    solved += game.levels[game.currentLevel].Solved[y, x].ToString();
                }
                writer.WriteLine(solved);
                solved = string.Empty;
            }
            writer.Close();
        }

        //Hantering av knappen Check som ska rätta spelplanen
        //Den hämtar en lista med alla positioner som är felaktiga och
        //lägger till ett "/" framför den felaktiga siffran
        private void Button_Check_Click(object sender, RoutedEventArgs e)
        {
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

        private void Button_Hint_Click(object sender, RoutedEventArgs e)
        {
            saveGame();
            Storyboard myBoard;
            myBoard = (Storyboard)this.Resources["showSettings"];
            myBoard.Begin();
            btnHint.IsEnabled = false;
            btnCheck.IsEnabled = false;
            settingButtonsActivation(true);

            //Nedan är koden som ska vara till denna knapp egentligen
            //Möjligen ska det visas tydligare att man får en hint
            int fusk = game.GetHint();
            if (fusk >= 0)
            {
                int x = fusk % 9;
                int y = fusk / 9;
                int block = (y / 3) * 3 + (x / 3);
                int ruta = (y % 3) * 3 + (x % 3);
                string value = game.levels[game.currentLevel].Solved[y, x];
                updateMatrix(block, ruta, value);
            }
        }

        private void Button_Close_Settings(object sender, RoutedEventArgs e)
        {
            Storyboard myBoard;
            myBoard = (Storyboard)this.Resources["hideSettings"];
            myBoard.Begin();
            btnHint.IsEnabled = true;
            btnCheck.IsEnabled = true;
            settingButtonsActivation(false);
        }

        //Aktiverar/Deaktiverar alla knappar i Mainwindows settings.
        private void settingButtonsActivation(bool x)
        {
            btnSetting1.IsEnabled = x;
            btnSetting2.IsEnabled = x;
            btnSetting3.IsEnabled = x;
            btnBack.IsEnabled = x;
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
            markedGridPosUpdate(_gridIndexNr, _lblFound, x.Content.ToString());
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
                
                default:
                    {
                        value = " ";
                        break;
                    }
                    
            }
            
            if (prevBlockIx != null && prevIx >= 0)
            {
                updateMatrix(_gridIndexNr, _lblFound, value);
            }
        } //CubeWithLabels_KeyDown_1
    }
}
