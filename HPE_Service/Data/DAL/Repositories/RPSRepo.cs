using System.Data;
using HPE_Service.Ëntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HPE_Service.Data.DAL.Extensions;

namespace HPE_Service.Data.DAL.Repositories
{

    public class RPSRepo : Repository<Player>
    {
        private DbContext _context;
        public RPSRepo(DbContext context)
            : base(context)
        {
            _context = context;
        }


        public IList<Player> TopN(int top)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = String.Format("Select Top {0} UserName, UserScore from Player order by UserScore desc ", top); 

                return this.ToList(command).ToList();
            }
        }

        public Player GetPlayerByName(string username)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = String.Format("Select UserName, UserScore from Player where UserName = '{0}'" ,username);

                return this.ToList(command).FirstOrDefault();
            }
        }

        public void UpdatePlayer(Player player)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = String.Format("Update Player set UserScore = {0} where UserName = '{1}' ", player.UserScore, player.UserName);

                // return this.ToList(command).ToList();
            }
        }

        public void InsertPlayer (Player player)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = string.Format("Insert into Player values ( {0}, {1})", player.UserName, player.UserScore );

                //return this.ToList(command).ToList();
            }
        }


        public IList<Player> UpdateScore(string p1name, string p2name)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "usp_UpdateScore ";

                command.Parameters.Add(command.CreateParameter("@P1Name ", p1name));
                command.Parameters.Add(command.CreateParameter("@P2Name ", p2name));

                return this.ToList(command).ToList();
            }
        }

        public void CleanDb()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = "truncate table Player";
                command.ExecuteNonQuery();

                //return this.ToList(command).ToList();
            }
        }


    }

}