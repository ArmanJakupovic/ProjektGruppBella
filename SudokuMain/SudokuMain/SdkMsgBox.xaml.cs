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

namespace SudokuMain
{
    /// <summary>
    /// Interaction logic for SdkMsgBox.xaml
    /// </summary>
    public partial class SdkMsgBox : Window
    {
        static string button_id;
        private bool hScoreCheck;
        static string name;
        static SdkMsgBox _myMsgBox;
        public SdkMsgBox()
        {
            InitializeComponent();
            hScoreCheck = false;
        }

        ///<summary>
        /// Ändra endast boxens meddelande.
        ///</summary>
        public static string ShowBox(string txtMessage)
        {
            _myMsgBox = new SdkMsgBox();
            _myMsgBox.Topmost = true;
            _myMsgBox.stackPanBtn.Visibility = Visibility.Visible;
            _myMsgBox.stackPanTxtBox.Visibility = Visibility.Collapsed;
            _myMsgBox.msgBoxTextBlock.Text = txtMessage;
            _myMsgBox.ShowDialog();
            return button_id;
        }

        ///<summary>
        /// Ändra endast boxens meddelande, och sätter ok knapp.
        ///</summary>
        public static string ShowBox(string txtMessage, bool okBtn)
        {
            _myMsgBox = new SdkMsgBox();
            _myMsgBox.Topmost = true;
            _myMsgBox.stackPanBtn.Visibility = Visibility.Visible;
            _myMsgBox.leftBtn.Visibility = Visibility.Collapsed;
            _myMsgBox.rightBtn.Content = "OK";
            _myMsgBox.stackPanTxtBox.Visibility = Visibility.Collapsed;
            _myMsgBox.msgBoxTextBlock.Text = txtMessage;
            _myMsgBox.rightBtn.IsDefault = true;
            _myMsgBox.ShowDialog();
            return button_id;
        }

        ///<summary>
        /// Ändra meddelande och titel
        ///</summary>
        public static string ShowBox(string txtMessage, string txtTitle)
        {
            _myMsgBox = new SdkMsgBox();
            _myMsgBox.Topmost = true;
            _myMsgBox.stackPanBtn.Visibility = Visibility.Visible;
            _myMsgBox.stackPanTxtBox.Visibility = Visibility.Collapsed;
            _myMsgBox.msgBoxTextBlock.Text = txtMessage;
            _myMsgBox.Title = txtTitle;
            _myMsgBox.ShowDialog();
            return button_id;
        }

        ///<summary>
        /// Ändra meddelande, titel och texten under bilden.
        ///</summary>
        public static string ShowBox(string txtMessage, string txtTitle, string txtBelowImage)
        {
            _myMsgBox = new SdkMsgBox();
            _myMsgBox.Topmost = true;
            _myMsgBox.stackPanBtn.Visibility = Visibility.Visible;
            _myMsgBox.stackPanTxtBox.Visibility = Visibility.Collapsed;
            _myMsgBox.msgBoxTextBlock.Text = txtMessage;
            _myMsgBox.Title = txtTitle;
            _myMsgBox.msgBoxBelowImage.Text = txtBelowImage;
            _myMsgBox.ShowDialog();
            return button_id;
        }

        ///<summary>
        /// Ändra meddelande, titel, texten under bilden, bilden i form av Images\\minBild.png
        ///</summary>
        public static string ShowBox(string txtMessage, string txtTitle, string txtBelowImage, string pathToImage)
        {
            _myMsgBox = new SdkMsgBox();
            _myMsgBox.Topmost = true;
            _myMsgBox.stackPanBtn.Visibility = Visibility.Visible;
            _myMsgBox.stackPanTxtBox.Visibility = Visibility.Collapsed;
            _myMsgBox.msgBoxTextBlock.Text = txtMessage;
            _myMsgBox.Title = txtTitle;
            _myMsgBox.msgBoxBelowImage.Text = txtBelowImage;
            _myMsgBox.msgBoxImage.Source = new BitmapImage(new Uri(@pathToImage, UriKind.Relative));
            _myMsgBox.ShowDialog();
            return button_id;
        }

