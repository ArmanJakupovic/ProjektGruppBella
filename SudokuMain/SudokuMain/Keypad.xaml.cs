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

namespace SudokuMain
{
    /// <summary>
    /// Interaction logic for Keypad.xaml
    /// </summary>
    public partial class Keypad : UserControl
    {
        private string _myLbl;
        private bool _check;
        private int _indexNr;
        private int _lblFound;
        private MainWindow x;
        public Keypad(ref string lbl, ref int indexNr, ref int lblFound, ref MainWindow testWin)
        {
            InitializeComponent();
            _myLbl = lbl;
            _indexNr = indexNr;
            _lblFound = lblFound;
            x = testWin;
            keypad_Popup.IsOpen = true;
            _check = false;
        }

        public void returnNumpadValue(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            _myLbl = btn.Content.ToString();
            keypad_Popup.IsOpen = false;
            x.markedGridPosUpdate(_indexNr, _lblFound, _myLbl);
            _check = true;
            valueChoosen();
        }

        public bool valueChoosen()
        {
            return _check;
        }
    }
}
