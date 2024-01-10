using Madspild.DataAccess;
using Madspild.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Madspild.View;
using System.Xml.Linq;
using System.Windows.Input;

namespace Madspild.ViewModel
{
    public class BasketViewModel : ViewModelBase, IDataErrorInfo
    {
        public RelayCommand AddCommand { get; private set; }

        private Basket basket = new Basket();
        private BasketRepository repository = new BasketRepository();
        private ObservableCollection<Basket> orderItems;
        public BasketViewModel() 
        {
            repository.RepositoryChanged += ModelChanged;
            Search();
            //AddCommand = new RelayCommand(p => Add(), p => CanAdd());
        }

        public ObservableCollection<Basket> OrderItems
        {
            get { return orderItems; }
            set
            {
                if (orderItems != value)
                {
                    orderItems = value;
                    OnPropertyChanged("OrderItems");
                }
            }
        }

        public void ModelChanged(object sender, DbEventArgs e)
        {
            if (e.Operation != DbOperation.SELECT)
            {
                Clear();
            }
            OrderItems = new ObservableCollection<Basket>(repository);
        }

        public string Id
        {
            get { return basket?.Id; }
            set
            {
                if (!basket.Id.Equals(value))
                {
                    basket.Id = value;
                    OnPropertyChanged("Id");
                }
            }
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
            repository.Search(PersonEmail, ProductName, Amount);
        }

        public void Update()
        {
            try
            {
                repository.Update(PersonEmail, ProductName, Amount, BacketDato);
            }
            catch (Exception ex)
            {
                OnWarning(ex.Message);
            }
        }

        private bool CanUpdate()
        {
            return basket.IsValid;
        }

        public void Add(Goods goods, int quantity)
        {
            try
            {
                repository.Add(PersonEmail, goods.Name, quantity.ToString());
            }
            catch (Exception ex)
            {
                OnWarning(ex.Message);
            }
        }

        public bool CanAdd()
        {
            return basket.IsValid;
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
