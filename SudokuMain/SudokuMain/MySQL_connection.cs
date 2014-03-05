using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace SudokuMain
{
    class MySQL_connection
    {        
        private MySqlConnection _connection;//Anslutningen
        private MySqlCommand _command; //SQL kommando
        private string _query;//SQL querryn
        private MySqlDataReader reader;//Läser resultaten från querries.

        //Konstruktor
        public MySQL_connection()
        {
            EncryptConnectionString(System.AppDomain.CurrentDomain.FriendlyName);//SudokuMain.vshost.exe
            EncryptConnectionString(System.Reflection.Assembly.GetExecutingAssembly().ManifestModule.Name);//SudokuMain.exe
            _connection = new MySqlConnection(Properties.Settings.Default.ConnectionString);
        }

        //Krypterar anslutningen i 
        private void EncryptConnectionString(string exeConfigName)
        {
            //Hämtar den den app som exekverar och använder dess config
            try
            {
                // Öppnar och hämtar konfigurationsdelen för den app som exekverar
                Configuration config = ConfigurationManager.OpenExeConfiguration(exeConfigName);

                ConnectionStringsSection section = config.GetSection("connectionStrings") as ConnectionStringsSection;

                if (!section.SectionInformation.IsProtected)
                {
                    // Krypterar sektionen
                    section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                    config.Save();
                }
            }
            catch (Exception ex)
            {
            }
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
    }
}

