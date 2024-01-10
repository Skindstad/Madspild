using Madspild.DataAccess;
using Madspild.Model;
using Madspild.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Madspild.ViewModel
{
    internal class ForsideViewModel : ViewModelBase, IDataErrorInfo
    {
        private Bought bought = new Bought();
        private ObservableCollection<Bought> boughtItems;
        public ForsideViewModel()
        {
            
        }

        public ObservableCollection<Bought> BoughtItems
        {
            get { return boughtItems; }
            set
            {
                if (boughtItems != value)
                {
                    boughtItems = value;
                    OnPropertyChanged("BoughtItems");
                }
            }
        }

        public string Id
        {
            get { return bought?.Id; }
            set
            {
                if (!bought.Id.Equals(value))
                {
                    bought.Id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        public string Name
        {
            get { return bought?.Name; }
            set
            {
                if (!bought.Name.Equals(value))
                {
                    bought.Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string Email
        {
            get { return bought?.Email; }
            set
            {
                if (!bought.Email.Equals(value))
                {
                    bought.Email = value;
                    OnPropertyChanged("Email");
                }
            }
        }

        public string Amount
        {
            get { return bought?.Amount; }
            set
            {
                if (!bought.Amount.Equals(value))
                {
                    bought.Amount = value;
                    OnPropertyChanged("Amount");
                }
            }
        }

        public string Total
        {
            get { return bought?.Total; }
            set
            {
                if (!bought.Total.Equals(value))
                {
                    bought.Total = value;
                    OnPropertyChanged("Total");
                }
            }
        }

        public string BoughtDato
        {
            get { return bought?.BoughtDato; }
            set
            {
                if (!bought.BoughtDato.Equals(value))
                {
                    bought.BoughtDato = value;
                    OnPropertyChanged("BoughtDato");
                }
            }
        }

        string IDataErrorInfo.Error
        {
            get { return (bought as IDataErrorInfo).Error; }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                string error = null;
                try
                {
                    error = (bought as IDataErrorInfo)[propertyName];
                }
                catch
                {
                }
                CommandManager.InvalidateRequerySuggested();
                return error;
            }
        }
    }
}
