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
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : Window
    {
        private AdminViewModel admin = new AdminViewModel();
        public UpdateWindow(Goods goods)
        {
            InitializeComponent();

            admin = new AdminViewModel();
            // Set the DataContext to the specific Goods instance
            admin.SelectedModel = goods;

            admin.WarningHandler += delegate (object sender, MessageEventArgs e) {
                MessageBox.Show(e.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            };
            admin.CloseHandler += delegate (object sender, EventArgs e) { Close(); };
            DataContext = admin;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the window
            Close();
        }
    }
}
