using Madspild.DataAccess;
using Madspild.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Madspild.View;
using System.ComponentModel;
using System.Windows.Input;

namespace Madspild.ViewModel
{
    public class AdminViewModel : ViewModelBase, IDataErrorInfo
    {
        public RelayCommand SearchCommand { get; private set; }
        public RelayCommand RemoveCommand { get; private set; }
        public RelayCommand AddCommand { get; private set; }
        public RelayCommand ClearCommand { get; private set; }
        public RelayCommand UpdateCommand { get; private set; }
        public RelayCommand CreateCommand { get; private set; }
        public RelayCommand UsersCommand { get; private set; }

        private Goods admin = new Goods();
        private GoodsRepository repository = new GoodsRepository();
        private ObservableCollection<Goods> inventory;
        public ObservableCollection<Basket> OrderItems { get; set; } = new ObservableCollection<Basket>();

        public AdminViewModel()
        {
            repository.RepositoryChanged += ModelChanged;
            Search();
            UpdateCommand = new RelayCommand(p => Update(), p => CanUpdate());
            SearchCommand = new RelayCommand(p => Search());
            ClearCommand = new RelayCommand(p => Clear());
            AddCommand = new RelayCommand(p => Add(), p => CanAdd());
            RemoveCommand = new RelayCommand(p => Remove(), p => CanRemove());
            CreateCommand = new RelayCommand(p => (new CreateWindow()).ShowDialog());
            UsersCommand = new RelayCommand(p => (new UsersView()).ShowDialog());
        }

        public ObservableCollection<Goods> Inventory
        {
            get { return inventory; }
            set
            {
                if (inventory != value)
                {
                    inventory = value;
                    OnPropertyChanged("Inventory");
                }
            }
        }

        public void ModelChanged(object sender, DbEventArgs e)
        {
            if (e.Operation != DbOperation.SELECT)
            {
                Clear();
            }
            Inventory = new ObservableCollection<Goods>(repository);
        }

        public string Id
        {
            get { return admin?.Id; }
            set
            {
                if (!admin.Id.Equals(value))
                {
                    admin.Id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        public string Name
        {
            get { return admin?.Name; }
            set
            {
                if (!admin.Name.Equals(value))
                {
                    admin.Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string Price
        {
            get { return admin?.Price; }
            set
            {
                if (!admin.Price.Equals(value))
                {
                    admin.Price = value;
                    OnPropertyChanged("Price");
                }
            }
        }

        public string Amount
        {
            get { return admin?.Amount; }
            set
            {
                if (!admin.Amount.Equals(value))
                {
                    admin.Amount = value;
                    OnPropertyChanged("Amount");
                }
            }
        }

        public string AmountLimit
        {
            get { return admin?.AmountLimit; }
            set
            {
                if (!admin.AmountLimit.Equals(value))
                {
                    admin.AmountLimit = value;
                    OnPropertyChanged("AmountLimit");
                }
            }
        }

       /* public string Category
        {
            get { return admin?.Category; }
            set
            {
                if (!admin.Category.Equals(value))
                {
                    admin.Category = value;
                    OnPropertyChanged("Category");
                }
            }
        }*/

        public string Path
        {
            get { return admin?.Path; }
            set
            {
                if (!admin.Path.Equals(value))
                {
                    admin.Path = value;
                    OnPropertyChanged("Path");
                }
            }
        }

        public Goods SelectedModel
        {
            get { return admin; }
            set
            {
                admin = value;
                OnPropertyChanged("Id");
                OnPropertyChanged("Name");
                OnPropertyChanged("Price");
                OnPropertyChanged("Amount");
                OnPropertyChanged("AmountLimit");
                OnPropertyChanged("Category");
                OnPropertyChanged("SelectedModel");
            }
        }

        private void Clear()
        {
            SelectedModel = null;
            admin = new Goods();
            OnPropertyChanged("Id");
            OnPropertyChanged("Name");
            OnPropertyChanged("Price");
            OnPropertyChanged("Amount");
            OnPropertyChanged("AmountLimit");
            OnPropertyChanged("Category");
            OnPropertyChanged("SelectedModel");
            Search();
        }

        public void Search()
        {
            repository.Search(Name, Price);
        }

        public void Update()
        {
            try
            {
                repository.Update(Name, Price, Amount, AmountLimit, Path);
            }
            catch (Exception ex)
            {
                OnWarning(ex.Message);
            }
        }

        private bool CanUpdate()
        {
            return admin.IsValid;
        }

        public void Add()
        {
            try
            {
                repository.Add(Name, Price, Amount, AmountLimit, Path);
                Clear();
            }
            catch (Exception ex)
            {
                OnWarning(ex.Message);
            }
        }

        public bool CanAdd()
        {
            return admin.IsValid;
        }

        public void Remove()
        {
            try
            {
                repository.Remove(Name);
            }
            catch (Exception ex)
            {
                OnWarning(ex.Message);
            }
        }

        private bool CanRemove()
        {
            return Id != null && Id.Length > 0;
        }

        public void UpdateGoods(Goods goods)
        {
            UpdateWindow dlg = new UpdateWindow(goods);
            dlg.ShowDialog();
        }

        public void AddToOrder(Goods goods, int quantity)
        {
            try
            {
                // Create a new Basket item for the order
                Basket orderItem = new Basket
                {
                    Id = admin.Id,
                    PersonEmail = "", // Assign the user's email
                    ProductName = goods.Name,
                    Amount = quantity.ToString(), // Set the ordered quantity
                    Price = goods.Price,
                    BacketDato = "",
                    BoughtDato = ""
                };

                // Use BasketRepository to add the order item
                BasketRepository basketRepository = new BasketRepository();
                basketRepository.Add(orderItem);

                // Notify property changed for any relevant properties
                OnPropertyChanged(nameof(OrderItems));
            }
            catch (DbException ex)
            {
                // Handle exception (show a warning, log, etc.)
                OnWarning("Failed to add item to order: " + ex.Message);
            }
        }

        string IDataErrorInfo.Error
        {
            get { return (admin as IDataErrorInfo).Error; }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                string error = null;
                try
                {
                    error = (admin as IDataErrorInfo)[propertyName];
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
