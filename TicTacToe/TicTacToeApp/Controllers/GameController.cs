using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TicTacToeApp.Extension;
using TicTacToeApp.Managers;
using TicTacToeApp.Models;
using TicTacToeApp.Services;

namespace TicTacToeApp.Controllers
{
    [Produces("application/json")]
    [Route("api/Game")]
    public class GameController : Controller
    {        
        private IMemoryCache _cache;

        public GameController(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        [HttpPost]
        public IActionResult Create()
        {
            GameService gameService = new GameService(_cache);
            Game game = gameService.Create();           
            return Ok(game);
        }

        [HttpPut("{moveID}/{playerID}")]
        public IActionResult Update(int moveID, Guid playerID)
        {
            GameService gameService = new GameService(_cache);
            Game game;

            game = gameService.Update(playerID, moveID);
            
            
            return Ok(game);
        }

        //[HttpGet("{id}", Name = "GetTodo")]
        //public IActionResult GetById(Guid id)
        //{
        //    Game game = gameManager.GetByID(id);
        //    if (game == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(game);
        //}

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            GameService gameService = new GameService(_cache);
            gameService.Delete(id);
            return NoContent();
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "game1", "game2" };
        }


    }
}