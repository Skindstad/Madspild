using Madspild.DataAccess;
using Madspild.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Madspild.ViewModel
{
    public class UserViewModel : ViewModelBase, IDataErrorInfo
    {
        public RelayCommand OkCommand { get; private set; }
        public RelayCommand ModCommand { get; private set; }
        public RelayCommand RemCommand { get; private set; }
        protected User model;
        protected UserRepository repository;

        public UserViewModel(User model, UserRepository repository) 
        {
            this.model = model;
            this.repository = repository;
            OkCommand = new RelayCommand(p => Add(), p => CanUpdate());
            ModCommand = new RelayCommand(p => Update(), p => CanUpdate());
            RemCommand = new RelayCommand(p => Remove());
        }


        public User Model
        {
            get { return model; }
        }

        public string Name
        {
            get { return model.Name; }
            set
            {
                if (!model.Name.Equals(value))
                {
                    model.Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public string Email
        {
            get { return model.Email; }
            set
            {
                if (!model.Email.Equals(value))
                {
                    model.Email = value;
                    OnPropertyChanged("Email");
                }
            }
        }

        public string Password
        {
            get { return model.Password; }
            set
            {
                if (!model.Password.Equals(value))
                {
                    model.Password = value;
                    OnPropertyChanged("Password");
                }
            }
        }

        public string HPhone
        {
            get { return model.HomePhone; }
            set
            {
                if (!model.HomePhone.Equals(value))
                {
                    model.HomePhone = value;
                    OnPropertyChanged("HPhone");
                }
            }
        }

        public string WPhone
        {
            get { return model.WorkPhone; }
            set
            {
                if (!model.WorkPhone.Equals(value))
                {
                    model.WorkPhone = value;
                    OnPropertyChanged("WPhone");
                }
            }
        }

        public string Address
        {
            get { return model.Address; }
            set
            {
                if (!model.Address.Equals(value))
                {
                    model.Address = value;
                    OnPropertyChanged("Address");
                }
            }
        }

        public string Zipcode
        {
            get { return model.Zipcode; }
            set
            {
                if (!model.Zipcode.Equals(value))
                {
                    model.Zipcode = value;
                    OnPropertyChanged("Zipcode");
                }
            }
        }

        public string Access
        {
            get { return model.Access; }
            set
            {
                if (!model.Access.Equals(value))
                {
                    model.Access = value;
                    OnPropertyChanged("Access");
                }
            }
        }

        public void Clear()
        {
            model = new User();
            OnPropertyChanged("Name");
            OnPropertyChanged("Email");
            OnPropertyChanged("HPhone");
            OnPropertyChanged("WPhone");
            OnPropertyChanged("Address");
            OnPropertyChanged("Zipcode");
            OnPropertyChanged("Password");
            OnPropertyChanged("Access");
        }

        public void Update()
        {
            if (IsValid)
            {
                try
                {
                    repository.Update(model);
                    OnClose();
                }
                catch (Exception ex)
                {
                    OnWarning(ex.Message);
                }
            }
        }

        public void Remove()
        {
            try
            {
                repository.Remove(model.Email);
                OnClose();
            }
            catch (Exception ex)
            {
                OnWarning(ex.Message);
            }
        }

        public void Add()
        {
            if (IsValid)
            {
                try
                {
                    repository.Add(model);
                    Clear();
                }
                catch (Exception ex)
                {
                    OnWarning(ex.Message);
                }
            }
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

        private bool IsValid
        {
            get { return model.IsValid; }
        }

        private bool CanUpdate()
        {
            return IsValid;
        }
    }
}

