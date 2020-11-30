using SphileSecurity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace SphileSecurity.Services
{
    public class EmailSender
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static void SendSubscriptionConfrimations(PackageSubscription packageSubscription)
        {
            var mailTo = new List<MailAddress>();
            mailTo.Add(new MailAddress(packageSubscription.CustomerEmail, GetCustomerName(packageSubscription.CustomerEmail)));
            var body = $"Good Day {GetCustomerName(packageSubscription.CustomerEmail)}," +
                $" Your subscription has bee recieved all you have to do now is pay your subscription fee whic is {packageSubscription.SubscriptionFee} to make things official." +
                $" < br /> Once you have paid your subscription status will change from  {packageSubscription.SubStatus} to paid and active." +
                $"<br/> This email confrims your subscription, if you have anny further enquiries feel free to contact us.";

            Models.EmailService emailService = new Models.EmailService();
            emailService.SendEmail(new EmailContent()
            {
                mailTo = mailTo,
                mailCc = new List<MailAddress>(),
                mailSubject = $"Subscription Confirmation ({GetPackageName(packageSubscription.SecurityPackageId).PackageType.PackageName})!!  | Ref No.:" + packageSubscription.SubReference,
                mailBody = body,
                mailFooter = $"<br/> Kind Regards, <br/> <b>Sphile Security Team </b>",
                mailPriority = MailPriority.High,
                mailAttachments = new List<Attachment>()

            });
        }

        public static string GetCustomerName(string customeEmail)
        {
            var name = (from customer in db.Customers
                        where customer.Email == customeEmail
                        select customer.FirstName).FirstOrDefault();
            return name;
        }

        public static SecurityPackage GetPackageName(int? id)
        {
            var packageName = db.SecurityPackages.Find(id);
            return packageName;

        }


        public static void PayementDone(PackageSubscription packageSubscription)
        {
            var mailTo = new List<MailAddress>();
            mailTo.Add(new MailAddress(packageSubscription.CustomerEmail, GetCustomerName(packageSubscription.CustomerEmail)));
            var body = $"Good Day {GetCustomerName(packageSubscription.CustomerEmail)}," +
                $" Your subscription fee has bee recieved thank you." +
                $"<br/> This email confrims your subscription fee payment, if you have anny further enquiries feel free to contact us.";

            Models.EmailService emailService = new Models.EmailService();
            emailService.SendEmail(new EmailContent()
            {
                mailTo = mailTo,
                mailCc = new List<MailAddress>(),
                mailSubject = $"Subscription Fee Confirmation ({GetPackageName(packageSubscription.SecurityPackageId).PackageType.PackageName})!!  | Ref No.:" + packageSubscription.SubReference,
                mailBody = body,
                mailFooter = $"<br/> Kind Regards, <br/> <b>Sphile Security Team </b>",
                mailPriority = MailPriority.High,
                mailAttachments = new List<Attachment>()

            });
        }

        public static void SubscriptionCancelled(PackageSubscription packageSubscription)
        {
            var mailTo = new List<MailAddress>();
            mailTo.Add(new MailAddress(packageSubscription.CustomerEmail, GetCustomerName(packageSubscription.CustomerEmail)));
            var body = $"Good Day {GetCustomerName(packageSubscription.CustomerEmail)}," +
                $" We regeret to see you leave us but unfortunately we have to say good by and thank you for trusting us with your saftey" +
                $"<br/> This email confrims your subscription has been cancelled, if you have anny further enquiries feel free to contact us.";

            Models.EmailService emailService = new Models.EmailService();
            emailService.SendEmail(new EmailContent()
            {
                mailTo = mailTo,
                mailCc = new List<MailAddress>(),
                mailSubject = $"Subscription  Cancelled({GetPackageName(packageSubscription.SecurityPackageId).PackageType.PackageName})!!  | Ref No.:" + packageSubscription.SubReference,
                mailBody = body,
                mailFooter = $"<br/> Kind Regards, <br/> <b>Sphile Security Team </b>",
                mailPriority = MailPriority.High,
                mailAttachments = new List<Attachment>()

            });
        } 
        public static void LeaveApplication(LeaveApplication leaveApplication)
        {
            var mailTo = new List<MailAddress>();
            mailTo.Add(new MailAddress(leaveApplication.EmployeeEmail, GetCustomerName(leaveApplication.EmployeeEmail)));
            var body = $"Good Day {GetCustomerName(leaveApplication.EmployeeEmail)}," +
                $" We have received your leave application" +
                $"<br/> This email confirms your application and the status of the application process";

            Models.EmailService emailService = new Models.EmailService();
            emailService.SendEmail(new EmailContent()
            {
                mailTo = mailTo,
                mailCc = new List<MailAddress>(),
                mailSubject = $"Leave Application : ({leaveApplication.LeaveApplicationStatus})!!  | Ref No.:" + leaveApplication.EmployeeEmail,
                mailBody = body,
                mailFooter = $"<br/> Kind Regards, <br/> <b>Sphile Security Team </b>",
                mailPriority = MailPriority.High,
                mailAttachments = new List<Attachment>()

            });
        }   
        public static void SecurityHIre(SecurityHire securityHire)
        {
            var mailTo = new List<MailAddress>();
            mailTo.Add(new MailAddress(securityHire.UserEmail, GetCustomerName(securityHire.UserEmail)));
            var body = $"Good Day {GetCustomerName(securityHire.UserEmail)}," +
                $" We have received your hire request." +
                $"<br/> This email confirms your security hire process";

            Models.EmailService emailService = new Models.EmailService();
            emailService.SendEmail(new EmailContent()
            {
                mailTo = mailTo,
                mailCc = new List<MailAddress>(),
                mailSubject = $"Security Hire Process : ({securityHire.Status})!!  | Ref No.:" + securityHire.UserEmail,
                mailBody = body,
                mailFooter = $"<br/> Kind Regards, <br/> <b>Sphile Security Team </b>",
                mailPriority = MailPriority.High,
                mailAttachments = new List<Attachment>()

            });
        }

    }
}