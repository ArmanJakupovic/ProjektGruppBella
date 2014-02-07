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
            _myMsgBox.stackPanBtn.Visibility = Visibility.Visible;
            _myMsgBox.stackPanTxtBox.Visibility = Visibility.Collapsed;
            _myMsgBox.msgBoxTextBlock.Text = txtMessage;
            _myMsgBox.ShowDialog();
            return button_id;
        }

        ///<summary>
        /// Ändra meddelande och titel
        ///</summary>
        public static string ShowBox(string txtMessage, string txtTitle)
        {
            _myMsgBox = new SdkMsgBox();
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
        public static string ShowBox(string txtMessage, string txtTitle, string txtBelowImage, string pathToImage, string groupBoxMessage, string txtLeftBtn, bool leftButtonActive, string txtRightBtn, bool RightButtonActive)
        {
            _myMsgBox = new SdkMsgBox();
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
        /// storleken på fönstret. Returnerar "left" eller "right"
        ///</summary>
        public static string ShowBox(string txtMessage, string txtTitle, string txtBelowImage, string pathToImage, string groupBoxMessage, string txtLeftBtn, bool leftButtonActive, string txtRightBtn, bool RightButtonActive, int height, int width)
        {
            _myMsgBox = new SdkMsgBox();
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
            _myMsgBox.Height = height;
            _myMsgBox.Width = width;
            _myMsgBox.ShowDialog();
            return button_id;
        }

        ///<summary>
        ///         ///<summary>
        /// Ändra meddelande, titel, texten under bilden, bilden i form av Images\\minBild.png, 
        /// groupBoxens meddelande, returnerar ett det namn som fyllt i boxen. 
        ///</summary>
        public static string showHighScoreBox(string txtMessage, string txtTitle, string txtBelowImage, string pathToImage, string groupBoxMessage)
        {
            _myMsgBox = new SdkMsgBox();
            _myMsgBox.hScoreCheck = true;
            _myMsgBox.stackPanBtn.Visibility = Visibility.Collapsed;
            _myMsgBox.stackPanTxtBox.Visibility = Visibility.Visible;
            _myMsgBox.msgBoxTextBlock.Text = txtMessage;
            _myMsgBox.Title = txtTitle;
            _myMsgBox.msgBoxBelowImage.Text = txtBelowImage;
            _myMsgBox.msgBoxImage.Source = new BitmapImage(new Uri(@pathToImage, UriKind.Relative));
            _myMsgBox.msgBoxGroupBox.Header = groupBoxMessage;
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
                name = _myMsgBox.stackPanTxtBoxBox.Text;
                _myMsgBox.hScoreCheck = false;
            }
            else
            {
                button_id = "right";
            }
            this.Close();
        }
    }
}