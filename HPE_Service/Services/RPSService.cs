using HPE_Service.Data.DAL;
using HPE_Service.Data.DAL.Repositories;
using HPE_Service.Ëntities;
using HPE_Service.ProblemSolution;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace HPE_Service.Services
{
    public class RPSService
    {
 
        private Data.DAL.IConnectionFactory connectionFactory;

        public IList<Player> TopN( int topN)
        {
            connectionFactory = ConnectionHelper.GetConnection();

            var _context = new DbContext(connectionFactory);

            var playerRepo = new RPSRepo(_context);

            return playerRepo.TopN(topN);
        }

        public IList<Player> Championship(string textData)
        {

            Tournament tournament = new Tournament(textData);

            tournament.Start();
            connectionFactory = ConnectionHelper.GetConnection();

            var _context = new DbContext(connectionFactory);

            var playerRepo = new RPSRepo(_context);
            IList<Player> result = new List<Player>();

            Player player1 = new Player
            {
                UserName = tournament.Winner,
                Strategy = tournament.WinnerMove
            };
            Player player2 = new Player
            {
                UserName = tournament.Second,
                Strategy = tournament.SecondMove
            };
            result.Add(player1);
            result.Add(player2);

            var updateResults = playerRepo.UpdateScore(tournament.Winner, tournament.Second);

            return result;//;
        }

        public void ManualScoreEntry(string p1Name, string p2Name)
        {
            connectionFactory = ConnectionHelper.GetConnection();

            var _context = new DbContext(connectionFactory);

            var playerRepo = new RPSRepo(_context);

            var updated =  playerRepo.UpdateScore(p1Name, p2Name);


        }

        public void CleanDb()
        {
            connectionFactory = ConnectionHelper.GetConnection();

            var _context = new DbContext(connectionFactory);

            var playerRepo = new RPSRepo(_context);

             playerRepo.CleanDb();


        }

        public BracketedGame TestSolution(string oneGameData)
        {
            BracketedGame game = new BracketedGame(oneGameData);
            game.RunGame();
            return game;
        }
        public Tournament Narrator(string gameData) {

            Tournament tournament = new Tournament(gameData);
            tournament.Start();
            return tournament;

        }

    }
}