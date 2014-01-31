using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SudokuMain
{
    class SingleHighscore
    {
        private string[,] _content = new string[5, 2];
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
                    SingleHighscore newHighscore = new SingleHighscore();
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
                //TODO implementerar standard highscores
                writer.Close();
            }
        }

        //Hämtar highscore för den specifika banan.
        //Används för att hämta rätt highscore till rätt bana.
        public string GetHighScore(int index)
        {
            string strHighscore = "";
            for (int i = 0; i < _hsSize; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    strHighscore += _highscoreList[index].getContent(i, j) + "\t";//namn och poäng med tab emellan
                }
                strHighscore += "\n";//byter rad för nästa namn
            }
            return strHighscore;
        }

    }
}
