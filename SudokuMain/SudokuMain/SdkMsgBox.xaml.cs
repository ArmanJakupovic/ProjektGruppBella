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
        static SdkMsgBox _myMsgBox;
        public SdkMsgBox()
        {
            InitializeComponent();
        }

        ///<summary>
        /// Ändra endast boxens meddelande.
        ///</summary>
        public static string ShowBox(string txtMessage)
        {
            _myMsgBox = new SdkMsgBox();
            _myMsgBox.msgBoxTextBlock.Text = txtMessage;
            _myMsgBox.ShowDialog();
            return button_id;
        }

        ///<summary>
        /// Här kan allt ändras. Om bildens önskas att ändras bör det ske i formatet Images\\minBild.png
        ///</summary>
        public static string ShowBox(string txtMessage, string txtTitle, string txtBelowImage, string pathToImage, string txtLeftBtn, string txtRightBtn)
        {
            _myMsgBox = new SdkMsgBox();
            _myMsgBox.msgBoxTextBlock.Text = txtMessage;
            _myMsgBox.Title = txtTitle;
            _myMsgBox.msgBoxBelowImage.Text = txtBelowImage;
            _myMsgBox.msgBoxImage.Source = new BitmapImage(new Uri(@pathToImage, UriKind.Relative));
            _myMsgBox.leftBtn.Content = txtLeftBtn;
            _myMsgBox.rightBtn.Content = txtRightBtn;
            _myMsgBox.ShowDialog();
            return button_id;
        }

        private void btnLeftIsClicked(object sender, EventArgs e)
        {
            button_id = "left";
            this.Close();
        }

        private void btnRightIsClicked(object sender, EventArgs e)
        {
            button_id = "right";
            this.Close();
        }
    }
}
