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
        public RelayCommand SearchCommand { get; private set; }

        private Basket basket = new Basket();
        private BasketRepository repository = new BasketRepository();
        private ObservableCollection<Basket> boughtItems;
        public ForsideViewModel()
        {
            repository.RepositoryChanged += ModelChanged;
            Search();
            SearchCommand = new RelayCommand(p => Search());
        }

        public ObservableCollection<Basket> BoughtItems
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

        public void ModelChanged(object sender, DbEventArgs e)
        {
            if (e.Operation != DbOperation.SELECT)
            {
                Clear();
            }
            BoughtItems = new ObservableCollection<Basket>(repository);
        }

        public string PersonEmail
        {
            get { return basket?.PersonEmail; }
            set
            {
                if (!basket.PersonEmail.Equals(value))
                {
                    basket.PersonEmail = value;
                    OnPropertyChanged("PersonEmail");
                }
            }
        }

        public string ProductName
        {
            get { return basket?.ProductName; }
            set
            {
                if (!basket.ProductName.Equals(value))
                {
                    basket.ProductName = value;
                    OnPropertyChanged("ProductName");
                }
            }
        }

        public string Amount
        {
            get { return basket?.Amount; }
            set
            {
                if (!basket.Amount.Equals(value))
                {
                    basket.Amount = value;
                    OnPropertyChanged("Amount");
                }
            }
        }

        public string Price
        {
            get { return basket?.Price; }
            set
            {
                if (!basket.Price.Equals(value))
                {
                    basket.Price = value;
                    OnPropertyChanged("Price");
                }
            }
        }

        public string BacketDato
        {
            get { return basket?.BacketDato; }
            set
            {
                if (!basket.BacketDato.Equals(value))
                {
                    basket.BacketDato = value;
                    OnPropertyChanged("BacketDato");
                }
            }
        }

        public string BoughtDato
        {
            get { return basket?.BoughtDato; }
            set
            {
                if (!basket.BoughtDato.Equals(value))
                {
                    basket.BoughtDato = value;
                    OnPropertyChanged("BoughtDato");
                }
            }
        }

        public Basket SelectedModel
        {
            get { return basket; }
            set
            {
                basket = value;
                OnPropertyChanged("Id");
                OnPropertyChanged("PersonEmail");
                OnPropertyChanged("ProductName");
                OnPropertyChanged("Amount");
                OnPropertyChanged("Price");
                OnPropertyChanged("BacketDato");
                OnPropertyChanged("BoughtDato");
                OnPropertyChanged("SelectedModel");
            }
        }

        private void Clear()
        {
            SelectedModel = null;
            basket = new Basket();
            OnPropertyChanged("Id");
            OnPropertyChanged("PersonEmail");
            OnPropertyChanged("ProductName");
            OnPropertyChanged("Amount");
            OnPropertyChanged("Price");
            OnPropertyChanged("BacketDato");
            OnPropertyChanged("BoughtDato");
            OnPropertyChanged("SelectedModel");
            Search();
        }

        public void Search()
        {
            repository.Bought(PersonEmail);
        }

        string IDataErrorInfo.Error
        {
            get { return (basket as IDataErrorInfo).Error; }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                string error = null;
                try
                {
                    error = (basket as IDataErrorInfo)[propertyName];
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
