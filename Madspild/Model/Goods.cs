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
    public class Goods : IDataErrorInfo, IComparable<Goods>
    {
        public string Id { get; set; }    // repræsenterer Id
        public string Name { get; set; }    // repræsenterer Name
        public string Price { get; set; }
        public string Amount { get; set; }
        public string AmountLimit { get; set; }
        public string Category { get; set; }
        public string Path { get; set; }


        public Goods()
        {
            Id = "";
            Name = "";
            Price = "";
            Amount = "";
            AmountLimit = "";
            Category = "";
            Path = "";

        }

        public Goods(string id, string name, string price, string amount, string amountLimit, string category, string path)
        {
            Id = id;
            Name = name;
            Price = price;
            Amount = amount;
            AmountLimit = amountLimit;
            Category = category;
            Path = path;
        }

        public override bool Equals(object obj)
        {
            try
            {
                Goods product = (Goods)obj;
                return Id.Equals(product.Id);
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

        public int CompareTo(Goods produkt)
        {
            return Id.CompareTo(produkt.Id);
        }

        private static readonly string[] validatedProperties = {"Name", "Price", "Amount", "AmountLimit", "Category", "Path" };

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
                case "Name": return ValidateName();
                case "Price": return ValidatePrice();
                case "Amount": return ValidateAmount();
                case "AmountLimit": return ValidateLimit();
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

        private string ValidatePrice()
        {
            foreach (char c in Price) if (c < '0' || c > '9') return "Price limit must be a number";
            return null;
        }

        private string ValidateAmount()
        {
            foreach (char c in Amount) if (c < '0' || c > '9') return "Amount  must be a number";
            return null;
        }

        private string ValidateLimit()
        {
            foreach (char c in AmountLimit) if (c < '0' || c > '9') return "Amount limit must be a number";
            return null;
        }

    }
}
