using HPE_Service.Data.DAL;
using HPE_Service.Data.DAL.Repositories;
using HPE_Service.Ëntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace HPE_Service.Services
{
    public class RPSService
    {

        public class BracketedFile
        {
            public string Path { get; set; }
            public string Ouput { get; set; }


            public BracketedFile(string FilePath)
            {
                Path = FilePath;
                Load();
            }

            public void Load()
            {
                string text = System.IO.File.ReadAllText(@Path);
            }

        }

        public class BracketedGame
        {
            public string GameInput { get; set; }
            public string GameOutput { get; set; }
            public string Winner { get; set; }
            public string WinnerMove { get; set; }
            public string Loser { get; set; }
            public string LoserMove { get; set; }
            public bool Completed { get; set; }
            public bool Promoted { get; set; }
            private string P1 { get; set; }
            private string P1Move { get; set; }
            private string P2 { get; set; }
            private string P2Move { get; set; }


            public BracketedGame(string Input)
            {

                GameInput = Input;
                Promoted = false;
            }

            public void RunGame()
            {
                Decode();
                GetWinner();
                Completed = true;
            }

            private void Decode()
            {

                char[] delimiterChars = { '"', '"' };
                string[] words = GameInput.Split(delimiterChars);


                var reg = new Regex("\".*?\"");//"\".*?\"");
                var matches = reg.Matches(GameInput);
                if (matches.Count != 4)
                {
                    //if (words.Length != 4)
                    //{
                    throw new System.ArgumentException("There is some missing player or strategy data, please check the game bracketed list.", "Game Error");
                }
                else
                {
                    P1 = matches[0].ToString();
                    P1 = P1.Remove(P1.IndexOf("\""), 1);
                    P1 = P1.Remove(P1.LastIndexOf("\""), 1);
                    P1Move = matches[1].ToString().ToUpper();
                    P1Move = P1Move.Remove(P1Move.IndexOf("\""), 1);
                    P1Move = P1Move.Remove(P1Move.LastIndexOf("\""), 1);
                    P2 = matches[2].ToString();
                    P2 = P2.Remove(P2.IndexOf("\""), 1);
                    P2 = P2.Remove(P2.LastIndexOf("\""), 1);
                    P2Move = matches[3].ToString().ToUpper();
                    P2Move = P2Move.Remove(P2Move.IndexOf("\""), 1);
                    P2Move = P2Move.Remove(P2Move.LastIndexOf("\""), 1);

                    if (!new string[] { "R", "P", "S" }.Any(s => P1Move.Contains(s)))
                    {
                        throw new System.ArgumentException("Invalid Strategy, please check the game bracketed list.", "Game Error");
                    }

                    if (!new string[] { "R", "P", "S" }.Any(s => P2Move.Contains(s)))
                    {
                        throw new System.ArgumentException("Invalid Strategy, please check the game bracketed list.", "Game Error");
                    }

                }

            }
            private void GetWinner()
            {



                switch (P1Move)
                {
                    case "R":
                        switch (P2Move)
                        {
                            case "R":
                                Winner = P1;
                                WinnerMove = P1Move;
                                Loser = P2;
                                LoserMove = P2Move;
                                break;

                            case "P":
                                Winner = P2;
                                WinnerMove = P2Move;
                                Loser = P1;
                                LoserMove = P1Move;
                                break;
                            case "S":
                                Winner = P1;
                                WinnerMove = P1Move;
                                Loser = P2;
                                LoserMove = P2Move;
                                break;
                        }

                        break;

                    case "P":
                        switch (P2Move)
                        {
                            case "R":
                                Winner = P1;
                                WinnerMove = P1Move;
                                Loser = P2;
                                LoserMove = P2Move;
                                break;

                            case "P":
                                Winner = P1;
                                WinnerMove = P1Move;
                                Loser = P2;
                                LoserMove = P2Move;
                                break;
                            case "S":
                                Winner = P2;
                                WinnerMove = P2Move;
                                Loser = P1;
                                LoserMove = P1Move;
                                break;
                        }

                        break;

                    case "S":
                        switch (P2Move)
                        {
                            case "R":
                                Winner = P2;
                                WinnerMove = P2Move;
                                Loser = P1;
                                LoserMove = P1Move;
                                break;

                            case "P":
                                Winner = P1;
                                WinnerMove = P1Move;
                                Loser = P2;
                                LoserMove = P2Move;
                                break;
                            case "S":
                                Winner = P1;
                                WinnerMove = P1Move;
                                Loser = P2;
                                LoserMove = P2Move;
                                break;
                        }

                        break;

                }
                GameOutput = "[\"" + Winner + "\", \"" + WinnerMove + "\"]";
            }

        }

        public class Tournament
        {
            public string TournamentInput { get; set; }
            public string TournamentOutput { get; set; }
            public string Winner { get; set; }
            public string WinnerMove { get; set; }
            public string Second { get; set; }
            public string SecondMove { get; set; }
            public bool TournamentCompleted { get; set; }
            public List<string> PlayerWaitingList = new List<string>();
            public List<BracketedGame> GamesQueue = new List<BracketedGame>();

            public Tournament(string Input)
            {

                TournamentInput = Input;


            }


            private void Decode()
            {
                string regularExpressionPattern = @"\[\[\[(.*?)]]]";

                var reg = new Regex(regularExpressionPattern);
                var matches = reg.Matches(TournamentInput);
                foreach (var item in matches)
                {
                    string gamedata = item.ToString();

                    string regularExpressionPattern2 = @"\[\[(.*?)]]";
                    var reg2 = new Regex(regularExpressionPattern2);
                    var games = reg2.Matches(gamedata);
                    foreach (var game in games)
                    {
                        AddGame(game.ToString());
                    }
                }


            }

            private void AddGame(string GameData)
            {
                BracketedGame Game = new BracketedGame(GameData);
                GamesQueue.Add(Game);

            }

            private void RunGames()
            {

                for (var i = 0; i < GamesQueue.Count; i++)
                {
                    GamesQueue[i].RunGame();
                    AddtoWaitingList(GamesQueue[i].GameOutput);
                }

                BracketedGame Final = GamesQueue.Last();
                Winner = Final.Winner;
                WinnerMove = Final.WinnerMove;
                Second = Final.Loser;
                SecondMove = Final.LoserMove;

            }
            private void AddtoWaitingList(string Player)
            {
                PlayerWaitingList.Add(Player);
                if (PlayerWaitingList.Count == 2)
                {
                    string GameData = "";
                    GameData = "[" + PlayerWaitingList[0].ToString() + "," + PlayerWaitingList[1].ToString() + "]";
                    AddGame(GameData);
                    PlayerWaitingList.Clear();
                }


            }

            public void Start()
            {
                Decode();
                RunGames();
            }


        }


        private Data.DAL.IConnectionFactory connectionFactory;

        public IList<Ëntities.Player> TopN( int topN)
        {
            connectionFactory = ConnectionHelper.GetConnection();

            var context = new DbContext(connectionFactory);

            var PlayerRepo = new RPSRepo(context);

            return PlayerRepo.TopN(topN);
        }

        public IList<Ëntities.Player> Championship(string textdata)
        {

            Tournament t = new Tournament(textdata);

            t.Start();
            connectionFactory = ConnectionHelper.GetConnection();

            var context = new DbContext(connectionFactory);

            var PlayerRepo = new RPSRepo(context);

            return PlayerRepo.UpdateScore(t.Winner,t.Second);
        }

        public void ManualScoreEntry(string p1name, string p2name)
        {
            connectionFactory = ConnectionHelper.GetConnection();

            var context = new DbContext(connectionFactory);

            var PlayerRepo = new RPSRepo(context);

            var updated =  PlayerRepo.UpdateScore(p1name, p2name);


        }

        public void CleanDb()
        {
            connectionFactory = ConnectionHelper.GetConnection();

            var context = new DbContext(connectionFactory);

            var PlayerRepo = new RPSRepo(context);

             PlayerRepo.CleanDb();


        }

    }
}