using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Madspild.Model
{
    public class Basket : IDataErrorInfo, IComparable<Basket>
    {
        public string Id { get; set; }    // repræsenterer Id
        public string PersonEmail { get; set; }    // repræsenterer Name
        public string ProductName { get; set; }
        public string Amount { get; set; }
        public string Price { get; set; }
        public string BacketDato { get; set; }
        public string BoughtDato { get; set; }


        public Basket()
        {
            Id = "";
            PersonEmail = "";
            ProductName = "";
            Amount = "";
            Price = "";
            BacketDato = "";
            BoughtDato = "";
        }

        public Basket(string id, string personEmail, string productName, string amount, string price, string backetDato, string boughtDato)
        {
            Id = id;
            PersonEmail = personEmail;
            ProductName = productName;
            Amount = amount;
            Price = price;
            BacketDato = backetDato;
            BoughtDato = boughtDato;
        }

        public override bool Equals(object obj)
        {
            try
            {
                Basket basket = (Basket)obj;
                return Id.Equals(basket.Id);
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

        public int CompareTo(Basket basket)
        {
            return Id.CompareTo(basket.Id);
        }

        private static readonly string[] validatedProperties = { "PersonEmail", "ProductName", "Amount", "Price", "backetDato", "boughtDato" };

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

        private string? Validate(string name)
        {
            switch (name)
            {
                case "Email": return ValidateEmail();
                case "Name": return ValidateName();
                case "Price": return ValidatePrice();
                case "Amount": return ValidateAmount();
            }
            return null;
        }

        private string ValidateName()
        {
            if (ProductName == null)
            {
                return "Product Name can not be null";
            }
            return null;
        }
        private string ValidateEmail()
        {
            if (PersonEmail == null)
            {
                return "Email can not be null";
            }
            return null;
        }

        private string ValidatePrice()
        {
            foreach (char c in Price) if (c < '0' || c > '9') return "Price limit must be a number";
            return null;
        }
        private string ValidateAmount()
        {
            foreach (char c in Price) if (c < '0' || c > '9') return "Amount limit must be a number";
            return null;
        }

    }
}
