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
    /// Interaction logic for Forside.xaml
    /// </summary>
    public partial class Forside : Window
    {
        private ForsideViewModel forside = new ForsideViewModel();
        public Forside()
        {
            InitializeComponent();
            forside.WarningHandler += delegate (object sender, MessageEventArgs e) {
                MessageBox.Show(e.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            };
            forside.CloseHandler += delegate (object sender, EventArgs e) { Close(); };
            DataContext = forside;
        }
    }
}
