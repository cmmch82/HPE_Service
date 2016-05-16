using HPE_Service.Data.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPE_Service.Services
{
    public class ConnectionHelper
    {
        public static IConnectionFactory GetConnection()
        {
            return new DbConnectionFactory("HPEConString");
        }
    }
}