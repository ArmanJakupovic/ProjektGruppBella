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
using System.Windows.Media.Animation;

namespace SudokuMain
{
    /// <summary>
    /// Interaction logic for CubeWithLabels.xaml
    /// </summary>
    public partial class CubeWithLabels : UserControl
    {
        private int CurrentLabel;
        private int CurrentNumber;

        public CubeWithLabels()
        {
            InitializeComponent();
            CurrentLabel = -1;
            CurrentNumber = -1;
        }

        public int FindClickedLabel()
        {
            return CurrentLabel;
        }

        public int FindNumber()
        {
            return CurrentNumber;
        }

        //Hämtar innehållet i den label med index ix (0-9);
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

        //Sätter en annan färg på rektangeln runt labeln. Om "active" är true blir den färgad.
        //Är den false blir den normal igen.
        public void setLabelBorder(int ix, bool active)
        {
            UniformGrid uGrd = this.field;
            Grid grd = uGrd.Children[ix] as Grid;
            Label tempLabel = grd.Children[0] as Label;
            findContent(ref tempLabel);
            Rectangle rectActive = grd.Children[1] as Rectangle;

            if (active)
            {
                rectActive.StrokeThickness = 5;
                rectActive.Stroke = new SolidColorBrush(Color.FromRgb(118, 18, 18)); 
            }
            else
            {
                rectActive.StrokeThickness = 1;
                rectActive.Stroke = Brushes.Black;
            }
        }

        //Sätter texten till fet i aktuell label om active=true
        public void setTextProperty(int ix, bool active)
        {
            UniformGrid uGrd = this.field;
            Grid grd = uGrd.Children[ix] as Grid;
            Label lbl = grd.Children[0] as Label;
            if (active)
                lbl.FontWeight = FontWeights.ExtraBlack;
            else
                lbl.FontWeight = FontWeights.Normal;
        }

        //Sätter bakgrunden till mörk om locked=true, annars till ljus
        //Sätter bakgrunden till röd om error=true
        private void setLabelBackground(ref Label lbl, bool locked, bool error=false)
        {
            int boxId = Convert.ToInt16(this.Name.Substring(3, 1));
            SolidColorBrush lockedSolidColorBrush = new SolidColorBrush();
            lockedSolidColorBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFD8B087"));
            SolidColorBrush oddSolidColorBrush = new SolidColorBrush();
            oddSolidColorBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF9D49E"));
            if (locked)
                lbl.Background = lockedSolidColorBrush;
            else
            {
                if (boxId % 2 == 0)
                    lbl.Background = oddSolidColorBrush;
                else
                    lbl.Background = Brushes.Moccasin;
            }
            if (error)
                lbl.Background = Brushes.Tomato;
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

        //Gör en färganimation på vald cell
        public void animateCell(int ix)
        {
            UniformGrid uGrd = this.field;
            Grid grd = uGrd.Children[ix] as Grid;
            Label lbl = grd.Children[0] as Label;
            
            SolidColorBrush myBrush = new SolidColorBrush();
            SolidColorBrush animationColor = new SolidColorBrush();
            animationColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#181816"));

            ColorAnimation cellAnimation = new ColorAnimation();
            cellAnimation.To = animationColor.Color;
                
            cellAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(950));
            cellAnimation.AutoReverse = true;

            myBrush.BeginAnimation(SolidColorBrush.ColorProperty, cellAnimation);
            lbl.Background = myBrush;
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
                    CurrentLabel = ix;
                    findContent(ref lblChk);
                    break;
                }
            }

        } //Label_MouseLeftButtonDown_1

        //Tar reda på vilket nummer som finns i aktuell label
        private void findContent(ref Label lblKontroll)
        {
            int tempNumber = 0;
            bool test = int.TryParse(lblKontroll.Content.ToString(), out tempNumber);
            CurrentNumber = tempNumber;
        }
    }
}
