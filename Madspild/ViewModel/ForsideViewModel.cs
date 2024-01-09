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

        CurrentUser model;
        private UserRepository repository = new();

        public ForsideViewModel()
        {
            Name = model!.Name;
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
