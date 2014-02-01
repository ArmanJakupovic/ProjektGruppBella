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
        private List<string>[,] _highscoreList;//Array med alla highscores för varje bana
        private int _diff, _lvl;//Indexerar vilken lista som ska hämtas

        //Konstruktor
        public Highscores()
        {
            _highscoreList = new List<string>[3,5];
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

                    List<string> newHighscore = new List<string>();

                    for (int i = 0; i < 5; i++)//Lägger fem rader i listan
                    {
                        row = reader.ReadLine();
                        newHighscore.Add(row.Replace(Convert.ToChar(32), Convert.ToChar(9)));//byter mellanslag mot tabb
                    }
                    _highscoreList[_diff, _lvl] = newHighscore;
                    row = reader.ReadLine();//släng radbytet mellan highscores
                    row = reader.ReadLine();//hämtar diff och lvl igen
                }
                reader.Close();
            }
            else
            {
                StreamWriter writer = new StreamWriter(File.Create("highscore.sdk"));

                int diff = 0;
                int level = 0;
                for (int i = 0; i < 15; i++)
                {
                    if (level == 5)
                    {
                        diff++;
                        level = 0;
                    }
                    writer.WriteLine("[" + diff + "," + level + "]");//Skriver diff och level
                    level++;
                  
                    for (int j = 0; j < 5; j++)
                    {
                        writer.WriteLine("-"); 
                    }
                    writer.WriteLine();//tom rad
                }
                writer.WriteLine();
                writer.Close();
                loadHighscores();
            }
        }

        //Hämtar highscore för den specifika banan.
        //Används för att hämta rätt highscore till rätt bana.
        public string GetHighScore(int diff, int lvl)
        {
            string strHighscore = "";
            for (int i = 0; i < 5; i++)
            {
                strHighscore += (i + 1).ToString() + ". " + _highscoreList[diff,lvl][i] + "\n";//namn och tab
            }
            return strHighscore;
        }

    }
}
