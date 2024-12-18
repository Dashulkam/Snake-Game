using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class Score
    {
        public int CurrentScore { get; private set; }

        public void IncreaseScore(int amount)
        {
            CurrentScore += amount;
        }

        public void Reset()
        {
            CurrentScore = 0;
        }
    }
}

