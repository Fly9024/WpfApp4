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

        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            auth SelectedUser = (auth)dgUsers.SelectedItem;//сохраняем выбранную строку datagrid в отдельный объект
            BaseConnect.BaseModel.auth.Remove(SelectedUser);//удаляем эту строку из модели
            BaseConnect.BaseModel.SaveChanges();//синхронизируем изменения с сервером
            MessageBox.Show("Выбранный пользователь удален");//обратная связь с оператором программы
            dgUsers.ItemsSource = BaseConnect.BaseModel.auth.ToList();//обновить строки в datagrid
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            LoadPages.MainFrame.GoBack();
        }

        private void btnEditUser_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
