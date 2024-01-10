using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Madspild.Model
{
    public class Bought : IDataErrorInfo, IComparable<Bought>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Amount { get; set; }
        public string Total { get; set; }
        public string BoughtDato { get; set; }

        public Bought()
        {
            Id = "";
            Name = "";
            Email = "";
            Amount = "";
            Total = "";
            BoughtDato = "";
        }
        public Bought(string id, string name, string email, string amount, string total, string boughtDato)
        {
            Id = id;
            Name = name;
            Email = email;
            Amount = amount;
            Total = total;
            BoughtDato = boughtDato;
        }

        public override bool Equals(object? obj)
        {
            try
            {
                Bought bought = (Bought)obj;
                return Id.Equals(bought.Id);
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

        public int CompareTo(Bought bought)
        {
            return Id.CompareTo(bought.Id);
        }

        private static readonly string[] validatedProperties = { "Name", "Email", "Amount", "Total", "BoughtDato"};

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
                case "Amount": return ValidateAmount();
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

        private string ValidateAmount()
        {
            if (Amount == null)
            {
                return "Amount can not be null";
            }
            return null;
        }

        private string ValidateTotal()
        {
            if (Total == null)
            {
                return "Total can not be null";
            }
            return null;
        }

    }
}
