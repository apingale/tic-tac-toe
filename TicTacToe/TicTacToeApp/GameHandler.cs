using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSocketManager;

namespace TicTacToeApp
{
    public class GameHandler : WebSocketHandler
    {
        public GameHandler(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {
        }

        public async Task SendMove(string socketID, string moveID, string playerId, string status)
        {
            await InvokeClientMethodToAllAsync("pingMessage", socketID, moveID, playerId, status);
        }
    }
}
