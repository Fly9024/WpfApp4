using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
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
        List<users> lu1;
        PageChange pc = new PageChange();
        public PageUsersLisl()
        {
            InitializeComponent();
            users = BaseConnect.BaseModel.users.ToList();
            lbUsersList.ItemsSource = users;
            //заполнение списка с фильтром по полу
            lbGenderFilter.ItemsSource = BaseConnect.BaseModel.genders.ToList();
            lbGenderFilter.SelectedValuePath = "id";
            lbGenderFilter.DisplayMemberPath = "gender";
            lu1 = users;
            DataContext = pc;//поместил объект в ресурсы страницы
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

            lbUsersList.ItemsSource = lu1;// возвращаем результат в виде списка, к которому применялись активные фильтры
            pc.Countlist = lu1.Count;//меняем количество элементов в списке для постраничной навигации
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            lbUsersList.ItemsSource = users;//в качестве источника данных исходный список
            lu1 = users;
            lbGenderFilter.SelectedIndex = -1; //сбрасываем выбранный элемент списка
            txtNameFilter.Text = "";//сбрасываем фильтр на строку
            txtOT.Text = "";
            txtDO.Text = "";
        }
        
        private void GoPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;//определяем, какой текстовый блок был нажат           
            //изменение номера страници при нажатии на кнопку
            switch (tb.Uid)
            {
                case "prev":
                    pc.CurrentPage--;
                    break;
                case "next":
                    pc.CurrentPage++;
                    break;
                default:
                    pc.CurrentPage = Convert.ToInt32(tb.Text);
                    break;
            }


            //определение списка
            lbUsersList.ItemsSource  = lu1.Skip(pc.CurrentPage * pc.CountPage - pc.CountPage).Take(pc.CountPage).ToList();
            
            txtCurrentPage.Text = "Текущая страница: " + (pc.CurrentPage).ToString();
            
            
        }

        private void txtPageCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                pc.CountPage = Convert.ToInt32(txtPageCount.Text);
            }
            catch
            {
                pc.CountPage = lu1.Count;
            }
            pc.Countlist = users.Count;
            lbUsersList.ItemsSource = lu1.Skip(0).Take(pc.CountPage).ToList();
        }

        private void UserImage_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Image IMG = sender as System.Windows.Controls.Image;
            int ind = Convert.ToInt32(IMG.Uid);
            users U = BaseConnect.BaseModel.users.FirstOrDefault(x => x.id == ind);//запись о текущем пользователе
            usersimage UI = BaseConnect.BaseModel.usersimage.FirstOrDefault(x => x.id_user == ind && x.avatar==true);//получаем запись о картинке для текущего пользователя
            BitmapImage BI=new BitmapImage();
            if (UI != null)//если для текущего пользователя существует запись о его катринке
            { 
                if (UI.path!=null)//если присутствует путь к картинке
                {
                    BI = new BitmapImage(new Uri(UI.path, UriKind.Relative));
                }
                else//если присутствуют двоичные данные
                {
                    BI.BeginInit();//начать инициализацию BitmapImage (для помещения данных из какого-либо потока)
                    BI.StreamSource = new MemoryStream(UI.image);//помещаем в источник данных двоичные данные из потока
                    BI.EndInit();//закончить инициализацию
                }
            }
            else//если в базе не содержится картинки, то ставим заглушку
            {
                switch (U.gender)//в зависимости от пола пользователя устанавливаем ту или иную картинку
                {
                    case 1:
                        BI = new BitmapImage(new Uri(@"/images/male.jpg", UriKind.Relative));
                        break;
                    case 2:
                        BI = new BitmapImage(new Uri(@"/images/female.jpg", UriKind.Relative));
                        break;
                    default:
                        BI = new BitmapImage(new Uri(@"/images/other.jpg", UriKind.Relative));
                        break;
                }
            }              
            IMG.Source = BI;//помещаем картинку в image
        }

        private void BtmAddImage_Click(object sender, RoutedEventArgs e)
        {
            Button BTN = (Button)sender;
            int ind = Convert.ToInt32(BTN.Uid);
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".jpg"; // задаем расширение по умолчанию
            openFileDialog.Filter = "Изображения |*.jpg;*.png"; // задаем фильтр на форматы файлов
            var result = openFileDialog.ShowDialog();
            if (result == true)//если файл выбран
            {
                System.Drawing.Image UserImage = System.Drawing.Image.FromFile(openFileDialog.FileName);//создаем изображение
                ImageConverter IC = new ImageConverter();//конвертер изображения в массив байт
                byte[] ByteArr = (byte[])IC.ConvertTo(UserImage, typeof(byte[]));//непосредственно конвертация
                usersimage UI = new usersimage() { id_user = ind, image = ByteArr, avatar=false };//создаем новый объект usersimage
                BaseConnect.BaseModel.usersimage.Add(UI);//добавляем его в модель
                BaseConnect.BaseModel.SaveChanges();//синхронизируем с базой
                MessageBox.Show("картинка пользователя добавлена в базу");
            }
            else
            {
                MessageBox.Show("операция выбора изображения отменена");
            }
        }
        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            RadioButton RB = (RadioButton)sender;
            switch (RB.Uid)
            {
                case "name":
                    lu1 = lu1.OrderBy(x=> x.name).ToList();
                    break;
                case "DR":
                    lu1 = lu1.OrderBy(x => x.dr).ToList();
                    break;
            }
            if (RBReverse.IsChecked == true) lu1.Reverse();
            lbUsersList.ItemsSource = lu1;
            txtPageCount_TextChanged(null, null);//вызываем событие с текстбокса изменения количества записей на странице (для отрисовки)
        }

        private void UserImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Image im = (System.Windows.Controls.Image)sender;
            int index = Convert.ToInt32(im.Uid);
            Gallery G = new Gallery(index);
            G.Show();
        }
    }
}
