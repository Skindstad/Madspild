using Madspild.ViewModel;
using System.Windows;
using System;
namespace Madspild.View
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        private AdminViewModel admin = new();
        public Admin()
        {
            InitializeComponent();
            admin.WarningHandler += delegate (object sender, MessageEventArgs e) {
                MessageBox.Show(e.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            };
            admin.CloseHandler += delegate (object sender, EventArgs e) { Close(); };
            DataContext = admin;
        }
    }
}
