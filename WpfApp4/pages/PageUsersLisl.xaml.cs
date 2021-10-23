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
    /// Логика взаимодействия для PageUsersLisl.xaml
    /// </summary>
    public partial class PageUsersLisl : Page
    {
        List<users> users;
        public PageUsersLisl()
        {
            InitializeComponent();
            users = BaseConnect.BaseModel.users.ToList();
            lbUsersList.ItemsSource = users;
            //заполнение списка с фильтром по полу
            lbGenderFilter.ItemsSource = BaseConnect.BaseModel.genders.ToList();
            lbGenderFilter.SelectedValuePath = "id";
            lbGenderFilter.DisplayMemberPath = "gender";
        }

        private void lbTraits_Loaded(object sender, RoutedEventArgs e)
        {
            //senser содержит объект, который вызвал данное событие, но только у него объектный тип, надо преобразовать
            ListBox lb = (ListBox)sender;//lb содержит ссылку на тот список, который вызвал это событие
            int index = Convert.ToInt32(lb.Uid);//получаем ID элемента списка. в данном случае оно совпадает с id user
            lb.ItemsSource = BaseConnect.BaseModel.users_to_traits.Where(x=>x.id_user == index).ToList();
            lb.DisplayMemberPath = "traits.trait";//показываем пользователю текстовое описание качества
        }

        private void Filter(object sender, RoutedEventArgs e)
        {
            List<users> lu1=users;
            //фильтр по количеству            
            try
            {
                int OT = Convert.ToInt32(txtOT.Text) - 1;//т.к. индексы начинаются с нуля
                int DO = Convert.ToInt32(txtDO.Text);
                //skip - пропустить определенное количество записей
                //take - выбрать определенное количество записей
                lu1 = users.Skip(OT).Take(DO - OT).ToList();
            }
            catch
            {
                //ничего не надо делать, если этот фильтр не применен
            }
            //фильтр по полу
            if(lbGenderFilter.SelectedValue!=null)//если пункт из списка не выбран, то сам фильтр работать не будет
            {
                lu1 = lu1.Where(x => x.gender == (int)lbGenderFilter.SelectedValue).ToList();
            }

            //фильтр по имени
            if(txtNameFilter.Text!="")
            {
                lu1 = lu1.Where(x => x.name.Contains(txtNameFilter.Text)).ToList();
            }

            lbUsersList.ItemsSource = lu1;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            lbUsersList.ItemsSource = users;//в качестве источника данных исходный список
            lbGenderFilter.SelectedIndex = -1; //сбрасываем выбранный элемент списка
            txtNameFilter.Text = "";//сбрасываем фильтр на строку
            txtOT.Text = "";
            txtDO.Text = "";
        }
    }
}
