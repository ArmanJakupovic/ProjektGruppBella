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

namespace SudokuMain
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
            if (File.Exists("savedGame.sdk"))//Om sparat visa continue
                btnContinue.Visibility = Visibility.Visible;
            else
                btnContinue.Visibility = Visibility.Collapsed;
        }

        private void btnNewGame_Click(Object sender, RoutedEventArgs args)
        {
            MainWindow game = new MainWindow();
            game.Show();
            //this.Hide();
        }

        private void btnContinue_Click(Object sender, RoutedEventArgs args)
        {
            MainWindow game = new MainWindow();
            game.Show();
        }

        private void btnExit_Click(Object sender, RoutedEventArgs args)
        {
            Application.Current.Shutdown();
        }
    }
}
