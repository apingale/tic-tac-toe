using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToeApp.Enums;

namespace TicTacToeApp.Models
{
    public class Player
    {
        public Player()
        {
            Moves = new List<int>();
        }
        public Guid ID { get; set; }
        public string Name { get; set; }
        public List<int> Moves { get; set; }
        public GameStatus Status { get; set; }
        public char Symbol { get; set; }
    }
}