        ///<summary>
        /// Ändra meddelande, titel, texten under bilden, bilden i form av Images\\minBild.png, groupBoxens meddelande, knapparnas innehåll. 
        ///</summary>
        public static string ShowBox(string txtMessage, string txtTitle, string txtBelowImage, string pathToImage, string groupBoxMessage, string txtLeftBtn, string txtRightBtn)
        {
            _myMsgBox = new SdkMsgBox();
            _myMsgBox.Topmost = true;
            _myMsgBox.stackPanBtn.Visibility = Visibility.Visible;
            _myMsgBox.stackPanTxtBox.Visibility = Visibility.Collapsed;
            _myMsgBox.msgBoxTextBlock.Text = txtMessage;
            _myMsgBox.Title = txtTitle;
            _myMsgBox.msgBoxBelowImage.Text = txtBelowImage;
            _myMsgBox.msgBoxImage.Source = new BitmapImage(new Uri(@pathToImage, UriKind.Relative));
            _myMsgBox.msgBoxGroupBox.Header = groupBoxMessage;
            _myMsgBox.leftBtn.Content = txtLeftBtn;
            _myMsgBox.rightBtn.Content = txtRightBtn;
            _myMsgBox.ShowDialog();
            return button_id;
        }

        ///<summary>
        /// Ändra meddelande, titel, texten under bilden, bilden i form av Images\\minBild.png, groupBoxens meddelande, knapparnas innehåll samt hurvida de ska vara aktiva.
        ///</summary>
        public static string ShowBox(string txtMessage, string txtTitle, string txtBelowImage, string pathToImage, string groupBoxMessage, string txtLeftBtn, string txtRightBtn, bool leftButtonActive, bool RightButtonActive)
        {
            _myMsgBox = new SdkMsgBox();
            _myMsgBox.Topmost = true;
            _myMsgBox.stackPanBtn.Visibility = Visibility.Visible;
            _myMsgBox.stackPanTxtBox.Visibility = Visibility.Collapsed;
            _myMsgBox.msgBoxTextBlock.Text = txtMessage;
            _myMsgBox.Title = txtTitle;
            _myMsgBox.msgBoxBelowImage.Text = txtBelowImage;
            _myMsgBox.msgBoxImage.Source = new BitmapImage(new Uri(@pathToImage, UriKind.Relative));
            _myMsgBox.msgBoxGroupBox.Header = groupBoxMessage;
            _myMsgBox.leftBtn.Content = txtLeftBtn;
            _myMsgBox.rightBtn.Content = txtRightBtn;
            if (leftButtonActive)
                _myMsgBox.leftBtn.Visibility = Visibility.Visible;
            else
                _myMsgBox.leftBtn.Visibility = Visibility.Collapsed; ;
            if (RightButtonActive)
                _myMsgBox.rightBtn.Visibility = Visibility.Visible;
            else _myMsgBox.rightBtn.Visibility = Visibility.Collapsed;
            _myMsgBox.ShowDialog();
            return button_id;
        }

