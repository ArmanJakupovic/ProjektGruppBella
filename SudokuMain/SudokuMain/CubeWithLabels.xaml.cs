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

namespace SudokuMain
{
    /// <summary>
    /// Interaction logic for CubeWithLabels.xaml
    /// </summary>
    public partial class CubeWithLabels : UserControl
    {
        private int CurrentLabel;

        public CubeWithLabels()
        {
            InitializeComponent();
            CurrentLabel = -1;
        }

        public int FindClickedLabel()
        {
            return CurrentLabel;
        }

        //Hämtar innehållet i den label ix (0-9);
        public string GetLabelContent(int ix)
        {
            UniformGrid uGrd = this.field;
            Grid grd = uGrd.Children[ix] as Grid;
            Label lbl = grd.Children[0] as Label;
            string retValue = lbl.Content.ToString();
            if(retValue.Length>1)
                return retValue.Substring(1,1);
            return retValue;
        }

        //Sätter bakgrunden till mörk om locked=true, annars till ljus
        //Sätter bakgrunden till röd om error=true
        private void setLabelBackground(ref Label lbl, bool locked, bool error=false)
        {
            SolidColorBrush lockedSolidColorBrush = new SolidColorBrush();
            lockedSolidColorBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFD8B087"));
            if (locked)
                lbl.Background = lockedSolidColorBrush;
            else
                lbl.Background = Brushes.Moccasin;
            if (error)
                lbl.Background = Brushes.Red;
        }

        //Skriver innehållet i sträng nr på label ix (0-9)
        //Om nr har ett negativt värde kommer bakgrunden att bli mörk
        public void SetLabelContent(int ix, string nr)
        {
            UniformGrid uGrd = this.field;
            Grid grd = uGrd.Children[ix] as Grid;
            Label lbl = grd.Children[0] as Label;
            string value = nr;
            if (nr.Length > 1)
                value = nr.Substring(1, 1);

            if (nr.Substring(0, 1) == "-")
                setLabelBackground(ref lbl, true);
            else if (nr.Substring(0, 1) == "/")
                setLabelBackground(ref lbl, false, true);
            else
                setLabelBackground(ref lbl, false);
                lbl.Content = value;
        }

        //Registrerar vilken label man klickat på. Main håller reda på vilket block man klickat på
        private void Label_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            Label lblChk = sender as Label;
            UniformGrid uGrd = this.field;

            for (int ix = 0; ix < 9; ix++)
            {
                Grid grd = uGrd.Children[ix] as Grid;
                if (grd.Children[0] == lblChk)
                {
                    //MessageBox.Show("ruta "+ix.ToString());
                    CurrentLabel = ix;
                    break;
                }
            }

        } //Label_MouseLeftButtonDown_1

    }
}