using ImfuyoRanch.Models.Business;
using ImfuyoRanch.Models;
using Microsoft.AspNet.Identity;
using ImfuyoRanch.Models.HelperToast;
using ImfuyoRanch.Controllers.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Net.Mail;
using ImfuyoRanch.ImfuyoRanchLogic;
using Microsoft.Ajax.Utilities;

namespace ImfuyoRanch.Controllers
{
    public class ShoppingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private CartBusiness cart_Service = new CartBusiness();
        private ItemBusiness item_Service = new ItemBusiness();
        private Order_Business Order_Business = new Order_Business();
        private DepartmentBusiness department_Service;

        public ActionResult Index(int? id)
        {
            var items_results = new List<Item>();
            try
            {
                if (id != null)
                {
                    if (id == 0)
                    {
                        items_results = item_Service.GetItems().ToList();
                        ViewBag.Department = "All Categories";
                    }
                    else
                    {
                        int notnull = Convert.ToInt16(id);
                        items_results = item_Service.GetItems().Where(x => x.Department.Department_ID == (int)id).ToList();
                        ViewBag.Department = department_Service.GetDepartment(notnull).Department_Name;
                    }
                }
                else
                {
                    items_results = item_Service.GetItems().ToList();
                    ViewBag.Department = "All Categories";
                }
            }
            catch (Exception) { }
            return View(items_results);
        }
        public ActionResult increaseItemQuantity(string id)
        {
            var item = cart_Service.GetCartItems().FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                cart_Service.increaseItemQuantity(id: id);
                this.AddToastMessage("Success!", item.Item.Name + " quantity has been increased", ToastType.Success);
                return RedirectToAction("ShoppingCart");
            }
            else
                this.AddToastMessage("Item Error!", "An unexpected error has occured", ToastType.Error);
            return RedirectToAction("ShoppingCart");
        }
        public ActionResult decreaseItemQuantity(string id)
        {
            var item = cart_Service.GetCartItems().FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                cart_Service.decreaseItemQuantity(id: id);
                this.AddToastMessage("Success!", item.Item.Name + " quantity has been decreased", ToastType.Success);
                return RedirectToAction("ShoppingCart");
            }
            else
                this.AddToastMessage("Item Error!", " An unexpected error has occured", ToastType.Error);
            return RedirectToAction("ShoppingCart");
        }
        public ActionResult add_to_cart(int id)
        {
            var UserName = User.Identity.GetUserName();
            int qty = 1;
            var item = item_Service.GetItem(id);
            CartItem ct = new CartItem();
            if (item != null)
            {
                cart_Service.UpdateQuantity(id, qty);
                //cart_Service.UpdateCart(item.ItemCode,item.Cart_Items.);
                cart_Service.AddItemToCart(id, UserName);
                this.AddToastMessage("Success!", item.Name + " quantity has been added", ToastType.Success);
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Not_Found", "Error");
        }
        public ActionResult remove_from_cart(string id)
        {
            var item = cart_Service.GetCartItems().FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                cart_Service.RemoveItemFromCart(id: id);
                this.AddToastMessage("Success!", item.Item.Name + "  has been removed", ToastType.Success);
                return RedirectToAction("ShoppingCart");
            }
            else
                return RedirectToAction("Not_Found", "Error");
        }
        public ActionResult ShoppingCart()
        {
            ViewBag.Total = cart_Service.GetCartTotal(cart_Service.GetCartID());
            ViewBag.TotalQTY = cart_Service.GetCartItems().FindAll(x => x.cartId == cart_Service.GetCartID()).Sum(q => q.quantity);
            return View(cart_Service.GetCartItems().FindAll(x => x.cartId == cart_Service.GetCartID()));
        }
        [HttpPost]
        public ActionResult ShoppingCart(List<CartItem> items)
        {
            foreach (var i in items)
            {
                cart_Service.UpdateCart(i.Id, i.quantity);
            }
            return RedirectToAction("ShoppingCart");
        } 
        public ActionResult EmptyCart(List<CartItem> items)
        {
            cart_Service.EmptyCart();
            
            return RedirectToAction("ShoppingCart");
        }
        public ActionResult countCartItems()
        {
            var username = User.Identity.GetUserName();
            int qty = cart_Service.GetCartItems().Sum(x => x.quantity);
            return Content(qty.ToString());
        }
        public ActionResult Checkout()
        {
            if (cart_Service.GetCartItems().Count == 0)
            {
                ViewBag.Err = "Opps... you should have atleast one cart item, please shop a few items";
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("HowToGetMyOrder");
        }
        [Authorize]
        public ActionResult HowToGetMyOrder()
        {
            return View();
        }
        [HttpPost]
        public ActionResult HowToGetMyOrder(FormCollection formCollection)
        {
           /* Session["street_number"]*/ string streetNume = formCollection["StreetNumber"];
            /*Session["street_name"] */ string streetName = formCollection["street_name"];
            Session["City"] = formCollection["City"];
            Session["State"] = formCollection["State"];
            Session["ZipCode"] = formCollection["ZipCode"];
            Session["Country"] = formCollection["Country"];
            return RedirectToAction("PlaceOrder", new { id = "deliver" });
        }
        [Authorize]
        public ActionResult PlaceOrder(string id)
        {
            /* Find the details of the customer placing the order*/
            ApplicationDbContext dataContext = new ApplicationDbContext();
            var user = dataContext.Users.ToList().Find(x=>x.Email == HttpContext.User.Identity.Name);
            var customer = new ApplicationUser(); 
            /* Place the order */
            dataContext.Orders.Add(new Order()
            {
                Email = user.Email,
                dateCraeted = DateTime.Now,
                shipped = false,
                status = "Awaiting Approval",
                TotalPrice = cart_Service.GetCartTotal(cart_Service.GetCartID())
            });

            dataContext.SaveChanges();
            var order = dataContext.Orders.ToList()
                .FindAll(x => x.Email == user.Email)
                .OrderByDescending(x => x.dateCraeted)
                .FirstOrDefault();
            /* If the customer requests delivery, save order address */
            if (id == "deliver")
            {
                dataContext.SaveChanges();
                try
                {
                    dataContext.OrderAddresses.Add(new OrderAddress()
                    {
                        OrderNo = order.OrderNo,
                        street = Session["street_name"].ToString(),
                        city = Session["City"].ToString(),
                        zipcode = Session["ZipCode"].ToString()
                        
                    });
                }
                catch (Exception x)
                {
                   var m = x.Message;
                }
            }
            /* Migrate cart items to map as order items */
            //   order_Service.AddOrderItems(order, cart_Service.GetCartItems());
            var items = cart_Service.GetCartItems();

            foreach (var item in items)
            {
                var x = new OrderItem()
                {
                    Order_ID = order.OrderNo,
                    ItemCode = item.ItemdId,
                    quantity = item.quantity,
                    price = item.price
                   
                };


                //   ob.updateStock_bot(x.item_id, x.quantity);
                // db.OrderItems.Add(x);
                // db.SaveChanges();
                dataContext.OrderItems.Add(x);
            }
            dataContext.SaveChanges();
            /* Empty the cart items */
            cart_Service.EmptyCart();
            /* Update Order Tracking Report */
            dataContext.Order_Trackings.Add(new OrderTracking()
            {
                orderNo = order.OrderNo,
                date = DateTime.Now,
                status = "Awaiting Payment",
                Recipient = User.Identity.GetUserName()
            });;
            dataContext.SaveChanges();
            //Redirect to payment
             return RedirectToAction("Payment", new { id = order.OrderNo });
           // return RedirectToAction("Payment",new { id = order.OrderNo });
        }
        public ActionResult Payment(int id)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            OrderDetailConFirm odcf = new OrderDetailConFirm();
            Order orderT = context.Orders.Where(l=> l.OrderNo== id).FirstOrDefault();
            odcf.order = orderT;
            odcf.address = context.OrderAddresses.ToList().Find(x => x.OrderNo == orderT.OrderNo);
            ViewBag.Items = context.OrderItems.ToList().FindAll(x => x.Order_ID == orderT.OrderNo).ToList();

     
            return View(odcf);
        }
        public ActionResult Secure_Payment(int id)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var order = context.Orders.Where(l => l.OrderNo == id).FirstOrDefault();
            return Redirect(PaymentLink(cart_Service.GetCartTotal(order.OrderNo.ToString()).ToString(), "Order Payment | Order No: " + order.OrderNo, order.OrderNo.ToString()));
        }
        
        public ActionResult Payment_Successfull(string id)
        {


            ApplicationDbContext context = new ApplicationDbContext();

            try
            {
                var order = context.Orders.Where(l => l.CustNo == id).FirstOrDefault();
                var items = context.OrderItems.Where(x => x.Order_ID == order.OrderNo).ToList();
                foreach (var tem in items)
                {
                    cart_Service.UpdateQuantity(tem.Item.ItemCode,tem.ItemCode);
                }
                OrderDetailConFirm odcf = new OrderDetailConFirm();
                Order orderT = context.Orders.Where(l => l.CustNo == id).FirstOrDefault();
                odcf.order = orderT;
           
            }
            catch (Exception ex) { }
            return View();
        }


        public ActionResult ConfirmQoute(int? id)
        {
            var OrderItem = ImfuyoLogic.GetItemDetails(id);
            var attachments = new List<Attachment>();
            attachments.Add(new Attachment(new MemoryStream(GeneratePDF(id)), "Qoutation Proof", "application/pdf"));
            var mailTo = new List<MailAddress>();
            mailTo.Add(new MailAddress(ImfuyoLogic.GetSupplierEmail(id), "Sphilile"));
            var body = $"Hello {OrderItem.Supplier.SupplierName}, \n\n {OrderItem.Supplier.SupplierName}" +
                $" it is that time of the year again where we need your help with the stock attached on the document.Please get back to us and confirm if" +
                $"you will be able to supply us with the stock .Have a wonderfull day. Please Find the attached" +
                $" Qoutation Iformation(Qoutation Deatils)<br/> Regards,<br/><br/> Imfuyo Ranch RSA <br/> .";

            ImfuyoRanch.Models.EmailService emailService = new ImfuyoRanch.Models.EmailService();
            emailService.SendEmail(new EmailContent()
            {
                mailTo = mailTo,
                mailCc = new List<MailAddress>(),
                mailSubject = "Qoutation Details | Ref No.:" + id,
                mailBody = body,
                mailFooter = "<br/> Many Thanks, <br/> <b>Imfuyo Ranch</b>",
                mailPriority = MailPriority.High,
                mailAttachments = attachments

            });

            var orderItems = db.OrderItems.Where(x => x.Order_ID == id).ToList();
            var orderDetails = new OrderDetails();
            double total = 0;
            foreach (var item in orderItems)
            {
                orderDetails.OrderId = item.Order_ID;
                orderDetails.ItemId = item.ItemCode;
                orderDetails.ItemName = item.Item.Name;
                orderDetails.ManagerEmail = User.Identity.GetUserName();
                orderDetails.Quantity = item.quantity;
                orderDetails.UnitPrice = (decimal)item.price;
                db.OrderDetails.Add(orderDetails);
                db.SaveChanges();
                total += item.price;
            }


            // Create Order Tracking Data witn Date

            var customerOrder = new CustomerOrder();
            customerOrder.UserOrder = User.Identity.GetUserName();
            customerOrder.OrderDate = DateTime.Now.Date.ToString();
            customerOrder.Status = "Waiting Approval";
            customerOrder.Total = total;
            customerOrder.OrderNumber = id.ToString();
            db.CustomerOrder.Add(customerOrder);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }






      public byte[] GeneratePDF(int? id)
        {
            MemoryStream memoryStream = new MemoryStream();
            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A5, 0, 0, 0, 0);
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
            document.Open();
            Order orderT = db.Orders.Where(l => l.OrderNo == id).FirstOrDefault();
            OrderItem orderItem = db.OrderItems.Where(x => x.Order_ID == id).FirstOrDefault();
            var qoutItem = ImfuyoLogic.GetItemDetails(id);

            //var tenant1 = db.Tenants.Find(roomBooking.TenantId);


            //var reservation = _iReservationService.Get(Convert.ToInt64(ReservationID));
            //var user = _iUserService.Get(reservation.UserID);

            iTextSharp.text.Font font_heading_3 = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 12, iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.RED);
            iTextSharp.text.Font font_body = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9, iTextSharp.text.BaseColor.BLUE);

            // Create the heading paragraph with the headig font
            PdfPTable table1 = new PdfPTable(1);
            PdfPTable table2 = new PdfPTable(5);
            PdfPTable table3 = new PdfPTable(1);

            iTextSharp.text.pdf.draw.VerticalPositionMark seperator = new iTextSharp.text.pdf.draw.LineSeparator();
            seperator.Offset = -6f;
            // Remove table cell
            table1.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            table3.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

            table1.WidthPercentage = 80;
            table1.SetWidths(new float[] { 100 });
            table2.WidthPercentage = 80;
            table3.SetWidths(new float[] { 100 });
            table3.WidthPercentage = 80;

            PdfPCell cell = new PdfPCell(new Phrase(""));
            cell.Colspan = 3;
            table1.AddCell("\n");
            table1.AddCell(cell);
            table1.AddCell("\n\n");
            table1.AddCell(
                "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t" +
                "Imfuyo Ranch RSA \n" +
                "Email :imfuyoRanchRSA@gmail.com" + "\n" +
                "\n" + "\n");
            table1.AddCell("------------Qoutation Details--------------!");
            foreach (var item in db.OrderItems.Where(x=>x.Order_ID==id))
            {
                table1.AddCell("Item Name : \t" + ImfuyoLogic.GetItemName(item.ItemCode));
                table1.AddCell("Item Quantity : \t" + item.quantity);
                table1.AddCell("Price : \t" + item.price);
            }
            //table1.AddCell("Last Name : \t" + roomBooking.Surname);
            //table1.AddCell("Identity Number : \t" + roomBooking.NID);
            //table1.AddCell("Date Of Birth : \t" + roomBooking.DOB);

            table1.AddCell("\n------------More Details details--------------!\n");

            table1.AddCell("Refernce Number # : \t" + orderItem.Order_ID);
            table1.AddCell("Manager Email : \t" + orderItem.Order.Email);
            table1.AddCell("Proposed Price: \t" + orderItem.Order.TotalPrice);
            //table1.AddCell("Guardian Name : \t" + roomBooking.GuardianName);
            //table1.AddCell("Subject : \t" + roomBooking.subject);

            table1.AddCell("\n");

            table3.AddCell("------------Looking forward to hear from you soon--------------!");

            //////Intergrate information into 1 document
            //var qrCode = iTextSharp.text.Image.GetInstance(reservation.QrCodeImage);
            //qrCode.ScaleToFit(200, 200);
            table1.AddCell(cell);
            document.Add(table1);
            //document.Add(qrCode);
            document.Add(table3);
            document.Close();

            byte[] bytes = memoryStream.ToArray();
            memoryStream.Close();
            return bytes;
        }
        public string PaymentLink(string totalCost, string paymentSubjetc, string order_id)
        {

            string paymentMode = ConfigurationManager.AppSettings["PaymentMode"], site, merchantId, merchantKey, returnUrl, cancelUrl, PF_NotifyURL;

            if (paymentMode == "test")
            {
                site = "https://sandbox.payfast.co.za/eng/process?";
                merchantId = "10017134";
                merchantKey = "dzhxjfvzun252";
            }
            else if (paymentMode == "live")
            {
                site = "https://www.payfast.co.za/eng/process?";
                merchantId = ConfigurationManager.AppSettings["PF_MerchantID"];
                merchantKey = ConfigurationManager.AppSettings["PF_MerchantKey"];
            }
            else
            {
                throw new InvalidOperationException("Payment method unknown.");
            }
            var stringBuilder = new StringBuilder();
            //PF_NotifyURL = Url.Action("Payment_Successfull", "Shopping",
            //    new System.Web.Routing.RouteValueDictionary(new { id = order_id }),
            //    "http", Request.Url.Host);
            //returnUrl = Url.Action("Order_Details", "Orders",
            //    new System.Web.Routing.RouteValueDictionary(new { id = order_id }),
            //    "http", Request.Url.Host);
            //cancelUrl = Url.Action("Index", "Shopping",
            //    new System.Web.Routing.RouteValueDictionary(new { id = order_id }),
            //    "http", Request.Url.Host);

            PF_NotifyURL = ConfigurationManager.AppSettings["NotifyURL"];
            returnUrl =Url.Action();
            cancelUrl = ConfigurationManager.AppSettings["CancelURL"];
            /* mechant details */
            stringBuilder.Append("&merchant_id=" + HttpUtility.HtmlEncode(merchantId));
            stringBuilder.Append("&merchant_key=" + HttpUtility.HtmlEncode(merchantKey));
            stringBuilder.Append("&return_url=" + HttpUtility.HtmlEncode(returnUrl));
            stringBuilder.Append("&cancel_url=" + HttpUtility.HtmlEncode(cancelUrl));
            stringBuilder.Append("&notify_url=" + HttpUtility.HtmlEncode(PF_NotifyURL));
            /* buyer details */
           ApplicationDbContext db = new ApplicationDbContext();
            var customer = db.Orders.Where(x => x.OrderNo.ToString() == order_id).FirstOrDefault();
           

            //db.Orders.FirstOrDefault(x => x.OrderNo.ToString() == order_id).customer;
            if (customer != null)
            {
                stringBuilder.Append("&name_first=" + HttpUtility.HtmlEncode(customer.FirstName));
                stringBuilder.Append("&name_last=" + HttpUtility.HtmlEncode(customer.LastName));
                stringBuilder.Append("&email_address=" + HttpUtility.HtmlEncode(customer.Email));
                //stringBuilder.Append("&cell_number=" + HttpUtility.HtmlEncode(customer.phone));
            }
            /* Transaction details */
            var order = db.Orders.Where(l=>l.OrderNo==Convert.ToInt32(order_id)).FirstOrDefault();
            if (order != null)
            {
               decimal pricetot = (decimal)cart_Service.GetCartTotal(cart_Service.GetCartID());
                stringBuilder.Append("&m_payment_id=" + HttpUtility.HtmlEncode(order.OrderNo));
                stringBuilder.Append("&amount=" + HttpUtility.HtmlEncode((double)order.TotalPrice));
                stringBuilder.Append("&item_name=" + HttpUtility.HtmlEncode(paymentSubjetc));
                stringBuilder.Append("&item_description=" + HttpUtility.HtmlEncode(paymentSubjetc));

                stringBuilder.Append("&email_confirmation=" + HttpUtility.HtmlEncode("1"));
                stringBuilder.Append("&confirmation_address=" + HttpUtility.HtmlEncode(ConfigurationManager.AppSettings["PF_ConfirmationAddress"]));
            }

            return (site + stringBuilder);
        }
       
    }
}