        ///<summary>
        /// Ändra meddelande, titel, texten under bilden, bilden i form av Images\\minBild.png, groupBoxens meddelande, knapparnas innehåll samt hurvida de ska vara aktiva,
        /// storleken på fönstret (x * nuvarande storlek). Returnerar "left" eller "right"
        ///</summary>
        public static string ShowBox(string txtMessage, string txtTitle, string txtBelowImage, string pathToImage, string groupBoxMessage, string txtLeftBtn, string txtRightBtn, bool leftButtonActive, bool RightButtonActive, double yHeight, double xWidth)
        {
            _myMsgBox = new SdkMsgBox();
            _myMsgBox.Topmost = true;
            _myMsgBox.stackPanBtn.Visibility = Visibility.Visible;
            _myMsgBox.stackPanTxtBox.Visibility = Visibility.Collapsed;
            _myMsgBox.msgBoxTextBlock.Text = txtMessage;
            _myMsgBox.Title = txtTitle;
            _myMsgBox.msgBoxBelowImage.Text = txtBelowImage;
            _myMsgBox.msgBoxImage.Source = new BitmapImage(new Uri(@pathToImage, UriKind.Relative));
            _myMsgBox.msgBoxGroupBox.Header = groupBoxMessage;
            _myMsgBox.leftBtn.Content = txtLeftBtn;
            _myMsgBox.rightBtn.Content = txtRightBtn;
            if (leftButtonActive)
                _myMsgBox.leftBtn.Visibility = Visibility.Visible;
            else
                _myMsgBox.leftBtn.Visibility = Visibility.Collapsed; ;
            if (RightButtonActive)
                _myMsgBox.rightBtn.Visibility = Visibility.Visible;
            else _myMsgBox.rightBtn.Visibility = Visibility.Collapsed;
            _myMsgBox.Height = _myMsgBox.Height * yHeight;
            _myMsgBox.Width = _myMsgBox.Width * xWidth;
            _myMsgBox.ShowDialog();
            return button_id;
        }

        ///<summary>
        /// Ändra meddelande, titel, texten under bilden, bilden i form av Images\\minBild.png, 
        /// groupBoxens meddelande, returnerar ett det namn som fyllt i boxen. 
        ///</summary>
        public static string showHighScoreBox(string txtMessage, string txtTitle, string txtBelowImage, string pathToImage, string groupBoxMessage)
        {
            _myMsgBox = new SdkMsgBox();
            _myMsgBox.Topmost = true;
            _myMsgBox.hScoreCheck = true;
            _myMsgBox.stackPanBtn.Visibility = Visibility.Collapsed;
            _myMsgBox.stackPanTxtBox.Visibility = Visibility.Visible;
            _myMsgBox.msgBoxTextBlock.Text = txtMessage;
            _myMsgBox.Title = txtTitle;
            _myMsgBox.msgBoxBelowImage.Text = txtBelowImage;
            _myMsgBox.msgBoxImage.Source = new BitmapImage(new Uri(@pathToImage, UriKind.Relative));
            _myMsgBox.msgBoxGroupBox.Header = groupBoxMessage;
            _myMsgBox.stackPanTxtBoxBox.Focus();
            _myMsgBox.rightBtnOk.IsDefault = true;
            _myMsgBox.ShowDialog();
            return name;
        }

        private void btnLeftIsClicked(object sender, EventArgs e)
        {
            button_id = "left";
            this.Close();
        }

        private void btnRightIsClicked(object sender, EventArgs e)
        {
            if (_myMsgBox.hScoreCheck)
            {
                if (_myMsgBox.stackPanTxtBoxBox.Text == "")
                    name = "DRP";
                else
                    name = _myMsgBox.stackPanTxtBoxBox.Text;
                _myMsgBox.hScoreCheck = false;
            }
            else
            {
                button_id = "right";
            }
            this.Close();
        }

        private bool isValid(string sToCheck)
        {
            if (sToCheck.All(Char.IsLetterOrDigit))
            {
                return true;
            }
            else
                return false;
        }


        private void notAllowedKeyDown(object sender, KeyboardEventArgs e)
        {
            if (!stackPanTxtBoxBox.Text.All(Char.IsLetterOrDigit))
            {
                SdkMsgBox.ShowBox("Invalid input! Must use 5 character. Try A-Z, a-z, 0-9.", "WARNING", "Dude, cerealiously!?", "Images\\cerealGuy.png", "Error", "", "I'm sorry", false, true);
                this.stackPanTxtBoxBox.Text = "";
            }
        }

    }
}
