using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TicTacToeApp.Managers;
using TicTacToeApp.Models;

namespace TicTacToeApp.Services
{
    public class GameService
    {
        private GameManager gameManager;
        private IMemoryCache _cache;

        public GameService(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
            gameManager = new GameManager();
        }

        public Game Create()
        {
            List<Game> cachedGame = GetCachedGames();
            gameManager._gamesNow = cachedGame != null ? cachedGame : new List<Game>();
            Game game = gameManager.Create();
            CacheGames();

            return game;
        }

        private void CacheGames()
        {
            _cache.Set("cachedGames", gameManager._gamesNow,
                               new MemoryCacheEntryOptions());
        }

        public Game Update(Guid playerID, int moveID)
        {
            gameManager._gamesNow = GetCachedGames();            
            Game game = gameManager.Update(playerID, moveID);
            CacheGames();
            return game;
        }

        private List<Game> GetCachedGames()
        {
            return _cache.Get("cachedGames") as List<Game>;
        }

        public void Delete(Guid gameID)
        {
            gameManager.Delete(gameID);
        }
    }
}
