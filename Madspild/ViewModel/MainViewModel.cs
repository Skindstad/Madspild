using Madspild.DataAccess;
using Madspild.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Madspild.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand LoginCommand { get; set; }

        public static UserRepository repository = new ();
        List<string> user = [];

        private string email = "";
        private string password = "";
        private string access = "";

        public MainViewModel() 
        {
            Clear();
            LoginCommand = new RelayCommand(p => Login(), p => CanLogin());
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
        public string Password
        {
            get { return password; }
            set
            {
                if (!password.Equals(value))
                {
                    password = value;
                    OnPropertyChanged("Password");
                }
            }
        }
        public string Access
        {
            get { return access; }
            set
            {
                if (!access.Equals(value))
                {
                    access = value;
                    OnPropertyChanged("Access");
                }
            }
        }

        private void Clear()
        {
            email = "";
            password = "";
            access = "";
        }

        private void Login()
        {
            
            try
            {

                user = repository.Login(email, password);
                
                //BindData = new ObservableCollection<Data>(repository);
            }
            catch (Exception ex)
            {
                OnWarning(ex.Message);
            }
            AccessRight(user);
        }

        private void AccessRight(List<string> user)
        {
            if (user[3] == "Admin")
            {
                Forside dlg = new Forside();
                dlg.ShowDialog();
            }
            else
            {
                
            }
        }

        private bool CanLogin()
        {
            return email.Length > 0 || Password.Length > 0;
        }

    }
}
