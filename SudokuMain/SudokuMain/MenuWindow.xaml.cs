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
using System.Windows.Shapes;
using System.IO;
using System.Windows.Media.Animation;

namespace SudokuMain
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        Settings _set = new Settings(); // inställningar, används vid uppstart och via funktionen btnOptions_Click
        private Storyboard _myBoard;
        public MenuWindow()
        {
            InitializeComponent();
            if (File.Exists("savedGame.sdk"))//Om sparat visa continue
                btnContinue.Visibility = Visibility.Visible;
            else
                btnContinue.Visibility = Visibility.Collapsed;
            gridOptions.Visibility = Visibility.Collapsed;

           //Initierar visibiliteten för de olika menyerna.
            gridButtons.Visibility = Visibility.Visible;
            gridDifficulty.Visibility = Visibility.Collapsed;
            gridOptions.Visibility = Visibility.Collapsed;
            //Slut
        }

        private void btnNewGame_Click(Object sender, RoutedEventArgs args)
        {
            _myBoard = (Storyboard)this.Resources["moveButtonsDownDiffUpp"];
            _myBoard.Begin();
        }

        private void btnContinue_Click(Object sender, RoutedEventArgs args)
        {
            MainWindow game = new MainWindow(true);
            game.Show();
            this.Close();
        }

        private void btnExit_Click(Object sender, RoutedEventArgs args)
        {
            Application.Current.Shutdown();
        }

        //Visar menyn under Options, laddar in inställningar från fil.
        private void btnOptions_Click(object sender, RoutedEventArgs e)
        {
            _set.loadSettings();

         //   gridButtons.Visibility = Visibility.Collapsed;
         //   gridOptions.Visibility = Visibility.Collapsed;

            /*******ANIMATIONER***************/
            _myBoard = (Storyboard)this.Resources["moveButtonsDown"];
            _myBoard.Begin();
            /******SLUT*PÅ*ANIMATIONER*******/

            showTimer.IsChecked = _set.getTimer();
            enableHighscore.IsChecked = _set.getHighscore();
            enablePanel.IsChecked = _set.getPanel();

            int diff = _set.getDifficulty();
            if (diff == 0)
                difficultyBeginner.IsChecked = true;
            else if (diff == 1)
                difficultyExperienced.IsChecked = true;
            else if (diff == 2)
                difficultyVeteran.IsChecked = true;
        }

        //Tillbaka till Menu utan att spara inställningar
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
         //   gridButtons.Visibility = Visibility.Visible;
         //   gridOptions.Visibility = Visibility.Collapsed;

            /*******ANIMATIONER***************/
            _myBoard = (Storyboard)this.Resources["moveButtonsUp"];
            _myBoard.Begin();
            /******SLUT*PÅ*ANIMATIONER*******/

            //TODO Göra så ändringar återställs. Kanske kommer hända automatiskt om vi använder en klass för att ladda in från fil.
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            _myBoard = (Storyboard)this.Resources["moveButtonsUpDiffDown"];
            _myBoard.Begin();
        }

        //Tillbaka till Menu och spara inställningar
        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            bool time = showTimer.IsChecked == true;
            bool score = enableHighscore.IsChecked == true;
            bool panel = enablePanel.IsChecked == true;

            int diff = 0; //Beginner
            if (difficultyExperienced.IsChecked == true)
                diff = 1;
            else if (difficultyVeteran.IsChecked == true)
                diff = 2;

            Settings set = new Settings(time, score, panel, diff);
            set.saveSettings();

        //    gridButtons.Visibility = Visibility.Visible;
        //    gridOptions.Visibility = Visibility.Collapsed;

            /*******ANIMATIONER***************/
            _myBoard = (Storyboard)this.Resources["moveButtonsUp"];
            _myBoard.Begin();
            /******SLUT*PÅ*ANIMATIONER*******/
        }

        //Svårighetsgraden sparas ej utan knapparna startar endast spelet.
        //Detta behöver hanteras.
        private void buttonDiff_Click(object sender, RoutedEventArgs e)
        {
            MainWindow game = new MainWindow();
            game.Show();
            this.Close();

            _myBoard = (Storyboard)this.Resources["moveButtonsUpDiffDown"];
            _myBoard.Begin();
            //this.Hide();
        }
    }
}
