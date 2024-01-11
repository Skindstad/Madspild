using Madspild.ViewModel;
using System.Windows;

namespace Madspild
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// // Lavet af Jakob
    public partial class MainWindow : Window
    {
        private MainViewModel model = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = model;
            model.WarningHandler += delegate (object sender, MessageEventArgs e) {
                MessageBox.Show(e.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            };

        }
    }
}