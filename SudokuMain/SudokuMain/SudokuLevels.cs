﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SudokuMain
{
    class SudokuLevels
    {
        public class Level
        {
            public string[,] Unsolved = new string[9, 9];
            public string[,] Solved = new string[9, 9];
            public int difficulty, level;
        }

        public int currentLevel;
        public int currentDifficulty;
        public List<Level> levels;

        public SudokuLevels()
        {
            currentLevel = 0;
            currentDifficulty = 0;
            levels = new List<Level>();
            levelZero();                //Hämtar en bana som finns ifall filen med levels saknas
        }

        //Begär en ny bana med diff=0-2 och nr=0-4
        //Om banan inte hittas sätts aktuell bana till 0 och svårighetsgrad = 0
        //Dessa finns alltid eftersom de inte läses från fil
        public void SetLevel(int diff, int nr)
        {
            Level retLevel = levels.FirstOrDefault(x => x.difficulty == diff && x.level == nr);
            if (retLevel != null)
            {
                currentLevel = levels.FindIndex(x => x.difficulty == diff && x.level == nr);
                currentDifficulty = diff;
            }
            else
            {
                currentLevel = 0;
                currentDifficulty = 0;
            }

        } //SetLevel

        //Sparar ner spelet och dess lösning, settings och tid.
        public void SaveGame(Settings _mainSettings, Time _time, bool hasCheated)
        {
            StreamWriter writer = new StreamWriter(File.Create("savedGame.sdk"));
            string unsolved = string.Empty;
            string solved = string.Empty;

            //Skriver ner time
            writer.WriteLine(_time.GetSeconds());
            writer.WriteLine(_time.GetMinutes());
            writer.WriteLine(_time.GetHours());
            writer.WriteLine();//tomrad

            //Fuskat eller inte
            writer.WriteLine(hasCheated);
            writer.WriteLine();//tomrad

            //Skriver diff,level
            //writer.WriteLine("[" + _mainSettings.getDifficulty().ToString() + "," + levels[currentLevel].level.ToString() + "]");
            writer.WriteLine("[" + levels[currentLevel].difficulty + "," + levels[currentLevel].level.ToString() + "]");

            //Skriver ner nuvarande spel
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if (levels[currentLevel].Unsolved[y, x].ToString() == " ")
                        unsolved += ".";
                    else
                        unsolved += levels[currentLevel].Unsolved[y, x].ToString();
                }
                writer.WriteLine(unsolved);
                unsolved = string.Empty;
            }
            writer.WriteLine();//Tom rad

            //Skriver ner lösningen
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    solved += levels[currentLevel].Solved[y, x].ToString();
                }
                writer.WriteLine(solved);
                solved = string.Empty;
            }
            writer.WriteLine();//Tom rad
            writer.Close();
        }

        //Laddar tidigare spel
        public void LoadGame(ref Time clock, ref bool hasCheated)
        {
            char[] delimiters = new char[] { '[', ']', ',' }; //För att ta bort oönskade tecken
            string[] info = new string[2];

            try
            {
                StreamReader loadStream = new StreamReader("savedGame.sdk");

                clock.SetSeconds(Convert.ToInt16( loadStream.ReadLine() ) );
                clock.SetMinutes(Convert.ToInt16( loadStream.ReadLine() ) );
                clock.SetHours(Convert.ToInt16( loadStream.ReadLine() ) );
                loadStream.ReadLine();//tomrad

                //Fuskat eller inte
                hasCheated = Convert.ToBoolean(loadStream.ReadLine());
                loadStream.ReadLine();//tomrad

                string line = loadStream.ReadLine();
                Level newLevel = new Level();
                string[] parts = line.Split(delimiters,
                StringSplitOptions.RemoveEmptyEntries);

                newLevel.difficulty = Convert.ToInt16(parts[0]);
                newLevel.level = Convert.ToInt16(parts[1]);

                //Läser in spelplanen
                for (int y = 0; y < 9; y++)
                {
                    int extraIndex = 0;
                    line = loadStream.ReadLine();
                    for (int x = 0; x < line.Length; x++)
                    {
                        if (char.IsDigit(line[x]))//Om egen tillagd siffre
                            newLevel.Unsolved[y, x + extraIndex] = line[x].ToString();
                        else if (line[x] == '-')//Om fast siffra
                        {
                            newLevel.Unsolved[y, x + extraIndex] = "-" + line[++x].ToString();
                            extraIndex--;
                        }
                        else if (line[x] == '/')//Om felaktig siffra
                        {
                            newLevel.Unsolved[y, x + extraIndex] = "/" + line[++x].ToString();
                            extraIndex--;
                        }
                        else//Tom ruta
                            newLevel.Unsolved[y, x + extraIndex] = " ";
                    }
                }

                loadStream.ReadLine(); //tomrad

                //Läser in den korrekta lösningen
                for (int y = 0; y < 9; y++)
                {
                    line = loadStream.ReadLine();
                    for (int x = 0; x < 9; x++)
                    {
                        newLevel.Solved[y, x] = line[x].ToString();
                    }
                }
                currentDifficulty = newLevel.difficulty;
                levels.Add(newLevel);
                loadStream.Close();
                currentLevel = 1;
            }
            catch (IOException e)
            {
                SdkMsgBox.ShowBox(e.Message + "\nError reading saved game.", "Error!", "File error...", "Images\\sPanda.png", "Oh no..", "", "OK", false, true);
            }
        }//laddar spel från textfil

        //Läser in alla nivåer från fil och lägger dessa i en lista
        public void ReadFromFile()
        {
            char[] delimiters = new char[] { '[', ']', ',' }; //För att ta bort oönskade tecken
            string[] info = new string[2];

            try
            {
                StreamReader inStream = new StreamReader(@"SudokuLevels.sdk");

                string line = inStream.ReadLine();

                while (line != null)
                {
                    Level newLevel = new Level();
                    string[] parts = line.Split(delimiters,
                     StringSplitOptions.RemoveEmptyEntries);

                    newLevel.difficulty = Convert.ToInt16(parts[0]);
                    newLevel.level = Convert.ToInt16(parts[1]);

                    //Läser in spelplanen
                    for (int y = 0; y < 9; y++)
                    {
                        line = inStream.ReadLine();
                        for (int x = 0; x < 9; x++)
                        {
                            if (char.IsDigit(line[x]))
                                newLevel.Unsolved[y, x] = "-" + line[x].ToString();
                            else
                                newLevel.Unsolved[y, x] = " ";
                        }
                    }

                    string slask = inStream.ReadLine(); //tomrad

                    //Läser in den korrekta lösningen
                    for (int y = 0; y < 9; y++)
                    {
                        line = inStream.ReadLine();
                        for (int x = 0; x < 9; x++)
                        {
                            if (char.IsDigit(line[x]))
                                newLevel.Solved[y, x] = line[x].ToString();
                            else
                                newLevel.Solved[y, x] = " ";
                        }
                    }

                    levels.Add(newLevel);
                    line = inStream.ReadLine(); //tomrad
                    line = inStream.ReadLine();
                }
                inStream.Close();
            }
            catch (IOException e)
            { 
                SdkMsgBox.ShowBox(e.Message + "\nYou will only be able to play one level.", "Error!", "File error...", "Images\\sPanda.png", "Oh no..", "", "OK", false, true);
            }
        } //readFromFile


        //En lokal bana som alltid finns med i spelet
        private void levelZero()
        {
            Level levelZero = new Level();
            levelZero.difficulty = 0;
            levelZero.level = 0;
            levelZero.Unsolved = new string[,]{
                {" ", " ", " ", " ", "-4", " ", "-2", "-9", " "},
                {"-6", "-9", "-2", " ", "-8", " ", " ", " ", " "},
                {" ", "-4", "-8", " ", " ", "-2", "-1", "-3", " "},
                {" ", " ", "-7", "-2", " ", "-6", " ", "-1", " "},
                {"-2", " ", "-1", " ", " ", "-4", "-7", " ", "-5"},
                {"-5", "-6", " ", " ", " ", " ", " ", " ", "-4"},
                {" ", "-7", "-6", " ", " ", " ", " ", "-8", " "},
                {" ", "-1", "-4", " ", " ", " ", "-9", "-7", " "},
                {" ", "-2", " ", "-8", " ", "-1", " ", " ", " "}
                };

            levelZero.Solved = new string[,]{
                {"1", "5", "3", "6", "4", "7", "2", "9", "8"},
                {"6", "9", "2", "1", "8", "3", "4", "5", "7"},
                {"7", "4", "8", "5", "9", "2", "1", "3", "6"},
                {"4", "3", "7", "2", "5", "6", "8", "1", "9"},
                {"2", "8", "1", "9", "3", "4", "7", "6", "5"},
                {"5", "6", "9", "7", "1", "8", "3", "2", "4"},
                {"3", "7", "6", "4", "2", "9", "5", "8", "1"},
                {"8", "1", "4", "3", "6", "5", "9", "7", "2"},
                {"9", "2", "5", "8", "7", "1", "6", "4", "3"}
                };

            levels.Add(levelZero);
        } //levelZero


        //Skapar en lista med index på alla positioner som är felaktiga
        public List<int> CheckMatch()
        {
            List<int> errorList = new List<int>();
            string nr;
            int index = 0;

            for (int y = 0; y < 9; y++)
                for (int x = 0; x < 9; x++)
                {
                    nr = levels[currentLevel].Unsolved[y, x];
                    if (levels[currentLevel].Unsolved[y, x].Length > 1)
                        nr = levels[currentLevel].Unsolved[y, x].Substring(1, 1);
                    if (nr != " " && nr != levels[currentLevel].Solved[y, x])
                        errorList.Add(index);
                    index++;
                }
            return errorList;
        } //CheckMatch

        //Returnerar en (slumpvald) position i matrisen som inte är satt
        public int GetHint()
        {
            Random rnd = new Random();
            bool hintExist = false;
            List<int> hintList = new List<int>();
            int pos = 0;
            int retValue = -1;

            //Skapar en lista med möjliga tips
            for (int y = 0; y < 9; y++)
                for (int x = 0; x < 9; x++)
                {
                    if (levels[currentLevel].Unsolved[y, x] == " ")
                    {
                        hintExist = true;
                        hintList.Add(pos);
                    }
                    pos++;
                }

            if (hintExist)
                retValue = hintList[rnd.Next(0, hintList.Count)];

            return retValue;
        } //GetHint

        //Returnerar true om aktuell siffra är ändringsbar
        public bool NumberIsChangeable(int gridIx, int lblIx)
        {
            if (levels[currentLevel].Unsolved[(3 * (gridIx / 3) + (lblIx / 3)), (3 * (gridIx % 3) + (lblIx % 3))].Substring(0, 1) == "-")
                return false;
            return true;
        }

        //Tömmer arrayen med lösningar i det aktuella spelet
        public void ClearCurrentSolution()
        {
            for (int y = 0; y < 9; y++)
                for (int x = 0; x < 9; x++)
                {
                    if (levels[currentLevel].Unsolved[y, x].Length > 1)
                    {
                        if (levels[currentLevel].Unsolved[y, x].Substring(0, 1) == "/")
                            levels[currentLevel].Unsolved[y, x] = " ";
                    }
                    else
                        levels[currentLevel].Unsolved[y, x] = " ";
                }
        }
    }
}
