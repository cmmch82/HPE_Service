using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
namespace HPE_Service.Data.DAL
{
    public interface IConnectionFactory
    {
        IDbConnection Create();
    }
}