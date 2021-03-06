﻿using Business.Data;
using Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Business.DataService
{
    public class OrderDataService : BaseContext
    {
        public static void SaveSuccessfulOrder(Order order)
        {
            db.Orders.Add(order);
            db.SaveChanges();
        }
        public static List<Order> UserOrders(string userId)
        {
            var orders = db.Orders.Where(x => x.Customer.User_Id == userId).ToList();
            return orders;
        }
    }
}