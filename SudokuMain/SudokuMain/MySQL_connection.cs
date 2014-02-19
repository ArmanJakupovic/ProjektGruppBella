using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SudokuMain
{
    class MySQL_connection
    {
        /******************************************************
         * TODO! 
         * Säkerhet i mot databasen.
         * Val om vad man vill göra när man inte kan spara till databasen. Testköra.
         * ****************************************************/
        
        private MySqlConnection _connection;//Anslutningen
        private MySqlCommand _command; //SQL kommando
        private string _query;//SQL querryn
        private MySqlDataReader reader;//Läser resultaten från querries.
        private string _server;
        private string _database;
        private string _uid;
        private string _password;

        //Konstruktor
        public MySQL_connection()
        {
            initialize();
        }

        //Init
        private void initialize()
        {
            _server = "5.150.195.196";
            _database = "jakupovic_gruppBella";
            _uid = "jakupovic_albert";
            _password = "foal1144";
            string connectionString;
            connectionString = "server=" + _server + ";User Id=" + _uid + ";password=" + _password + ";database=" + _database;

            _connection = new MySqlConnection(connectionString);


            //server=82.99.48.146;User Id=jakupovic_admin;password=jaar1186;database=jakupovic_testLo
        }

        //Öppnar anslutningen
        private bool openConnection()
        {
            try
            {
                _connection.Open();
                return true;
            }
            catch (MySqlException ex)//Om det inte går att ansluta
            {
                return false;
            }
        }

        //Stänger anslutningen
        private bool closeConnection()
        {
            try
            {
                _connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                return false;
            }
        }

        //Sätter in ny highscore i databasen
        public bool InsertToDatabase(SingleHighscore list, int diff, int lvl)
        {
            string[] row;

            _query = "DELETE FROM Highscore WHERE Diff='" + diff + "' AND Level='" + lvl + "';";//Tar bort gamla placeringar

            if (openConnection())
            {
                _command = new MySqlCommand(_query, _connection);//Execute query (ovanstående)
                _command.ExecuteNonQuery();

                for (int i = 0; i < list.GetLength(); i++)
                {
                    row = list.GetNameAndScore(i).Split(Convert.ToChar(9));//Hämtar namn och poäng
                    _query = "INSERT INTO Highscore (Name, Score, Diff, Level) VALUES ('" + row[0] + "','" + row[1] + "', '" + diff + "','" + lvl + "');";
                    _command = new MySqlCommand(_query, _connection);
                    _command.ExecuteNonQuery();//Execute query
                }
                closeConnection();
                return true;
            }
            else
                return false;
        }

        //Hämtar highscore och returnerar en lista
        public SingleHighscore GetHighscore(int diff, int lvl)
        {
            SingleHighscore list = new SingleHighscore();
            if (openConnection())
            {
                _query = "SELECT Name,Score FROM Highscore WHERE Diff='" + diff + "' AND Level='" + lvl + "' ORDER BY Score ASC;";
                _command = new MySqlCommand(_query, _connection);
                reader = _command.ExecuteReader();
                while (reader.Read())
                {
                    list.LoadScore(reader.GetString(0), Convert.ToInt16(reader.GetString(1)));
                }
                reader.Close();
                closeConnection();
                return list;
            }
            else
                return null;
            
        }

        //Fyller databasen med standardvärden
        public bool FillTable(int numberOfDiff, int numberOfLvls, int numberOfNames)
        {
            if (openConnection())//Om det går att öppna anslutningen
            {
                for (int i = 0; i < numberOfDiff; i++)
                {
                    for (int j = 0; j < numberOfLvls; j++)
                    {
                        for (int ix = 0; ix < numberOfNames; ix++)
                        {
                            _query = "INSERT INTO Highscore (Name, Score, Diff, Level) VALUES ('" + i.ToString()+j.ToString() + "-','3600', '" + i + "','" + j + "')";
                            _command = new MySqlCommand(_query, _connection);
                            _command.ExecuteNonQuery();
                        }
                    }
                }
                closeConnection();
                return true;
            }
            else
                return false;
        }

        //Rensar databasen
        public bool DeleteTable()
        {
           _query = "DELETE FROM Highscore";

            if (openConnection())
            {
                _command = new MySqlCommand(_query, _connection);
                _command.ExecuteNonQuery();
                closeConnection();
                return true;
            }
            else
                return false;
        }
    }
}

