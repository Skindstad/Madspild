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
    public class CombinedViewModel
    {
        public AdminViewModel Admin { get; set; }
        public BasketViewModel Basket { get; set; }

        public CombinedViewModel()
        {
            Admin = new AdminViewModel();
            Basket = new BasketViewModel();
        }
    }
    /// <summary>
    /// Interaction logic for Order.xaml
    /// </summary>
    public partial class Order : Window
    {
        private CombinedViewModel combinedViewModel = new CombinedViewModel();
        public Order()
        {
            InitializeComponent();
            combinedViewModel.Admin.WarningHandler += delegate (object sender, MessageEventArgs e) {
                MessageBox.Show(e.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            };
            combinedViewModel.Admin.CloseHandler += delegate (object sender, EventArgs e) { Close(); };
            DataContext = combinedViewModel;
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

                    //string selectedEmail = quantityWindow.SelectedEmail;

                    // Sends selected item with quantity to Add() method in BasketViewModel
                    combinedViewModel.Basket.Add(selectedGoods, selectedQuantity);
                }
            }
        }
    }
}
