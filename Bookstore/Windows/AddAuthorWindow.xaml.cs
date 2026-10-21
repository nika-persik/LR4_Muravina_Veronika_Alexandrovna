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

namespace Bookstore.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddAuthorWindow.xaml
    /// </summary>
    public partial class AddAuthorWindow : Window
    {
        public Entities.Authors NewAuthor { get; private set; }

        public AddAuthorWindow()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TboxSurname.Text))
            {
                MessageBox.Show("Введите фамилию автора!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(TboxName.Text))
            {
                MessageBox.Show("Введите имя автора!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (DpDateOfBirth.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату рождения автора!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var author = new Entities.Authors();
            author.Surname = TboxSurname.Text.Trim();
            author.Name = TboxName.Text.Trim();
            author.Date_of_birth = DpDateOfBirth.SelectedDate.Value;

            if (string.IsNullOrWhiteSpace(TboxPatronymic.Text))
            {
                author.Patronymic = null;
            }
            else
            {
                author.Patronymic = TboxPatronymic.Text.Trim();
            }

            if (DpDateOfDeath.SelectedDate == null)
            {
                author.Date_of_death = null;
            }
            else
            {
                author.Date_of_death = DpDateOfDeath.SelectedDate;
            }

            if (string.IsNullOrWhiteSpace(TboxBiography.Text))
            {
                author.Biography = null;
            }
            else
            {
                author.Biography = TboxBiography.Text.Trim();
            }

            App.Context.Authors.Add(author);
            App.Context.SaveChanges();

            NewAuthor = author;
            MessageBox.Show("Автор успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
