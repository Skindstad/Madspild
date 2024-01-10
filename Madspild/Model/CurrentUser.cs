using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Madspild.Model
{
    public class CurrentUser : IDataErrorInfo, IComparable<CurrentUser>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Access { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string Address { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }

        public CurrentUser()
        {
            Id = "";
            Name = "";
            Email = "";
            Password = "";
            Access = "";
            HomePhone = "";
            WorkPhone = "";
            Address = "";
            Zipcode = "";
            City = "";
        }

        public CurrentUser(string id, string name, string email, string password, string access, string homePhone, string workPhone, string address, string zipcode, string city)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
            Access = access;
            HomePhone = homePhone;
            WorkPhone = workPhone;
            Address = address;
            Zipcode = zipcode;
            City = city;
        }

        public override bool Equals(object? obj)
        {
            try
            {
                CurrentUser user = (CurrentUser)obj;
                return Id.Equals(user.Id);
            }
            catch
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public int CompareTo(CurrentUser user)
        {
            return Id.CompareTo(user.Id);
        }

        private static readonly string[] validatedProperties = { "Name", "Email", "Password", "Access", "HomePhone", "WorkPhone", "Address", "Zipcode" };

        public bool IsValid
        {
            get
            {
                foreach (string property in validatedProperties) if (GetError(property) != null) return false;
                return true;
            }
        }

        string IDataErrorInfo.Error
        {
            get { return IsValid ? null : "Illegal model object"; }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get { return Validate(propertyName); }
        }

        private string GetError(string name)
        {
            foreach (string property in validatedProperties) if (property.Equals(name)) return Validate(name);
            return null;
        }

        public string Validate(string name)
        {
            switch (name)
            {
                case "Name": return ValidateName();
                case "Email": return ValidateEmail();
                case "Password": return ValidatePassword();
                case "Access": return ValidateAccess();
                case "HomePhone": return ValidateHomePhone();
                case "WorkPhone": return ValidateWorkPhone();
                case "Address": return ValidateAddress();
                case "Zipcode": return ValidateZipcode();
                case "City": return ValidateCity();
            }
            return null;
        }

        private string ValidateName()
        {
            if (Name == null)
            {
                return "Name can not be null";
            }
            return null;
        }


        private string ValidateEmail()
        {
            if (Email == null)
            {
                return "Email can not be null";
            }
            return null;
        }

        private string ValidatePassword()
        {
            if (Password == null)
            {
                return "Password can not be null";
            }
            return null;
        }

        private string ValidateAccess()
        {
            if (Access == null)
            {
                return "Access Right can not be null";
            }
            return null;
        }


        private string ValidateHomePhone()
        {
            if (HomePhone.Length != 8)
                return "Phone must be a number of 8 digits in length";
            foreach (char c in HomePhone) if (c < '0' || c > '9') return "Phone must be a number of 8 digits in length";
            return null;
        }

        private string ValidateWorkPhone()
        {
            if (WorkPhone.Length != 10)
                return "Phone must be a number of 8 digits in length";
            foreach (char c in WorkPhone) if (c < '0' || c > '9') return "Phone must be a number of 8 digits in length";
            return null;
        }

        private string ValidateAddress()
        {
            if (Address == null)
            {
                return "Address can not be null";
            }
            return null;
        }

        private string ValidateZipcode()
        {
            if (Zipcode.Length != 4) return "Zipcode must be a number of 4 digits";
            foreach (char c in Zipcode) if (c < '0' || c > '9') return "Zipcode must be a number of 4 digits";
            return null;
        }
        private string ValidateCity()
        {
            if (City == null || City.Length == 0) return "City can not be empty";
            return null;
        }

    }
}
