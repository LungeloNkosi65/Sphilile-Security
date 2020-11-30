//using PayFast;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using System.Web.Routing;

//namespace SphileSecurity.Services
//{
//    public class PaymentService:IController
//    {
//        public static bool OnceOffPayment()
//        {
//            return true;
//        }

//        public void OnceOff()
//        {
//            var onceOffRequest = new PayFastRequest(this.payFastSettings.PassPhrase);

//            // Merchant Details
//            onceOffRequest.merchant_id = this.payFastSettings.MerchantId;
//            onceOffRequest.merchant_key = this.payFastSettings.MerchantKey;
//            onceOffRequest.return_url = this.payFastSettings.ReturnUrl;
//            onceOffRequest.cancel_url = this.payFastSettings.CancelUrl;
//            onceOffRequest.notify_url = this.payFastSettings.NotifyUrl;

//            // Buyer Details
//            onceOffRequest.email_address = "sbtu01@payfast.co.za";
//            //double amount = Convert.ToDouble(db.Items.Select(x => x.CostPrice).FirstOrDefault());
//            //var products = db.Items.Select(x => x.Name).ToList();
//            // Transaction Details
//            onceOffRequest.m_payment_id = "";
//            onceOffRequest.amount = 1500;
//            onceOffRequest.item_name = "Once off option";
//            onceOffRequest.item_description = "Some details about the once off payment";


//            // Transaction Options
//            onceOffRequest.email_confirmation = true;
//            onceOffRequest.confirmation_address = "sbtu01@payfast.co.za";

//            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{onceOffRequest.ToString()}";

//            //return Redirect(redirectUrl);
//        }

//        public void AdHoc()
//        {
//            var adHocRequest = new PayFastRequest(this.payFastSettings.PassPhrase);

//            // Merchant Details
//            adHocRequest.merchant_id = this.payFastSettings.MerchantId;
//            adHocRequest.merchant_key = this.payFastSettings.MerchantKey;
//            adHocRequest.return_url = this.payFastSettings.ReturnUrl;
//            adHocRequest.cancel_url = this.payFastSettings.CancelUrl;
//            adHocRequest.notify_url = this.payFastSettings.NotifyUrl;
//              #endregion Methods
//            // Buyer Details
//            adHocRequest.email_address = "sbtu01@payfast.co.za";
//            //double amount = Convert.ToDouble(db.FoodOrders.Select(x => x.Total).FirstOrDefault());
//            //var products = db.FoodOrders.Select(x => x.UserEmail).ToList();
//            // Transaction Details
//            adHocRequest.m_payment_id = "";
//            adHocRequest.amount = 70;
//            adHocRequest.item_name = "Adhoc Agreement";
//            adHocRequest.item_description = "Some details about the adhoc agreement";

//            // Transaction Options
//            adHocRequest.email_confirmation = true;
//            adHocRequest.confirmation_address = "sbtu01@payfast.co.za";

//            // Recurring Billing Details
//            adHocRequest.subscription_type = SubscriptionType.AdHoc;

//            var redirectUrl = $"{this.payFastSettings.ProcessUrl}{adHocRequest.ToString()}";

//            return Redirect(redirectUrl);
//        }

//        public void Return()
//        {
//            //return View();
//        }

//        public void Cancel()
//        {
//            //return View();
//        }

//        [HttpPost]
//        public async Task<ActionResult> Notify([ModelBinder(typeof(PayFastNotifyModelBinder))] PayFastNotify payFastNotifyViewModel)
//        {
//            payFastNotifyViewModel.SetPassPhrase(this.payFastSettings.PassPhrase);

//            var calculatedSignature = payFastNotifyViewModel.GetCalculatedSignature();

//            var isValid = payFastNotifyViewModel.signature == calculatedSignature;

//            System.Diagnostics.Debug.WriteLine($"Signature Validation Result: {isValid}");

//            // The PayFast Validator is still under developement
//            // Its not recommended to rely on this for production use cases
//            var payfastValidator = new PayFastValidator(this.payFastSettings, payFastNotifyViewModel, IPAddress.Parse(this.HttpContext.Request.UserHostAddress));

//            var merchantIdValidationResult = payfastValidator.ValidateMerchantId();

//            System.Diagnostics.Debug.WriteLine($"Merchant Id Validation Result: {merchantIdValidationResult}");

//            var ipAddressValidationResult = payfastValidator.ValidateSourceIp();

//            System.Diagnostics.Debug.WriteLine($"Ip Address Validation Result: {merchantIdValidationResult}");

//            // Currently seems that the data validation only works for successful payments
//            if (payFastNotifyViewModel.payment_status == PayFastStatics.CompletePaymentConfirmation)
//            {
//                var dataValidationResult = await payfastValidator.ValidateData();

//                System.Diagnostics.Debug.WriteLine($"Data Validation Result: {dataValidationResult}");
//            }

//            if (payFastNotifyViewModel.payment_status == PayFastStatics.CancelledPaymentConfirmation)
//            {
//                System.Diagnostics.Debug.WriteLine($"Subscription was cancelled");
//            }

//            return new HttpStatusCodeResult(HttpStatusCode.OK);
//        }

//        public ActionResult Error()
//        {
//            return View();
//        }

//        public void Execute(RequestContext requestContext)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}