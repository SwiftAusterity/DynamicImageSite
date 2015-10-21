using System;
using System.Collections.Generic;
using System.Web.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;

using Ninject;

using Site.Data.API.Repository;
using Site.Models;
using Site.ViewModels;

namespace Site.Controllers
{
    [HandleError]
    public class ContactController : BaseContentController
    {
        [Inject]
        public AccountMembershipService AccountService { get; set; }

        [Inject]
        public IContactRepository ContactRepository { get; set; }

        protected override String ParentSection() { return "Contact"; }
        protected override String IndexTitle() { return "Contact"; }
        protected override String MainClass() { return "contact"; }

        public override ActionResult Index(String section, String page, bool mobile)
        {
            if (!String.IsNullOrEmpty(section) && String.IsNullOrEmpty(page))
            {
                page = section;
                section = String.Empty;
            }

            if(mobile)
                return IndexWithViewModel(new ContactFormMobileViewModel(Kernel, HttpContext), section, page, mobile);
            else
                return IndexWithViewModel(new ContactFormViewModel(Kernel, HttpContext), section, page, mobile);
        }

        //Actions
        public ActionResult SubmitContact(string email, string fullName, string body, bool enewsletter)
        {
            var viewModel = new ContactFormViewModel(Kernel, HttpContext);

            if (ValidateContactForm(viewModel, email, fullName, body, enewsletter))
            {
                //We need to email this.
                SendContact(email, body, fullName);

                ContactRepository.Save(email, body, fullName, enewsletter);

                if (enewsletter)
                    AddToMailChimp(email);

                viewModel.Messages.Add("Your request was successfully submitted.");
                viewModel.Email = viewModel.Body = viewModel.FullName = String.Empty;
                viewModel.eNewsletter = false;
            }

            //send back to the index page, thank them
            return IndexWithViewModel(viewModel, String.Empty, String.Empty, false);
        }

        public ActionResult SubmitSubscribe(string email, string fullName)
        {
            var viewModel = new ContactFormViewModel(Kernel, HttpContext);

            viewModel.Errors =
                viewModel.Errors.Union(AccountService.ValidateContactDetails(email, fullName)).ToList();

            if (viewModel.Errors.Count() <= 0)
            {
                ContactRepository.Save(email, String.Empty, fullName, true);
                AddToMailChimp(email);

                viewModel.Messages.Add("Your request was successfully submitted.");
                viewModel.Email = viewModel.Body = viewModel.FullName = String.Empty;
                viewModel.eNewsletter = false;
            }

            //send back to the index page, thank them
            return IndexWithViewModel(viewModel, String.Empty, String.Empty, false);
        }

        //Mobile Actions
        public ActionResult MobileSubmitContact(string email, string fullName, string body, bool enewsletter)
        {
            var viewModel = new ContactFormMobileViewModel(Kernel, HttpContext);

            if (ValidateContactForm(viewModel, email, fullName, body, enewsletter))
            {
                //We need to email this.
                SendContact(email, body, fullName);

                ContactRepository.Save(email, body, fullName, enewsletter);

                if (enewsletter)
                    AddToMailChimp(email);

                viewModel.Messages.Add("Your request was successfully submitted.");
                viewModel.Email = viewModel.Body = viewModel.FullName = String.Empty;
                viewModel.eNewsletter = false;
            }

            //send back to the index page, thank them
            return IndexWithViewModel(viewModel, String.Empty, String.Empty, true);
        }

        public ActionResult MobileSubmitSubscribe(string email, string fullName)
        {
            var viewModel = new ContactFormMobileViewModel(Kernel, HttpContext);

            viewModel.Errors =
                viewModel.Errors.Union(AccountService.ValidateContactDetails(email, fullName)).ToList();

            if (viewModel.Errors.Count() <= 0)
            {
                ContactRepository.Save(email, String.Empty, fullName, true);
                AddToMailChimp(email);

                viewModel.Messages.Add("Your request was successfully submitted.");
                viewModel.Email = viewModel.Body = viewModel.FullName = String.Empty;
                viewModel.eNewsletter = false;
            }

            //send back to the index page, thank them
            return IndexWithViewModel(viewModel, String.Empty, String.Empty, true);
        }


        public JsonPResult AjaxSubmitContact(string email, string fullName, string body, bool enewsletter, string callback)
        {
            var viewModel = new ContactFormViewModel(Kernel, HttpContext);

            if (ValidateContactForm(viewModel, email, fullName, body, enewsletter))
            {
                //We need to email this.
                SendContact(email, body, fullName);

                ContactRepository.Save(email, body, fullName, enewsletter);

                if (enewsletter)
                    AddToMailChimp(email);
            }
            else
                return new JsonPResult(callback, Json(new { success = "false", errors = viewModel.Errors }));

            //send back to the index page, thank them
            return new JsonPResult(callback, Json(new { success = "true" }));
        }

        public JsonPResult AjaxSubmitSubscribe(string email, string fullName, string callback)
        {
            var viewModel = new ContactFormViewModel(Kernel, HttpContext);

            viewModel.Errors =
                viewModel.Errors.Union(AccountService.ValidateContactDetails(email, fullName)).ToList();

            if (viewModel.Errors.Count() <= 0)
            {
                ContactRepository.Save(email, String.Empty, fullName, true);

                AddToMailChimp(email);
            }
            else
                return new JsonPResult(callback, Json(new { success = "false", errors = viewModel.Errors }));

            //send back to the index page, thank them
            return new JsonPResult(callback, Json(new { success = "true" }));
        }

        private bool ValidateContactForm(BaseViewModel viewModel, string email, string fullName, string body, bool subscribe)
        {
            viewModel.Errors =
                viewModel.Errors.Union(AccountService.ValidateContactDetails(email, fullName)).ToList();

            if (String.IsNullOrEmpty(body))
                viewModel.Errors.Add("Please include some text in the body.");

            return viewModel.Errors.Count() == 0;
        }

        public void SendContact(string fromEmail, string desc, string name)
        {
            StringBuilder body = new StringBuilder();

            body.AppendLine("From: " + fromEmail);
            body.AppendLine("Name: " + name);
            body.AppendLine("Body: ");
            body.AppendLine(desc);

            MailAddress from = new MailAddress(fromEmail, String.Format("Infuz.com Contact: {0}", name));
            MailAddress to = new MailAddress("inquiries@infuz.com", "Infuz.com Contact Form");
            MailMessage email = new MailMessage(from, to);
            email.Subject = "New contact from: " + name;
            email.Body = body.ToString();

            email.IsBodyHtml = false;

            SendEmail(email);
        }

        public void SendEmail(MailMessage email)
        {
            var emailServer = WebConfigurationManager.AppSettings["smtpServer"].ToString();

            var smtp = new SmtpClient(emailServer, 587);
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential("noreply@infuz.com", "lrgv wmrt wund ssah", "");

            try { smtp.Send(email); }
            catch { } //Maybe store it in a file or something if emailing dies? }
        }

        public bool AddToMailChimp(String address)
        {
            var mergeVars = new Dictionary<String, object>();
            var input = new PerceptiveMCAPI.Types.listSubscribeInput("a66afdfa38637673991abf1ca2800d71-us1", "3d28f3ecab", address, mergeVars);
            var api = new PerceptiveMCAPI.Methods.listSubscribe(input);
            var result = api.Execute();
            return result.result;
        }
    }
}
