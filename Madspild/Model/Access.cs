using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Madspild.Model
{
    public class Access : IDataErrorInfo, IComparable<Access>
    {
        public string Id { get; set; }    // repræsenterer Id
        public string Name { get; set; }    // repræsenterer Name


        public Access()
        {
            Id = "";
            Name = "";
        }

        public Access(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public override bool Equals(object obj)
        {
            try
            {
                Access access = (Access)obj;
                return Id.Equals(access.Id);
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

        public override string ToString()
        {
            return string.Format("{0} {1}", Id, Name);
        }

        public int CompareTo(Access access)
        {
            return Id.CompareTo(access.Id);
        }

        public bool IsValid
        {
            get { return Id != null && Name != null && Name.Trim().Length > 0; }
        }

        string? IDataErrorInfo.Error => IsValid ? null : "Illegal model object";

        string IDataErrorInfo.this[string propertyName] => Validate(propertyName);

        private string? Validate(string property)
        {
            if (property.Equals("AccessName")) return Name != null && Name.Trim().Length > 0 ? null : "Illegal Name";
            return null;
        }

 
    }
}
