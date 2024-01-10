using Madspild.DataAccess;
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
    /// Interaction logic for Order.xaml
    /// </summary>
    public partial class Order : Window
    {
        private AdminViewModel admin = new AdminViewModel();
        private BasketViewModel basket = new BasketViewModel();
        public Order()
        {
            InitializeComponent();
            admin.WarningHandler += delegate (object sender, MessageEventArgs e) {
                MessageBox.Show(e.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            };
            admin.CloseHandler += delegate (object sender, EventArgs e) { Close(); };
            DataContext = admin;
        }

        private void Grid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grid.SelectedItem != null)
            {
                // Assuming the selected item is of type Goods
                Goods selectedGoods = (Goods)grid.SelectedItem;

                // Open a new window for selecting quantity
                QuantitySelectionWindow quantityWindow = new QuantitySelectionWindow(selectedGoods);
                bool? result = quantityWindow.ShowDialog();

                // Check if the user confirmed the selection
                if (result == true)
                {
                    // Get the selected quantity from the QuantitySelectionWindow
                    int selectedQuantity = quantityWindow.SelectedQuantity;

                    // Add the selected item with quantity to the new DataGrid
                    basket.Add(selectedGoods, selectedQuantity);
                }
            }
        }
    }
}
