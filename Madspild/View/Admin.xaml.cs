using Madspild.ViewModel;
using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Input;
using Madspild.Model;
namespace Madspild.View
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        private AdminViewModel admin = new AdminViewModel();
        public Admin()
        {
            InitializeComponent();
            admin.WarningHandler += delegate (object sender, MessageEventArgs e) {
                MessageBox.Show(e.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            };
            admin.CloseHandler += delegate (object sender, EventArgs e) { Close(); };
            DataContext = admin;
        }

        private void grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                try
                {
                    DataGridRow row = sender as DataGridRow;
                    Goods goods = (Goods)row.Item;
                    admin.UpdateGoods(goods);
                }
                catch
                {
                }
            }
        }
    }
}
