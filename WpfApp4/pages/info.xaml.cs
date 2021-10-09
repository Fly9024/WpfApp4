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
    /// Логика взаимодействия для info.xaml
    /// </summary>
    public partial class info : Page
    {
        public info()
        {
            InitializeComponent();          
        }   
         public info(auth CurrentUser)
        {
            InitializeComponent();
           try
            {
                tbName.Text = CurrentUser.users.name;
                tbDR.Text = CurrentUser.users.dr.ToString("yyyy MMMM dd");
                tbGender.Text = CurrentUser.users.genders.gender;
                //список из качеств личности авторизованного пользователя
                List<users_to_traits> LUTT = BaseConnect.BaseModel.users_to_traits.Where(x => x.id_user == CurrentUser.id).ToList();
                foreach (users_to_traits UT in LUTT)
                {
                    tbTraits.Text += UT.traits.trait + "; ";
                }
            }
            catch
            {
                MessageBox.Show("информации о вас нет в системе");
            }           
        }   


        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            LoadPages.MainFrame.GoBack();
        }
    }
}
