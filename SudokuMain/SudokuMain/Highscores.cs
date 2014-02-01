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
        private List<string> _name = new List<string>();
        private List<int> _score = new List<int>();
        public void LoadScore(string value, int points) { _name.Add(value); _score.Add(points); }
        public string GetScore(int index) { return _name[index] + "\t" + _score[index].ToString(); }
        public void InsertScore(string name, int points, int index) { _name.Insert(index, name); _score.Insert(index, points); }
        public void RemoveLast(int index) { _name.RemoveAt(index); _score.RemoveAt(index); }
        public List<int> GetPoints() { return _score; }
    }

    class Highscores
    {
        private SingleHighscore[,] _highscoreList;//Array med alla highscores för varje bana
        private int _diff, _lvl;//Indexerar vilken lista som ska hämtas

        //Konstruktor
        public Highscores()
        {
            _highscoreList = new SingleHighscore[3,5];
            _diff = 0;
            _lvl = 0;
            loadHighscores();
        }

        //Hämtar alla highscores som finns ifrån textfil
        private void loadHighscores()
        {
            string row;

            if (File.Exists("highscore.sdk"))
            {
                char[] delimiters = { '[', ',', ']' };
                string[] diffLvl = new string[2];

                StreamReader reader = new StreamReader("highscore.sdk");
                row = reader.ReadLine(); //Läser första raden [diff,level]


                while (row != "")
                {
                    diffLvl = row.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    _diff = Convert.ToInt16(diffLvl[0]);//index för diff
                    _lvl = Convert.ToInt16(diffLvl[1]);//index för level

                    SingleHighscore newHighscore = new SingleHighscore();

                    for (int i = 0; i < 5; i++)//Lägger fem rader i listan
                    {
                        row = reader.ReadLine();
                        string[] highScoreRow = row.Split(Convert.ToChar(32));
                        newHighscore.LoadScore(highScoreRow[0], Convert.ToInt16(highScoreRow[1]));
                    }
                    _highscoreList[_diff, _lvl] = newHighscore;
                    row = reader.ReadLine();//släng radbytet mellan highscores
                    row = reader.ReadLine();//hämtar diff och lvl igen
                }
                reader.Close();
            }
            else
            {
                saveHighscores();
                loadHighscores();
            }
        }

        //Skriver alla highscores till textfil
        private void saveHighscores()
        {
            StreamWriter writer = new StreamWriter(File.Create("highscore.sdk"));

            for (int i = 0; i < 3; i++)
			{
                for (int j = 0; j < 5; j++)
                {
                    writer.WriteLine("[" + i + "," + j + "]");//Skriver diff och level
                    for (int ix = 0; ix < 5; ix++)
                    {
                        if(_highscoreList[i,j] == null)
                            writer.WriteLine("- 0"); 
                        else
                            writer.WriteLine(_highscoreList[i, j].GetScore(ix));
                    }
                    writer.WriteLine();//tom rad
                }
            }
            writer.WriteLine();
            writer.Close();
        }

        //Hämtar highscore för den specifika banan.
        //Används för att hämta rätt highscore till rätt bana.
        public string GetHighScore(int diff, int lvl)
        {
            string strHighscore = "";
            for (int i = 0; i < 5; i++)
            {
                strHighscore += (i + 1).ToString() + ". " + _highscoreList[diff,lvl].GetScore(i) + "\n";//namn och tab
            }
            return strHighscore;
        }

        //Jämför poäng för att se om man platsar på listan
        //Omm man platsar läggs man till
        public void CompareScore(string name, int score, int diff, int lvl)
        {
            List<int> prevHighscore = _highscoreList[diff, lvl].GetPoints();//hämtar lista med poäng för specifik bana
            for (int i = 0; i < prevHighscore.Count; i++)
            {
                if (prevHighscore[i] < score)
                {
                    _highscoreList[diff, lvl].InsertScore(name, score, i);//lägger till 
                    _highscoreList[diff, lvl].RemoveLast(5);//petar bort sämsta i listan
                    break;
                }
            }
        }  
    }
}
