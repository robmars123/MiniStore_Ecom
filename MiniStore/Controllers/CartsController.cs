using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Business.DataService;
using Business.Entities;
using Business.Enums;
using Business.Paypal;
using Business.ViewModels;
using Microsoft.AspNet.Identity;
using PayPal.Api;
using static MiniStore.Controllers.PaymentController;

namespace MiniStore.Controllers
{
    public class CartsController : Controller
    {
        private CartDataService _cartDataService = new CartDataService();
        private CartViewModel _cartProductList = new CartViewModel();
        private APIContext apiContext = Configuration.GetAPIContext();
        private static string _userID;
        private PayPal.Api.Payment payment;

        // GET: Carts
        public ActionResult Index()
        {
            GetUserID(out _userID);
            _cartProductList.CartProducts = _cartDataService.GetAddedCartProducts(_userID);
            PrimaryProductImage(_cartProductList);
            Session["CartItems"] = _cartProductList.CartProducts;
            return View(_cartProductList);
        }
        private void PrimaryProductImage(CartViewModel _cartProductList)
        {
            _cartProductList.CartProducts.ForEach(item =>
           _cartProductList.PrimaryProduct_Image = _cartDataService.GetImages(item.Product_Id).FirstOrDefault()
           );
        }
        public void GetUserID(out string userID)
        {
            _userID = User.Identity.GetUserId(); //logged in User
            //userID = null;
            //if no loggedIn user, grab sessionID - loggedIn_User will be null.
            //get sessionID and save as userID - unregistered users
            userID = (_userID != null) ? userID = _userID : HttpContext.Session.SessionID;
        }
        public ActionResult Add(ProductViewModel model)
        {
            GetUserID(out _userID);
            _cartDataService.AddProductToCart(model, _userID);
            return RedirectToAction("Index", "Carts");
        }

