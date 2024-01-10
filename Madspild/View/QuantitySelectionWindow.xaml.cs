using Madspild.Model;
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
    /// Interaction logic for QuantitySelectionWindow.xaml
    /// </summary>
    public partial class QuantitySelectionWindow : Window
    {
        public int SelectedQuantity { get; private set; }
        private Goods selectedGoods;
        public QuantitySelectionWindow(Goods goods)
        {
            InitializeComponent();
            selectedGoods = goods;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate and parse the user input
            if (int.TryParse(txtQuantity.Text, out int quantity))
            {
                // Set the selected quantity and close the window with DialogResult
                SelectedQuantity = quantity;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Invalid quantity. Please enter a valid number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
