using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SudokuMain
{
    class Highscores
    {
        private SingleHighscore[,] _highscoreList;//Array med alla highscores för varje bana
        private int _diff, _lvl;//Indexerar vilken lista som ska hämtas
        private int _numberOfLvls, _numberOfNames;//antal banor per svårighetsgrad. antal namn per highscore
        MySQL_connection _DBConnection = new MySQL_connection();

        //Konstruktor
        public Highscores(bool multiplayer=false, int diff=0, int lvl=0)
        {
            _numberOfLvls = 5;
            _numberOfNames = 5;
            _highscoreList = new SingleHighscore[3,_numberOfLvls];
            _diff = diff;
            _lvl = lvl;
            if (!multiplayer)
                loadHighscoresTxt();
            else
            {
                loadHighscoreDB();
            }
        }

        //Laddar highscore från databasen
        private void loadHighscoreDB()
        { 
            _highscoreList[_diff,_lvl] = _DBConnection.GetHighscore(_diff,_lvl);
            if (_highscoreList[_diff, _lvl] == null)//Om man inte lyckats ladda från databasen.
                loadHighscoresTxt();
        }

        //Spara highscore till databasen
        private bool saveHighscoreDB()
        {
            if (!_DBConnection.InsertToDatabase(_highscoreList[_diff, _lvl], _diff, _lvl))
                return false;
            else
                return true;
        }

        //Hämtar alla highscores som finns ifrån textfil
        private void loadHighscoresTxt()
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
                saveHighscoresTxt();
                loadHighscoresTxt();
            }
        }

        //Skriver alla highscores till textfil
        private void saveHighscoresTxt()
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
                if (i == _numberOfNames - 1)
                {
                    strHighscore += (i + 1).ToString() + ". " + _highscoreList[diff, lvl].GetNameAndTime(i);//namn och tab
                }
                else
                {
                    strHighscore += (i + 1).ToString() + ". " + _highscoreList[diff, lvl].GetNameAndTime(i) + "\n";//namn och tab
                }
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
        public void InsertToHighscoreTxt(string name, int score, int diff, int lvl, int index)
        {
            _highscoreList[diff, lvl].InsertScore(name, score, index);//lägger till 
            _highscoreList[diff, lvl].RemoveLast(_numberOfNames);//petar bort sämsta i listan
            saveHighscoresTxt();
        }

        //Placerar personen i listan och petar bort den sista personen
        public void InsertToHighscoreDB(string name, int score, int diff, int lvl, int index)
        {
            _highscoreList[diff, lvl].InsertScore(name, score, index);//lägger till 
            _highscoreList[diff, lvl].RemoveLast(_numberOfNames);//petar bort sämsta i listan
            saveHighscoreDB();
                //TODO val om skriva till lokal fil.
        }
    }
}
