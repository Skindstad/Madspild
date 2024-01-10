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
    public class UsersViewModel : ViewModelBase, IDataErrorInfo
    {

        public RelayCommand SearchCommand { get; private set; }
        public RelayCommand CreateCommand { get; private set; }

        protected User model;
        public static UserRepository repository = new();
        public ObservableCollection<User> users;

        private string id = "";
        private string name = "";
        private string email = "";
        private string homePhone = "";
        private string workPhone = "";
        private string address = "";
        private string zipcode = "";
        private string city = "";

        public UsersViewModel() 
        {
            Search();
            SearchCommand = new RelayCommand(p => Search());
            CreateCommand = new RelayCommand(p => (new CreateUserView()).ShowDialog());
            users = new ObservableCollection<User>(repository);
            repository.RepositoryChanged += Refresh;
        }


        private void Refresh(object sender, DbEventArgs e)
        {
            Users = new ObservableCollection<User>(repository);
        }

        public ObservableCollection<User> Users
        {
            get { return users; }
            set
            {
                if (users != value)
                {
                    users = value;
                    OnPropertyChanged("Users");
                }
            }
        }

        public string Id
        {
            get { return id; }
            set
            {
                if (!id.Equals(value))
                {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (!name.Equals(value))
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                if (!email.Equals(value))
                {
                    email = value;
                    OnPropertyChanged("Email");
                }
            }
        }

        public string HomePhone
        {
            get { return homePhone; }
            set
            {
                if (!homePhone.Equals(value))
                {
                    homePhone = value;
                    OnPropertyChanged("HomePhone");
                }
            }
        }

        public string WorkPhone
        {
            get { return workPhone; }
            set
            {
                if (!workPhone.Equals(value))
                {
                    workPhone = value;
                    OnPropertyChanged("WorkPhone");
                }
            }
        }

        public string Address
        {
            get { return address; }
            set
            {
                if (!address.Equals(value))
                {
                    address = value;
                    OnPropertyChanged("Address");
                }
            }
        }

        public string Zipcode
        {
            get { return zipcode; }
            set
            {
                if (!zipcode.Equals(value))
                {
                    zipcode = value;
                    OnPropertyChanged("Zipcode");
                }
            }
        }

        public string City
        {
            get { return city; }
            set
            {
                if (!city.Equals(value))
                {
                    city = value;
                    OnPropertyChanged("City");
                }
            }
        }

        private void Search()
        {
            try
            {
                repository.Search(name, email, homePhone, workPhone, address, zipcode);
                Users = new ObservableCollection<User>(repository);
            }
            catch (Exception ex)
            {
                OnWarning(ex.Message);
            }
        }

        public void UpdateUser(User user)
        {
            UpdateUserView dlg = new (user);
            dlg.ShowDialog();
        }


        string IDataErrorInfo.Error
        {
            get { return (model as IDataErrorInfo).Error; }
        }

        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                string error = null;
                try
                {
                    error = (model as IDataErrorInfo)[propertyName];
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
