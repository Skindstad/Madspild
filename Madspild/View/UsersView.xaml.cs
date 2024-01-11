using Madspild.Model;
using Madspild.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Madspild.View
{
    /// <summary>
    /// Interaction logic for UsersView.xaml
    /// </summary>
    /// // Lavet af Jakob
    public partial class UsersView : Window
    {
        UsersViewModel model = new();
        public UsersView()
        {
            InitializeComponent();
            DataContext = model;
            model.WarningHandler += delegate (object sender, MessageEventArgs e) {
                MessageBox.Show(e.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            };
        }
        private void grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                try
                {
                    DataGridRow row = sender as DataGridRow;
                    User user = (User)row.Item;
                    model.UpdateUser(user);
                }
                catch
                {
                }
            }
        }
    }
}
