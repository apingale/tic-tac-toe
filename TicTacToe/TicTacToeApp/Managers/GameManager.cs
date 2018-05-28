using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicTacToeApp.Enums;
using TicTacToeApp.Extension;
using TicTacToeApp.Models;

namespace TicTacToeApp.Managers
{
    public class GameManager
    {
        public List<Game> _gamesNow;

        public GameManager()
        {
            _gamesNow = new List<Game>();
        }
        

        /// <summary>
        /// Creates New game or 
        /// </summary>
        /// <param name="playerID"></param>
        /// <returns></returns>
        public Game Create()
        {
            // Get if there is any unassigned existing game 
            Guid playerID = Guid.NewGuid();

            Game game = GetExistingGames();
            if (game != null)
            {
                game.Player2 = new Player();
                game.Player2.ID = playerID;
                game.PlayedBy = playerID;
                game.Player2.Symbol = 'O';
            }
            else
            {
                game = new Game();
                game.Player1 = new Player();
                game.Player1.ID = playerID;
                game.PlayedBy = playerID;
                game.Player1.Symbol = 'X';
                _gamesNow.Add(game);
            }
           

            return game;
        }

        private Game GetExistingGames()
        {
            Game currentGame = _gamesNow.Where(x => x.Player2 == null || x.Player1 == null).FirstOrDefault();
            return currentGame;
        }

        public Game Update(Guid playerID, int moveID)
        {
            // Updating players moves 
            if (CanUpdate(playerID))
            {
                UpdatePlayersMove(playerID, moveID);
                AnalysePlayersMove(playerID, moveID);
            }
            else
                throw new Exception("Waiting for other player");
            Game currentGame = FetchGame(playerID);
            return currentGame;
        }

        /// <summary>
        /// If both the ok players are not online do not update
        /// </summary>
        /// <param name="playerID"></param>
        public bool CanUpdate(Guid playerID)
        {
            Game game = FetchGame(playerID);
            bool canUpdate = false;
            if (game.Player1 != null && game.Player2 != null)
            {
                canUpdate = true;
            }

            return canUpdate;
        }

        private void UpdatePlayersMove(Guid playerID, int moveID)
        {
            List<int> moves = _gamesNow.Where(x =>  x.Player1 != null && x.Player1.ID == playerID).Select(x => x.Player1.Moves).FirstOrDefault();
            if (moves != null && !moves.Contains(moveID))
                moves.Add(moveID);

            moves = _gamesNow.Where(x => x.Player2 != null && x.Player2.ID == playerID).Select(x => x.Player2.Moves).FirstOrDefault();
            if (moves != null && !moves.Contains(moveID))
                moves.Add(moveID);
        }

        private Player FetchPlayer(Guid playerID)
        {
            Player player;
            player = _gamesNow.Where(x => x.Player1.ID == playerID).Select(x => x.Player1).SingleOrDefault();

            if(player == null)
                player = _gamesNow.Where(x => (x.Player2 != null && x.Player2.ID == playerID)).Select(x => x.Player2).SingleOrDefault();
            return player;
        }

        private void AnalysePlayersMove(Guid playerID, int moveID)
        {
            // Check if match is draw
            CheckMatchResults(playerID);
        }

        private void CheckMatchResults(Guid playerID)
        {                
            Game game = FetchGame(playerID);
            if (game.Player1 == null || game.Player2 == null)
                return;

            if ((game.Player1.Moves.Count() + game.Player2.Moves.Count()) == 9)
                game.Status = GameStatus.Draw;

            // Get player info
            Player player = FetchPlayer(playerID);
            bool haveWinner = false;
            // Check if player has won
            haveWinner = CheckWinner(player);

            // If player has won update status
            if (haveWinner)
            {
                player.Status = GameStatus.Won;
                game.Status = GameStatus.Won;
            }
        }

        private Game FetchGame(Guid playerID)
        {
            return _gamesNow.Where(x => (x.Player1 != null && x.Player1.ID == playerID) || (x.Player2 != null && x.Player2.ID == playerID)).Single();
        }

        private bool CheckWinner(Player player)
        {
            bool haveWinner = false;
            // Check Columns
            haveWinner = IsWinner(player, GridLines.Column);

            //Check diagonal
            if (!haveWinner)
                haveWinner = IsWinner(player, GridLines.Diagonal);

            // check Row
            if (!haveWinner)
                haveWinner = IsWinner(player, GridLines.Row);
            return haveWinner;
        }

        private bool IsWinner(Player player, GridLines gridLine)
        {
            bool isWon = false;
            Dictionary<string, List<int>> winningSets = LoadJson(gridLine);
            foreach (var item in winningSets)
            {
                if (player.Moves.Intersect(item.Value).Count() == item.Value.Count())
                {
                    isWon = true;
                    break;
                }
            }

            return isWon;
        }

        private Dictionary<string, List<int>> LoadJson(GridLines gridLine)
        {
            Dictionary<string, List<int>> winningSets = null;
            switch (gridLine)
            {
                case GridLines.Column:
                    winningSets = LoadJson("ColumnWinnerSets.json");
                    break;

                case GridLines.Diagonal:
                    winningSets = LoadJson("DiagonalWinnerSets.json");
                    break;
                case GridLines.Row:
                    winningSets = LoadJson("RowWinnerSets.json");
                    break;
            }

            return winningSets;
        }

        private Dictionary<string, List<int>> LoadJson(string fileName)
        {
            string path = string.Format(@"E:\Code\TicTacToe\TicTacToe\TicTacToeApp\JSON\{0}", fileName);
            string Json = System.IO.File.ReadAllText(path);
            Dictionary<string, List<int>> winningSets = JsonConvert.DeserializeObject<Dictionary<string, List<int>>>(Json);

            return winningSets;
        }

        public void Delete(Guid gameID)
        {
            Game game = _gamesNow.Where(x => x.ID == gameID).SingleOrDefault();
            _gamesNow.Remove(game);
        }
    }
}
