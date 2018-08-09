using Business.Data;
using Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Business.DataService
{
    public class CustomerDataService : BaseContext
    {
        public static void SaveCustomer(Customer customer)
        {
            db.Customers.Add(customer);
            db.SaveChanges();
        }
        public static Customer GetCustomer(string _userID)
        {
            Customer customer = db.Customers.Where(x => x.User_Id == _userID).FirstOrDefault();
            return customer;
        }
    }
}