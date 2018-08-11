using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Business.Data;
using Business.DataService;
using Business.Entities;
using Business.Enums;
using Business.Paypal;
using Business.ViewModels;
using Microsoft.AspNet.Identity;
using PayPal.Api;

namespace Business.DataService
{
    public class PaymentDataService : BaseContext
    {
        private APIContext apiContext = Configuration.GetAPIContext();
        private static CartDataService _cartDataService = new CartDataService();
        private static PayPal.Api.Payment payment;

        //save customer info
        public static void SaveCustomerInfo(Payment executedPayment, string _userID)
        {
            Customer customer = new Customer();
            customer.First_Name = executedPayment.payer.payer_info.first_name;
            customer.Last_Name = executedPayment.payer.payer_info.last_name;
            customer.Address = executedPayment.payer.payer_info.shipping_address.line1 + " " +
                                executedPayment.payer.payer_info.shipping_address.line2 + " " +
                                executedPayment.payer.payer_info.shipping_address.city + ", " +
                                executedPayment.payer.payer_info.shipping_address.postal_code +
                                executedPayment.payer.payer_info.shipping_address.country_code;
            customer.Phone_Number = executedPayment.payer.payer_info.shipping_address.phone;
            customer.Email_Address = executedPayment.payer.payer_info.email;
            customer.User_Id = _userID;
            CustomerDataService.SaveCustomer(customer);
        }

        //save successful order
        public static void SaveOrder(Payment executedPayment, string _userID)
        {
            Business.Entities.Order order = new Business.Entities.Order();
            order.Customer_Id = CustomerDataService.GetCustomer(_userID).Customer_Id;

            var dateToday = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var hours = DateTime.Now;
            var time = dateToday.ToString("MMMM dd, yyyy ") + hours.ToString("h:m tt",
                  CultureInfo.InvariantCulture);
            order.Order_Date = time;
            order.ShipAddress = executedPayment.payer.payer_info.shipping_address.line1;
            order.ShipCity = executedPayment.payer.payer_info.shipping_address.city;
            order.PostalCode = Convert.ToInt32(executedPayment.payer.payer_info.shipping_address.postal_code);
            order.ShipCountry = executedPayment.payer.payer_info.shipping_address.country_code;
            order.Payment_Status = (int)Payment_Status.Paid;
            foreach (var item in executedPayment.transactions)
            {
                order.Order_Value = Convert.ToDecimal(item.amount.total);
            }
            //Add more info if needed

            OrderDataService.SaveSuccessfulOrder(order);

            //clear cart if it is successful order
            _cartDataService.RemoveAllItems(_userID);
        }
        public static Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            payment = new Payment() { id = paymentId };
            return payment.Execute(apiContext, paymentExecution);
        }

        public static Payment CreatePayment(APIContext apiContext, string redirectUrl, List<Cart> productList)
        {
            List<CartItem> orderSession = new List<CartItem>();

            //similar to credit card create itemlist and add item objects to it
            var itemList = new ItemList();
            List<Item> listOfItems = new List<Item>();

            decimal subTotal = 0;
            foreach (var product in productList)
            {
                Item item = new Item();
                item.currency = "USD";
                item.name = product.Product_Name;
                item.description = product.Product_Description;
                item.quantity = product.Quantity.ToString();
                item.sku = product.Product_Id.ToString();
                item.price = product.UnitPrice.ToString();
                listOfItems.Add(item);
                subTotal += (product.Quantity > 1) ? product.UnitPrice * product.Quantity : product.UnitPrice;

                CartItem _orderSession = new CartItem();
                _orderSession.Cart_Id = product.Cart_Id;
                _orderSession.User_Id = product.User_Id;
                _orderSession.Product_Id = product.Product_Id;
                _orderSession.Quantity = product.Quantity;
                _orderSession.UnitPrice = (decimal)product.PriceTotal;

                _cartDataService.AddCartItem(_orderSession);
            }
            itemList.items = listOfItems;
            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            // similar as we did for credit card, do here and create details object
            var details = new Details()
            {
                tax = "1",
                shipping = "1",
                subtotal = subTotal.ToString()
            };

            // similar as we did for credit card, do here and create amount object
            var amount = new Amount()
            {
                currency = "USD",
                total = (Convert.ToDecimal(details.subtotal) + Convert.ToDecimal(details.shipping) + Convert.ToDecimal(details.tax)).ToString(), //total.ToString(), // Total must be equal to sum of shipping, tax and subtotal.
                details = details
            };

            var transactionList = new List<Transaction>();
            Random r = new Random();
            string invoiceCode = DateTime.Now.ToString("MMddyyyyss") + r.Next(100);

            transactionList.Add(new Transaction()
            {
                description = "Transaction description.",

                invoice_number = invoiceCode + 1,
                amount = amount,
                item_list = itemList,
            });


            payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return payment.Create(apiContext);
        }
    }
}