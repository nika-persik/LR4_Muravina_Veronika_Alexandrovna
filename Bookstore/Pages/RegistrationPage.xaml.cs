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
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        private string currentCaptcha;
        private Random random = new Random();

        public RegistrationPage()
        {
            InitializeComponent();
            GenerateNewCaptcha();
        }

        private void GenerateNewCaptcha()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            currentCaptcha = new string(Enumerable.Repeat(chars, 5).Select(s => s[random.Next(s.Length)]).ToArray());
            CaptchaTextBlock.Text = currentCaptcha;
        }

        private void RefreshCaptchaBtn_Click(object sender, RoutedEventArgs e)
        {
            GenerateNewCaptcha();
            CaptchaInputBox.Clear();
        }

        private void BtnReg_Click(object sender, RoutedEventArgs e)
        {
            string errors = CheckErrors();
            if (errors.Length > 0)
            {
                MessageBox.Show(errors, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                GenerateNewCaptcha();
                CaptchaInputBox.Clear();
            }
            else
            {
                var newUser = new Entities.Users
                {
                    Surname = TBoxSurname.Text.Trim(),
                    Name = TBoxName.Text.Trim(),
                    Email = TBoxEmail.Text.Trim(),
                    Number_phone = TBoxPhone.Text.Trim(),
                    Login = TBoxRegLog.Text.Trim(),
                    Password = TBoxRegPas.Text.Trim(),
                    ID_role = 1
                };

                App.Context.Users.Add(newUser);
                App.Context.SaveChanges();

                MessageBox.Show("Вы успешно зарегистрированы!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new AuthorizationPage());
            }
        }

        private string CheckErrors()
        {
            var errors = new StringBuilder();

            if (string.IsNullOrEmpty(TBoxSurname.Text)) errors.AppendLine("Введите фамилию!");
            if (string.IsNullOrEmpty(TBoxName.Text)) errors.AppendLine("Введите имя!");
            if (string.IsNullOrEmpty(TBoxEmail.Text) || !TBoxEmail.Text.Contains("@")) errors.AppendLine("Введите корректный Email!");
            if (string.IsNullOrEmpty(TBoxRegLog.Text)) errors.AppendLine("Логин не может быть пустым!");
            if (string.IsNullOrEmpty(TBoxRegPas.Text)) errors.AppendLine("Пароль не может быть пустым!");

            if (CaptchaInputBox.Text != currentCaptcha)
                errors.AppendLine("Капча введена неверно!");

            var existingUser = App.Context.Users.FirstOrDefault(p => p.Login == TBoxRegLog.Text.Trim());
            if (existingUser != null)
                errors.AppendLine("Этот логин уже занят!");

            return errors.ToString();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AuthorizationPage());
        }
    }
}
