using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using Site.Data.API.Repository;
using Site.Data.API;

using Ninject;

namespace Site.Models
{
    #region Services
    // The FormsAuthentication type is sealed and contains static members, so it is difficult to
    // unit test code that calls its members. The interface and helper class below demonstrate
    // how to create an abstract wrapper around such a type in order to make the AccountController
    // code unit testable.

    public interface IMembershipService
    {
        int MinPasswordLength { get; }

        bool ValidateUser(string userName, string password);
    }

    public class AccountMembershipService
    {
        [Inject]
        public IKernel Kernel { get; set; }

        [Inject]
        public IUserRepository UserRepository { get; set; }

        private readonly PromoMembershipProvider _provider;

        public AccountMembershipService(IKernel kernel)
        {
            kernel.Inject(this);
            _provider = new PromoMembershipProvider(Kernel);
        }

        public int MinPasswordLength
        {
            get
            {
                return 6;
            }
        }

        public Guid ValidateUser(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (string.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");

            var returnValue = Guid.Empty;

            try
            {
                //var user = UserRepository.Get(userName);

                //returnValue = _provider.ValidateUser(user, HashPass(password, user.PasswordSalt));
            }
            catch
            {
                returnValue = Guid.Empty;
            }

            return returnValue;
        }

        public string HashPass(string password, string passwordSalt)
        {
            var encoder = new UTF8Encoding();
            var sha1Hasher = new SHA1CryptoServiceProvider();

            sha1Hasher.ComputeHash(encoder.GetBytes(password + passwordSalt));

            return BitConverter.ToString(sha1Hasher.Hash).ToLower().Replace("-", string.Empty);
        }

        public IEnumerable<string> ValidateContactDetails(string email, string name)
        {
            var errorsList = new List<string>();

            if (string.IsNullOrEmpty(name))
                errorsList.Add("Please submit your name.");
            else if (name.Length > 100)
                errorsList.Add("Please limit name to 100 characters.");

            if (string.IsNullOrEmpty(email))
                errorsList.Add("Please submit your email address.");
            else if (email.Length > 255)
                errorsList.Add("Please limit email address to 255 characters.");
            else if (!IsValidEmailAddress(email))
                errorsList.Add("Please submit an actual email address (e.g. name@site.com).");

            return errorsList;
        }


        public IEnumerable<string> ValidateAccountDetails(string email, string password, string confirmPassword, string firstName, string lastName
            , string jobTitle, string companyName, string phone, string address, string address2, string city, string state, string zipcode)
        {
            var errorsList = new List<string>();
            if (string.IsNullOrEmpty(email))
                errorsList.Add("Please submit your email address.");
            else if (email.Length > 255)
                errorsList.Add("Please limit email address to 255 characters.");
            else if (!IsValidEmailAddress(email))
                errorsList.Add("Please submit an actual email address (e.g. name@site.com).");

            if (string.IsNullOrEmpty(password))
                errorsList.Add("Please submit your password.");
            else if (password.Length < 6 || password.Length > 16)
                errorsList.Add("Please limit passwords to between 6 and 16 characters.");

            if (string.IsNullOrEmpty(confirmPassword))
                errorsList.Add("Please confirm your password.");

            if (string.IsNullOrEmpty(firstName))
                errorsList.Add("Please submit your first name.");
            else if (firstName.Length > 50)
                errorsList.Add("Please limit first name to 50 characters.");

            if (string.IsNullOrEmpty(lastName))
                errorsList.Add("Please submit your last name.");
            else if (lastName.Length > 50)
                errorsList.Add("Please limit last name to 50 characters.");

            if (string.IsNullOrEmpty(jobTitle))
                errorsList.Add("Please submit your job title.");
            else if (jobTitle.Length > 100)
                errorsList.Add("Please limit job title to 100 characters.");

            if (string.IsNullOrEmpty(companyName))
                errorsList.Add("Please submit your company name.");
            else if (companyName.Length > 100)
                errorsList.Add("Please limit company name to 100 characters.");

            if (string.IsNullOrEmpty(phone))
                errorsList.Add("Please submit your phone number.");
            else if (!IsValidPhoneNumber(phone))
                errorsList.Add("Please submit a valid phone number.");

            if (string.IsNullOrEmpty(address))
                errorsList.Add("Please submit your address.");
            else if (address.Length > 100)
                errorsList.Add("Please limit address to 100 characters.");

            if (address2.Length > 100)
                errorsList.Add("Please limit address2 to 100 characters.");

            if (string.IsNullOrEmpty(city))
                errorsList.Add("Please submit your city.");
            else if (city.Length > 100)
                errorsList.Add("Please limit city to 100 characters.");

            if (string.IsNullOrEmpty(state))
                errorsList.Add("Please submit your state.");

            if (string.IsNullOrEmpty(zipcode))
                errorsList.Add("Please submit your zipcode.");
            else if (!IsValidZipcode(zipcode))
                errorsList.Add("Please submit a valid zipcode.");

            return errorsList;
        }

        private bool IsValidEmailAddress(string email)
        {
            return new System.Text.RegularExpressions
                .Regex("^[a-zA-Z0-9._%-+]+@[a-zA-Z0-9.-]+[.][a-zA-Z]{2,4}$")
                .IsMatch(email);
        }

        private bool IsValidZipcode(string zip)
        {
            return new System.Text.RegularExpressions
                .Regex(@"^\d{5}([-]?\d{4})?$")
                .IsMatch(zip);
        }

        private bool IsValidPhoneNumber(string phone) 
        { 
            return new System.Text.RegularExpressions
                .Regex(@"^(1\s*[-\/\.]?)?(\((\d{3})\)|(\d{3}))\s*[-\/\.]?\s*(\d{3})\s*[-\/\.]?\s*(\d{4})\s*(([xX]|[eE][xX][tT])\.?\s*(\d+))*$")
                .IsMatch(phone);
        }

    }

    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (string.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");

            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
    #endregion

    public class PromoMembershipProvider
    {
        public PromoMembershipProvider(IKernel kernel)
        {
            kernel.Inject(this);
        }

        public Guid ValidateUser(IUser user, string passwordHash)
        {
            //if (!passwordHash.Equals(user.PasswordHash))
            //    return Guid.Empty;

            return user.ID;
        }
    }
}
