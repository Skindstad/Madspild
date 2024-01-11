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
        public string PersonEmail { get; set; }    // repræsenterer Email
        public string ProductName { get; set; } // repræsenterer Name
        public string Amount { get; set; } // repræsenterer Amount
        public string Price { get; set; } // repræsenterer Price
        public string BacketDato { get; set; } // repræsenterer Basket Dato
        public string BoughtDato { get; set; } // repræsenterer Bought Dato

        // Opretter et objekt ved at sætte alle felter til blank.
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
        // Opretter et objekt, hvor alle felter initialiseres med parametre.
        // Konstruktøren garanterer ikke, at objektet er lovligt.
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
        // Implementerer sammenligning alene på Id.
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
        // Validering af objektet.
        // Arrayet angiver hvilke properties, der skal valideres.
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
        // Valideringsmetoder til de enkelte properties.
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
