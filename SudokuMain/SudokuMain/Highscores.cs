using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SudokuMain
{
    class SingleHighscore//En lista som håller en highscore
    {
        private string[,] _content;
        public SingleHighscore(int hsRows) { _content = new string[hsRows, 2]; }//hsRows styr storleken på highscore
        public void setContent(int x, int y, string value) { _content[x, y] = value; }
        public string getContent(int x, int y) { return _content[x, y]; }
    }

    class Highscores
    {
        private List<SingleHighscore> _highscoreList;//Lista med alla highscores för varje bana
        private int _hsSize;//Antal rader per highscore
        private int _nameLength;//Antal bokstäver i namnet

        //Konstruktor
        public Highscores()
        {
            _highscoreList = new List<SingleHighscore>();
            _hsSize = 5;
            _nameLength = 3;
            loadHighscores();
        }

        //Hämtar alla highscores som finns ifrån textfil
        private void loadHighscores()
        {
            string row;

            if (File.Exists("highscore.sdk"))
            {
                StreamReader reader = new StreamReader("highscore.sdk");
                row = reader.ReadLine(); //Läser första raden [x,x] och struntar i denna
                while (row != "")
                {
                    SingleHighscore newHighscore = new SingleHighscore(_hsSize);
                    for (int i = 0; i < _hsSize; i++)
                    {
                        row = reader.ReadLine().Replace(" ", string.Empty);//Tar bort mellanslag
                        newHighscore.setContent(i, 0, row.Substring(0, _nameLength));//Namn 3 bokstäver
                        newHighscore.setContent(i, 1, row.Substring(_nameLength,row.Length-_nameLength));//Resultat
                    }
                    reader.ReadLine();//släng radbytet mellan highscores
                    _highscoreList.Add(newHighscore);
                    row = reader.ReadLine();
                }
                reader.Close();
            }
            else
            {
                StreamWriter writer = new StreamWriter(File.Create("highscore.sdk"));
               //TODO! implementera kod för skrivning
                writer.Close();
                //loadHighscores();
            }
        }

        //Hämtar highscore för den specifika banan.
        //Används för att hämta rätt highscore till rätt bana.
        public string GetHighScore(int index)
        {
            string strHighscore = "";
            for (int i = 0; i < _hsSize; i++)
            {
                strHighscore += (i + 1).ToString() + ". " + _highscoreList[index].getContent(i, 0) + "\t";//namn och tab
                strHighscore += _highscoreList[index].getContent(i, 1) + "\n";//poäng och enter
            }
            return strHighscore;
        }

    }
}
