using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Business.Generics
{
    public static class ProductList
    {
        public static List<T> GetLists<T>(List<T> obj)
        {
            List<T> x = new List<T>();
            x = obj.ToList();

            return x;
        }
        public static T GetItem<T>(T obj)
        {
            T x = obj;
            return x;
        }
    }
}