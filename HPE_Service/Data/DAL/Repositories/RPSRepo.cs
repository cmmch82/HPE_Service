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
                command.CommandText = String.Format("Select Top {0} UserName, UserScore from Player ", top); 

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
                command.CommandText = String.Format("Insert into Player values ( {0}, {1})", player.UserName, player.UserScore );

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

                //return this.ToList(command).ToList();
            }
        }

        //public Empleado GetEmpleadoById(int empId)
        //{
        //    using (var command = _context.CreateCommand())
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.CommandText = "uspGetEmpleadoById";

        //        command.Parameters.Add(command.CreateParameter("@id", empId));

        //        return this.ToList(command).FirstOrDefault();
        //    }
        //}


        //public bool CreateEmpleado(Empleado emp)
        //{
        //    using (var command = _context.CreateCommand())
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.CommandText = "uspCreateEmpleado";

        //        command.Parameters.Add(command.CreateParameter("@Promedio", emp.Promedio));
        //        command.Parameters.Add(command.CreateParameter("@Nombre", emp.Nombre));
        //        command.Parameters.Add(command.CreateParameter("@Primer_Apellido", emp.Primer_Apellido));
        //        command.Parameters.Add(command.CreateParameter("@Segundo_Apellido", emp.Segundo_Apellido));
        //        command.Parameters.Add(command.CreateParameter("@Direccion", emp.Direccion));

        //        command.ExecuteNonQuery();


        //        return true;

        //        // this.ToList(command).FirstOrDefault();


        //    }

        //}

        //public Empleado UpdateEmpleado(Empleado emp)
        //{
        //    using (var command = _context.CreateCommand())
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.CommandText = "uspUpdateEmpleado";

        //        command.Parameters.Add(command.CreateParameter("@id", emp.Id));
        //        command.Parameters.Add(command.CreateParameter("@Promedio", emp.Promedio));
        //        command.Parameters.Add(command.CreateParameter("@Nombre", emp.Nombre));
        //        command.Parameters.Add(command.CreateParameter("@Primer_Apellido", emp.Primer_Apellido));
        //        command.Parameters.Add(command.CreateParameter("@Segundo_Apellido", emp.Segundo_Apellido));
        //        command.Parameters.Add(command.CreateParameter("@Direccion", emp.Direccion));

        //        return this.ToList(command).FirstOrDefault();
        //    }

        //}

        //public void DeleteEmpleado(int empId)
        //{
        //    using (var command = _context.CreateCommand())
        //    {
        //        command.CommandText = string.Format("Delete from Empleado where id= {0}", empId);
        //        command.ExecuteReader();

        //    }
        //}


    }

}