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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp4.pages
{
    /// <summary>
    /// Логика взаимодействия для adminMenu.xaml
    /// </summary>
    public partial class adminMenu : Page
    {
        public adminMenu()
        {
            InitializeComponent();
            dgUsers.ItemsSource = BaseConnect.BaseModel.auth.ToList();
        }

        private void btnSaveCahanges_Click(object sender, RoutedEventArgs e)
        {
           BaseConnect.BaseModel.SaveChanges();
        }
    }
}
