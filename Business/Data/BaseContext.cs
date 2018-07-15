using Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Business.Data
{
    public abstract class BaseContext
    {
        public static DataContext db;
        public BaseContext()
        {
            db = new DataContext();
        }
    }
}