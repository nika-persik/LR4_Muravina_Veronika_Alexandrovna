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
    /// Логика взаимодействия для AddBookPage.xaml
    /// </summary>
    public partial class AddBookPage : Page
    {
        private Entities.Books currentBook = null;

        public AddBookPage()
        {
            InitializeComponent();
            LoadComboBoxes();
        }

        public AddBookPage(Entities.Books book)
        {
            InitializeComponent();
            LoadComboBoxes();
            currentBook = book;

            if (currentBook != null)
            {
                HeaderTextBlock.Text = "Редактирование книги";
                TboxTitle.Text = currentBook.Name;
                ComboAuthor.SelectedItem = currentBook.Authors;
                ComboGenre.SelectedItem = currentBook.Genres;
                TboxYear.Text = currentBook.Year_of_publication.ToString();
                TboxPrice.Text = currentBook.Price.ToString();
                TboxStock.Text = currentBook.Stock_quantity.ToString();
            }
        }

        private void LoadComboBoxes()
        {
            ComboAuthor.ItemsSource = App.Context.Authors.ToList();
            ComboAuthor.DisplayMemberPath = "FullName";

            ComboGenre.ItemsSource = App.Context.Genres.ToList();
            ComboGenre.DisplayMemberPath = "Name";
        }

        private void BtnAddAuthor_Click(object sender, RoutedEventArgs e)
        {
            var window = new Windows.AddAuthorWindow();
            window.Owner = Window.GetWindow(this);

            if (window.ShowDialog() == true)
            {
                LoadComboBoxes();
                ComboAuthor.SelectedItem = window.NewAuthor;
            }
        }

        private void BtnAddGenre_Click(object sender, RoutedEventArgs e)
        {
            var window = new Windows.AddGenreWindow();
            window.Owner = Window.GetWindow(this);

            if (window.ShowDialog() == true)
            {
                LoadComboBoxes();
                ComboGenre.SelectedItem = window.NewGenre;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string errors = CheckErrors();
            if (errors.Length > 0)
            {
                MessageBox.Show(errors, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (currentBook == null)
            {
                var newBook = new Entities.Books();
                newBook.Name = TboxTitle.Text.Trim();
                newBook.ID_author = (ComboAuthor.SelectedItem as Entities.Authors).ID_author;
                newBook.ID_genre = (ComboGenre.SelectedItem as Entities.Genres).ID_genre;
                newBook.Year_of_publication = int.Parse(TboxYear.Text);
                newBook.Price = decimal.Parse(TboxPrice.Text);
                newBook.Stock_quantity = int.Parse(TboxStock.Text);

                App.Context.Books.Add(newBook);
            }
            else
            {
                currentBook.Name = TboxTitle.Text.Trim();
                currentBook.ID_author = (ComboAuthor.SelectedItem as Entities.Authors).ID_author;
                currentBook.ID_genre = (ComboGenre.SelectedItem as Entities.Genres).ID_genre;
                currentBook.Year_of_publication = int.Parse(TboxYear.Text);
                currentBook.Price = decimal.Parse(TboxPrice.Text);
                currentBook.Stock_quantity = int.Parse(TboxStock.Text);
            }

            App.Context.SaveChanges();
            MessageBox.Show("Данные успешно сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.GoBack();
        }

        private string CheckErrors()
        {
            var errors = new StringBuilder();

            if (string.IsNullOrEmpty(TboxTitle.Text))
            {
                errors.AppendLine("Укажите название книги!");
            }

            if (ComboAuthor.SelectedItem == null)
            {
                errors.AppendLine("Выберите автора!");
            }

            if (ComboGenre.SelectedItem == null)
            {
                errors.AppendLine("Выберите жанр!");
            }

            int year = 0;
            if (int.TryParse(TboxYear.Text, out year) == false)
            {
                errors.AppendLine("Год должен быть целым числом!");
            }

            decimal price = 0;
            if (decimal.TryParse(TboxPrice.Text, out price) == false)
            {
                errors.AppendLine("Некорректная цена!");
            }
            else
            {
                if (price <= 0)
                {
                    errors.AppendLine("Цена должна быть больше нуля!");
                }
            }

            int stock = 0;
            if (int.TryParse(TboxStock.Text, out stock) == false)
            {
                errors.AppendLine("Количество на складе должно быть целым числом!");
            }
            else
            {
                if (stock < 0)
                {
                    errors.AppendLine("Количество на складе не может быть отрицательным!");
                }
            }

            return errors.ToString();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
