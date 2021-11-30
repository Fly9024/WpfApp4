using System;
using System.Collections.Generic;
using System.IO;
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

namespace WpfApp4.pages
{
    

    /// <summary>
    /// Логика взаимодействия для Gallery.xaml
    /// </summary>
    public partial class Gallery : Window
    {
        List<usersimage> LUI;
        BitmapImage BI;
        int CountUsersImages;//общее количество картинок
        int CurrentUserImage = 0;//текущая картинка
        public Gallery(int id)
        {
            InitializeComponent();
            Title = "Галерея пользователя: " + BaseConnect.BaseModel.users.FirstOrDefault(x=>x.id==id).name;//заголовок окна страницы
            LUI = BaseConnect.BaseModel.usersimage.Where(x=> x.id_user==id).ToList();//сам список картинок
            CountUsersImages = LUI.Count;//количество картинок в списке

            //вставляем картинку в image. заглушку или первую по счету
            BI = new BitmapImage();
            if (CountUsersImages > 0)
            {
                BI.BeginInit();//начало инициации изображения
                BI.StreamSource = new MemoryStream(LUI[0].image);//создание изображения из массива байт 
                BI.EndInit();//конец инцицации изображения
            }
            else
            {
                BI = new BitmapImage(new Uri(@"/images/EmptyImage.jpg", UriKind.Relative));
            }

            ImgUserPic.Source = BI;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (CountUsersImages > 0)
            {
                Button button = (Button)sender;
                switch (button.Uid)//увеличиваем или уменьшаем индекс в списке в зависимости от нажатой кнопки
                {
                    case "Left":
                        CurrentUserImage--;
                        if (CurrentUserImage < 0) CurrentUserImage = CountUsersImages - 1;//карусель
                        break;
                    case "Right":
                        CurrentUserImage++;
                        if (CurrentUserImage >= CountUsersImages) CurrentUserImage = 0;//карусель
                        break;
                    case "Avatar":
                        for (int i = 0; i < CountUsersImages; i++)
                        {
                            LUI[i].avatar = i == CurrentUserImage;
                        }
                        BaseConnect.BaseModel.SaveChanges();
                        MessageBox.Show("Аватар успешно изменен");
                        break;
                    default:
                        CurrentUserImage = 0;
                        break;
                }
                //вставляем картинку в image
                BI = new BitmapImage();
                BI.BeginInit();//начало инициации изображения
                BI.StreamSource = new MemoryStream(LUI[CurrentUserImage].image);//создание изображения из массива байт                    
                BI.EndInit();//конец инцицации изображения
                ImgUserPic.Source = BI;
            }
        }
    }
}
