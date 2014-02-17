using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuMain
{
    class SingleHighscore
    {
        private List<string> _name = new List<string>();//lista med namn
        private List<int> _score = new List<int>();//lista med poäng
        
        //Returnerar en sträng med namn och poäng
        public void LoadScore(string value, int points)
        { 
            _name.Add(value);
            _score.Add(points);
        }

        //Returnerar en sträng med namn och poäng
        public string GetNameAndScore(int index)
        { 
            return _name[index] + "\t" + _score[index]; 
        }

        //Returnerar en sträng med namn och poäng(tid)
        public string GetNameAndTime(int index)
        { 
            return _name[index] + "\t" + (DateTime.Today + new TimeSpan(0, 0, _score[index])).ToString("H:mm:ss"); 
        }

        //Placerar en ny person på listan
        public void InsertScore(string name, int points, int index)
        { 
            _name.Insert(index, name);
            _score.Insert(index, points);
        }

        //Tar bort en person från listan på indexets plats. Ämnat för sista personen
        public void RemoveLast(int index) 
        { 
            _name.RemoveAt(index); 
            _score.RemoveAt(index);
        }

        //Returnerar en lista med poäng
        public List<int> GetPoints() 
        { 
            return _score; 
        }

        //Returnerar längden på listan
        public int GetLength() 
        {
            return _score.Count;
        }
    }
}
