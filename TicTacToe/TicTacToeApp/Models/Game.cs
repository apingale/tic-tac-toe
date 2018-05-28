using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToeApp.Enums;

namespace TicTacToeApp.Models
{
    public class Game
    {
        public Game()
        {
            ID = Guid.Parse("d971a852-28be-4c53-b1df-36fe8cfb3c2d");
            Status = GameStatus.New;
            SocketID = Guid.Parse("a429bf6e-9099-4118-8714-92d924d759e6");
        }
        public Guid ID { get; set; }        
        public GameStatus Status { get; set; }
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public Guid PlayedBy { get; set; }
        public Guid SocketID { get; set; }

    }
}
