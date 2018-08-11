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
            Customer existedCustomer = db.Customers.Where(x=>x.User_Id == customer.User_Id).FirstOrDefault();

            if (existedCustomer != null)
                customer = existedCustomer;
            else
            {
                db.Customers.Add(customer);
                db.SaveChanges();
            }
        }
        public static Customer GetCustomer(string _userID)
        {
            Customer customer = db.Customers.Where(x => x.User_Id == _userID).FirstOrDefault();
            return customer;
        }
        public static List<Customer> GetCustomers()
        {
            List<Customer> customers = db.Customers.ToList();
            foreach (Customer cust in customers)
                      cust.TotalSpent = TotalSpent(cust.User_Id);
            return customers;
        }
        public static decimal TotalSpent(string _userID)
        {
            var ordersTotal = db.Orders.Where(x => x.Customer.User_Id == _userID).Sum(y => y.Order_Value);
            return ordersTotal;
        }
    }
}