using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;

namespace ConsoleApplication1
{

    class Program
    {
        static void Main(string[] args)
        {
            string path = @"c:\HPE_RockPaperScissors\TextFiles\game1.txt";
            string readText = File.ReadAllText(path);
            Console.WriteLine(readText);
            var client = new System.Net.Http.HttpClient();
            var content = new StringContent(readText);
            var response = client.PostAsync("http://localhost:55273/api/championship/new", content).Result;

            var test = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(test);

            //WebApiCheck test = new WebApiCheck();
            //test.TruncateDB();
            //test.LoadChapionshipFile();
            //test.TopTen();
            //test.ManualFirstandSecondplace();


        }



    }






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


        public BracketedGame(string Input) {

            GameInput = Input;
            Promoted = false;
        }

        public void RunGame() {
            Decode();
            GetWinner();
            Completed = true;
        }

        private void Decode() {

            char[] delimiterChars = { '"', '"' };
            string[] words = GameInput.Split(delimiterChars);


            var reg = new Regex("\".*?\"");//"\".*?\"");
            var matches = reg.Matches(GameInput);
            if (matches.Count != 4) {
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

                if (!new string[] { "R", "P", "S" }.Any(s => P1Move.Contains(s))) {
                    throw new System.ArgumentException("Invalid Strategy, please check the game bracketed list.", "Game Error");
                }

                if (!new string[] { "R", "P", "S" }.Any(s => P2Move.Contains(s)))
                {
                    throw new System.ArgumentException("Invalid Strategy, please check the game bracketed list.", "Game Error");
                }

            }

        }
        private void GetWinner() {



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
                Console.WriteLine(item.ToString());

                string gamedata = item.ToString();
                //string regularExpressionPattern2 = @"\[\[(.*?)]]";
                //string regularExpressionPattern2 = ("\".*?\"");
                string regularExpressionPattern2 = @"\[\[(.*?)]]";
                var reg2 = new Regex(regularExpressionPattern2);
                var games = reg2.Matches(gamedata);
                foreach (var game in games)
                {
                    AddGame(game.ToString());
                }
            }
            Console.ReadLine();

        }

        private void AddGame(string GameData)
        {
            BracketedGame Game = new BracketedGame(GameData);
            GamesQueue.Add(Game);

        }

        private void RunGames() {


            //bool isEmpty = !GamesQueue.Any();
            //if (isEmpty)
            //{
            //    // error message
            //}
            //else
            //{
            //    // show grid
            //}


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


            //foreach (var game in GamesQueue)
            //{
            //    game.RunGame();
            //    AddGame(game.GameOutput);
            //}





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

        public void Start() {
            Decode();
            RunGames();
        }


    }

    public class TestManualProcess
    {
        public TestManualProcess()
        {
            //char[] delimiterChars = { '[', ']' };

            string champ = "";

            // Declare a new StringBuilder.
            StringBuilder builder = new StringBuilder();
            builder.Append("[");
            builder.Append("[");
            builder.Append("[[\"Armando\", \"P\"], [\"Dave\", \"S\"]],");
            builder.Append("[[\"Richard\", \"R\"], [\"Michael\", \"S\"]]");
            builder.Append("],");
            builder.Append("[");
            builder.Append("[[\"Allen\", \"S\"], [\"Omer\", \"P\"]],");
            builder.Append("[[\"John\", \"R\"], [\"Robert\", \"P\"]]");
            builder.Append("]");
            builder.Append("]");

            // Get a reference to the StringBuilder's buffer content.
            champ = builder.ToString();
            Tournament t = new Tournament(champ);
            t.Start();
            Console.WriteLine(t.Winner);
            Console.WriteLine(t.WinnerMove);
            Console.ReadLine();



            //champ = champ.Trim();
            //string regularExpressionPattern = @"\[\[\[(.*?)]]]";

            //var reg = new Regex(regularExpressionPattern);
            //var matches = reg.Matches(champ);
            //foreach (var item in matches)
            //{
            //    Console.WriteLine(item.ToString());

            //    string test = item.ToString();
            //    string regularExpressionPattern2 = @"\[\[(.*?)]]";

            //    var reg1 = new Regex(regularExpressionPattern2);
            //    var matches1 = reg1.Matches(test);
            //    foreach (var item1 in matches1)
            //    {
            //        Console.WriteLine(item1.ToString());
            //    }
            //}
            //Console.ReadLine();


            //string champ = "[[\"Armando\", \"P\"], [\"Dave\", \"S\"]]";

            //BracketedGame game1 = new BracketedGame(champ);
            //game1.RunGame();

            //Console.WriteLine(game1.GameOutput);


            // char[] delimiterChars = { '[', ']' };
            // string[] words = champ.Split(delimiterChars);
            //// System.Console.WriteLine("{ 0} words in text:", words.Length);

            // foreach (string s in words)
            // {
            //     System.Console.WriteLine(s);
            // }

            // Console.ReadLine();

            // string regularExpressionPattern = @"\[(.*?)\]";

            // string inputText = "[[\"Armando\", \"P\"], [\"Dave\", \"S\"]]";

            // var reg = new Regex("\".*?\"");
            // var matches = reg.Matches(inputText);
            // foreach (var item in matches)
            // {
            //     Console.WriteLine(item.ToString());
            // }

            // Console.ReadLine();

            // inputText = inputText.Remove(inputText.IndexOf("["), 1);
            // inputText = inputText.Remove(inputText.LastIndexOf("]"), 1);
            // Console.WriteLine(inputText);
            // Regex re = new Regex(regularExpressionPattern);

            // foreach (Match m in re.Matches(inputText))
            // {
            //     Console.WriteLine(m.Value);
            // }

            Console.ReadLine();
        }

    }

    public class WebApiCheck{

        public WebApiCheck()
        {



        }

        public void LoadChapionshipFile()
        {
            Console.WriteLine("\n");
            string path = @"c:\HPE_RockPaperScissors\TextFiles\game1.txt";
            string readText = File.ReadAllText(path);
            Console.WriteLine("---------------------this is the text file ------------------------");
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine(readText);
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("------------------Calling Post Operation--------------------");
            var client = new System.Net.Http.HttpClient();
            var content = new StringContent(readText);
            var response = client.PostAsync("http://localhost:55273/api/championship/new", content).Result;
            var test = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(test);
            Console.ReadLine();

        }

        public void ManualFirstandSecondplace()
        {
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("------------------Calling manual winner  Operation--------------------");
            var client = new System.Net.Http.HttpClient();
            var content = new StringContent("first=John&second=Mark");

            var response = client.PostAsync("http://localhost:55273/api/championship/", content).Result;
            var test = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(test);
            Console.ReadLine();

        }


        public void TopTen()
        {
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("------------------Calling top 10 Operation--------------------");
            var client = new System.Net.Http.HttpClient();
            var response = client.GetAsync("http://localhost:55273/api/championship/top?count=2").Result;
            var test = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(test);
            Console.ReadLine();
        }

        public void TruncateDB()
        {
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("------------------Calling Delete Operation--------------------");
            var client = new System.Net.Http.HttpClient();
            client.DeleteAsync("http://localhost:55273/api/championship/truncate");
            Console.WriteLine("-------------------Db is empty----------------------------------------");
            Console.ReadLine();
        }
    }

}
