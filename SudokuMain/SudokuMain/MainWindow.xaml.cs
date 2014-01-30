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

        public MainWindow()
        {
            InitializeComponent();
            game.SetLevel(0, 2);
            initBoard();
            getHighScore();
            saveGame();
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
            CubeWithLabels cube = sender as CubeWithLabels;
            int indexnr = -1;
            int lblFound = -1;
            for (int ix = 0; ix < 9; ix++)
            {
                CubeWithLabels cubeCompair = grdBoard.Children[ix] as CubeWithLabels;
                if (cubeCompair == cube)
                {
                    //MessageBox.Show(ix.ToString());
                    indexnr = ix;
                    lblFound = cube.FindClickedLabel();
                    break;
                }
            }

            string nummer = cube.GetLabelContent(lblFound);
            //MessageBox.Show("Ruta: " + indexnr.ToString() + ", Label: " + lblFound.ToString() + ", Innehåll: " + nummer);
            updateMatrix(indexnr, lblFound, "X");
        }

        //Hämtar highscore för den specifika banan
        private void getHighScore()
        {
            if (File.Exists("highscore.sdk"))
            {
                StreamReader reader = new StreamReader("highscore.sdk");
                txtHighScore.Text = reader.ReadToEnd();
                reader.Close();
            }
            else
                File.CreateText("highscore.sdk");
        }

        //Sparar ner spelet och dess lösning och settings
        private void saveGame()
        {
            StreamWriter writer = new StreamWriter(File.Create("savedGame.sdk"));
            string unsolved = string.Empty;
            string solved = string.Empty;
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                { 
                    if(game.levels[game.currentLevel].Unsolved[y, x].ToString() == null)
                        unsolved += " ";
                    else
                        unsolved += game.levels[game.currentLevel].Unsolved[y, x].ToString();

                    solved += game.levels[game.currentLevel].Solved[y, x].ToString();
                }
            }
            writer.WriteLine(unsolved);
            writer.WriteLine(solved);
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
            Storyboard myBoard;
                myBoard = (Storyboard)this.Resources["showSettings"];
                myBoard.Begin();

        }

        private void Button_Close_Settings(object sender, RoutedEventArgs e)
        {
            Storyboard myBoard;
                myBoard = (Storyboard)this.Resources["hideSettings"];
                myBoard.Begin();
        }

    }
}
