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

namespace Bookstore.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        public AuthorizationPage()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = TBoxLogin.Text.Trim();
            string password = TBoxPassword.Password.Trim();

            var currentUser = App.Context.Users.FirstOrDefault(p => p.Login == login && p.Password == password);

            if (currentUser != null)
            {
                App.CurrentUser = currentUser;
                MessageBox.Show("Вы успешно вошли в систему!", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new BooksPage());
            }
            else
            {
                MessageBox.Show("Пользователь с такими данными не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnRegistr_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage());
        }

        private void BtnGuest_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentUser = null;
            NavigationService.Navigate(new BooksPage());
        }
    }
}
