using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPE_Service.Data.DAL
{
    public interface IUnitOfWork
    {
        void Dispose();

        void SaveChanges();
    }
}