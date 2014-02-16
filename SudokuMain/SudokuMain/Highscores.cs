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
        private List<string> _name = new List<string>();//lista med namn
        private List<int> _score = new List<int>();//lista med poäng
        public void LoadScore(string value, int points) { _name.Add(value); _score.Add(points); }//Ladda in till listorna
        public string GetNameAndScore(int index) { return _name[index] + "\t" + _score[index]; }//Returnerar en sträng med namn och poäng
        public string GetNameAndTime(int index) { return _name[index] + "\t" + (DateTime.Today + new TimeSpan(0, 0, _score[index])).ToString("H:mm:ss"); }//Returnerar en sträng med namn och poäng(tid)
        public void InsertScore(string name, int points, int index) { _name.Insert(index, name); _score.Insert(index, points); }//Placerar en ny person på listan
        public void RemoveLast(int index) { _name.RemoveAt(index); _score.RemoveAt(index); }//Tar bort en person från listan på indexets plats. Ämnat för sista personen
        public List<int> GetPoints() { return _score; } //Returnerar en lista med poäng
    }

    class Highscores
    {
        private SingleHighscore[,] _highscoreList;//Array med alla highscores för varje bana
        private int _diff, _lvl;//Indexerar vilken lista som ska hämtas
        private int _numberOfLvls, _numberOfNames;//antal banor per svårighetsgrad. antal namn per highscore

        //Konstruktor
        public Highscores()
        {
            _numberOfLvls = 5;
            _numberOfNames = 5;
            _highscoreList = new SingleHighscore[3,_numberOfLvls];
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

                    for (int i = 0; i < _numberOfNames; i++)//Lägger fem rader i listan
                    {
                        row = reader.ReadLine();
                        string[] highScoreRow = row.Split(Convert.ToChar(9));//splittar vid TAB
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
                for (int j = 0; j < _numberOfLvls; j++)
                {
                    writer.WriteLine("[" + i + "," + j + "]");//Skriver diff och level
                    for (int ix = 0; ix < _numberOfNames; ix++)
                    {
                        if (_highscoreList[i, j] == null)
                            writer.WriteLine("-\t3600");
                        else
                            writer.WriteLine(_highscoreList[i, j].GetNameAndScore(ix));
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
            for (int i = 0; i < _numberOfNames; i++)
            {
                strHighscore += (i + 1).ToString() + ". " + _highscoreList[diff,lvl].GetNameAndTime(i) + "\n";//namn och tab
            }
            return strHighscore;
        }

        //Jämför poäng för att se om man platsar på listan
        //Omm man platsar läggs man till
        public int CompareScore(int score, int diff, int lvl)
        {
            List<int> prevHighscore = _highscoreList[diff, lvl].GetPoints();//hämtar lista med poäng för specifik bana
            for (int i = 0; i < prevHighscore.Count; i++)
            {
                if (prevHighscore[i] > score)
                {
                    return i;
                }
            }
            return -1;
        } 
 
        //Placerar personen i listan och petar bort den sista personen
        public void InsertToHighscore(string name, int score, int diff, int lvl, int index)
        {
            _highscoreList[diff, lvl].InsertScore(name, score, index);//lägger till 
            _highscoreList[diff, lvl].RemoveLast(_numberOfNames);//petar bort sämsta i listan
            saveHighscores();
        }
    }
}