        // GET: Carts/Create
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult Successful_Payment()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cart cart)
        {
            if (ModelState.IsValid)
            {
                //db.Carts.Add(cart);
                //db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cart);
        }

        // GET: Carts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(FormCollection cartForm, int id)
        {
            if (ModelState.IsValid)
            {
                Cart cartItem = new Cart();
                var quantity = cartForm.GetValues("item.Quantity").FirstOrDefault();
                cartItem.Quantity = Convert.ToInt32(quantity);
                _cartDataService.Update(cartItem, id);
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Carts/Delete/5
        public ActionResult Remove(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View();
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _cartDataService.Remove(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult PaymentWithPaypal()
        {
            try
            {
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist
                    //it is returned by the create function call of the payment class

                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    // So we have provided URL of this controller only
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority +
                                "/Carts/PaymentWithPayPal?";

                    //guid we are generating for storing the paymentID received in session
                    //after calling the create function and it is used in the payment execution

                    var guid = Convert.ToString((new Random()).Next(100000));

                    //CreatePayment function gives us the payment approval url
                    //on which payer is redirected for paypal account payment
                    var productList = (List<Cart>)Session["CartItems"];
                    var createdPayment = PaymentDataService.CreatePayment(apiContext, baseURI + "guid=" + guid, productList);

                    //get links returned from paypal in response to Create function call

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = null;

                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;

                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = lnk.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This section is executed when we have received all the payments parameters

                    // from the previous call to the function Create

                    // Executing a payment

                    var guid = Request.Params["guid"];

                    var executedPayment = PaymentDataService.ExecutePayment(apiContext, payerId, Session[guid] as string);

                    //Save Customer
                    PaymentDataService.SaveCustomerInfo(executedPayment, _userID);

                    //Mark cartitems as paid
                    var cartItems = _cartDataService.GetCartItems(_userID);
                    _cartDataService.UpdateCartItems(cartItems);

                    //Save Successful Order
                    PaymentDataService.SaveOrder(executedPayment, _userID);

                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("Not_Approved");
                    }
                }
            }
            catch (Exception ex)
            {
                var error = ex;// Logger.log("Error" + ex.Message);
                return View("FailureView");
            }

            return View("Successful_Payment");
        }







        //public ActionResult PaymentWithCreditCard()
        //{
        //    //create and item for which you are taking payment
        //    //if you need to add more items in the list
        //    //Then you will need to create multiple item objects or use some loop to instantiate object
        //    Item item = new Item();
        //    item.name = "Demo Item";
        //    item.currency = "USD";
        //    item.price = "5";
        //    item.quantity = "1";
        //    item.sku = "sku";

        //    //Now make a List of Item and add the above item to it
        //    //you can create as many items as you want and add to this list
        //    List<Item> itms = new List<Item>();
        //    itms.Add(item);
        //    ItemList itemList = new ItemList();
        //    itemList.items = itms;

        //    //Address for the payment
        //    Address billingAddress = new Address();
        //    billingAddress.city = "NewYork";
        //    billingAddress.country_code = "US";
        //    billingAddress.line1 = "23rd street kew gardens";
        //    billingAddress.postal_code = "43210";
        //    billingAddress.state = "NY";


        //    //Now Create an object of credit card and add above details to it
        //    //Please replace your credit card details over here which you got from paypal
        //    CreditCard crdtCard = new CreditCard();
        //    crdtCard.billing_address = billingAddress;
        //    crdtCard.cvv2 = "874";  //card cvv2 number
        //    crdtCard.expire_month = 1; //card expire date
        //    crdtCard.expire_year = 2020; //card expire year
        //    crdtCard.first_name = "Aman";
        //    crdtCard.last_name = "Thakur";
        //    crdtCard.number = "1234567890123456"; //enter your credit card number here
        //    crdtCard.type = "visa"; //credit card type here paypal allows 4 types

        //    // Specify details of your payment amount.
        //    Details details = new Details();
        //    details.shipping = "1";
        //    details.subtotal = "5";
        //    details.tax = "1";

        //    // Specify your total payment amount and assign the details object
        //    Amount amnt = new Amount();
        //    amnt.currency = "USD";
        //    // Total = shipping tax + subtotal.
        //    amnt.total = "7";
        //    amnt.details = details;

        //    // Now make a transaction object and assign the Amount object
        //    Transaction tran = new Transaction();
        //    tran.amount = amnt;
        //    tran.description = "Description about the payment amount.";
        //    tran.item_list = itemList;
        //    tran.invoice_number = "your invoice number which you are generating";

        //    // Now, we have to make a list of transaction and add the transactions object
        //    // to this list. You can create one or more object as per your requirements

        //    List<Transaction> transactions = new List<Transaction>();
        //    transactions.Add(tran);

        //    // Now we need to specify the FundingInstrument of the Payer
        //    // for credit card payments, set the CreditCard which we made above

        //    FundingInstrument fundInstrument = new FundingInstrument();
        //    fundInstrument.credit_card = crdtCard;

        //    // The Payment creation API requires a list of FundingIntrument

        //    List<FundingInstrument> fundingInstrumentList = new List<FundingInstrument>();
        //    fundingInstrumentList.Add(fundInstrument);

        //    // Now create Payer object and assign the fundinginstrument list to the object
        //    Payer payr = new Payer();
        //    payr.funding_instruments = fundingInstrumentList;
        //    payr.payment_method = "credit_card";

        //    // finally create the payment object and assign the payer object & transaction list to it
        //    Payment pymnt = new Payment();
        //    pymnt.intent = "sale";
        //    pymnt.payer = payr;
        //    pymnt.transactions = transactions;

        //    try
        //    {
        //        //getting context from the paypal
        //        //basically we are sending the clientID and clientSecret key in this function
        //        //to the get the context from the paypal API to make the payment
        //        //for which we have created the object above.

        //        //Basically, apiContext object has a accesstoken which is sent by the paypal
        //        //to authenticate the payment to facilitator account.
        //        //An access token could be an alphanumeric string

        //        APIContext apiContext = Configuration.GetAPIContext();

        //        //Create is a Payment class function which actually sends the payment details
        //        //to the paypal API for the payment. The function is passed with the ApiContext
        //        //which we received above.

        //        Payment createdPayment = pymnt.Create(apiContext);

        //        //if the createdPayment.state is "approved" it means the payment was successful else not

        //        if (createdPayment.state.ToLower() != "approved")
        //        {
        //            return View("FailureView");
        //        }
        //    }
        //    catch (PayPal.PayPalException ex)
        //    {
        //        var error = ex;
        //        return View("FailureView");
        //    }

        //    return View("SuccessView");
        //}
    }
}
