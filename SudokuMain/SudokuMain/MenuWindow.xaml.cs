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
            if (_set.getOnline() == true)
                btnMulti.Content = "ON";
            else btnMulti.Content = "OFF";

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

        //Startar ett nytt spel med nollställd tid
        //och de inställningar man eventuellt ändrar till via Options.
        private void btnNewGame_Click(Object sender, RoutedEventArgs args)
        {
            _myBoard = (Storyboard)this.Resources["moveButtonsDownDiffUpp"];
            _myBoard.Begin();
        }

        //Fortsätter föregående spel INKLUSIVE dess inställningar,
        //alltså kommer inte ändringar man gör i MenuWindow Options
        //verka på spelet man återupptar, ändringar får göras under spelets gång.
        private void btnContinue_Click(Object sender, RoutedEventArgs args)
        {
            _set.saveSettings();
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
            showTimer.IsChecked       = _set.getTimer();
            enableHighscore.IsChecked = _set.getHighscore();
            enablePanel.IsChecked     = _set.getPanel();
            enableMusic.IsChecked     = _set.getMusic();

            /*******ANIMATIONER***************/
            _myBoard = (Storyboard)this.Resources["moveButtonsDown"];
            _myBoard.Begin();
            /******SLUT*PÅ*ANIMATIONER*******/

        }

        //Tillbaka till Menu utan att spara inställningar
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
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
            _set.SetTimer(showTimer.IsChecked == true);
            _set.SetHighscore(enableHighscore.IsChecked == true);
            _set.SetPopupPanel(enablePanel.IsChecked == true);
            _set.SetMusic(enableMusic.IsChecked == true);
            _set.saveSettings();

            /*******ANIMATIONER***************/
            _myBoard = (Storyboard)this.Resources["moveButtonsUp"];
            _myBoard.Begin();
            /******SLUT*PÅ*ANIMATIONER*******/
        }

        //Uppdaterar inställningen "difficulty" och startar ett nytt spel.
        private void buttonDiff_Click(object sender, RoutedEventArgs e)
        {
            Button btnDiff = (Button)sender;
            if (btnDiff.Content.ToString() == "Beginner")
                _set.setDifficulty(0);
            if (btnDiff.Content.ToString() == "Experienced")
                _set.setDifficulty(1);
            if(btnDiff.Content.ToString() == "Veteran")
                _set.setDifficulty(2);
            _set.saveSettings();

            MainWindow game = new MainWindow(false);
            game.Show();
            this.Close();

            _myBoard = (Storyboard)this.Resources["moveButtonsUpDiffDown"];
            _myBoard.Begin();
            //this.Hide();
        }

        //Knapp som togglar cloudhighscore av eller på
        private void btnCloudHighscoreClicked(object sender, RoutedEventArgs e)
        {
            Button myButton = (Button)sender;

            if (myButton.Content.ToString() == "OFF")
            {
                myButton.Content = "ON";
                _set.SetOnline(true);
            }
            else
            {
                myButton.Content = "OFF";
                _set.SetOnline(false);
            }
        }
    }
}
