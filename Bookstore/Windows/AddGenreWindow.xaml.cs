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
    /// Логика взаимодействия для AddGenreWindow.xaml
    /// </summary>
    public partial class AddGenreWindow : Window
    {
        public Entities.Genres NewGenre { get; private set; }

        public AddGenreWindow()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TboxGenreName.Text))
            {
                MessageBox.Show("Введите название жанра!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string name = TboxGenreName.Text.Trim();

            var existingGenre = App.Context.Genres.FirstOrDefault(g => g.Name.ToLower() == name.ToLower());
            if (existingGenre != null)
            {
                MessageBox.Show("Такой жанр уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var genre = new Entities.Genres();
            genre.Name = name;

            if (string.IsNullOrWhiteSpace(TboxDescription.Text))
                genre.Description = null;
            else
                genre.Description = TboxDescription.Text.Trim();

            App.Context.Genres.Add(genre);
            App.Context.SaveChanges();

            NewGenre = genre;
            MessageBox.Show("Жанр успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
