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
    /// Логика взаимодействия для BooksPage.xaml
    /// </summary>
    public partial class BooksPage : Page
    {
        public BooksPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string roleName = "Гость";
            bool isAdmin = false;
            bool isGuest = (App.CurrentUser == null);

            if (isGuest == false && App.CurrentUser.Roles != null)
            {
                roleName = App.CurrentUser.Roles.Name_role;
                if (roleName == "Администратор" || App.CurrentUser.ID_role == 2)
                {
                    isAdmin = true;
                }
            }

            RoleTextBlock.Text = $"Вы зашли как: {roleName}";

            if (isAdmin == true)
            {
                BtnAddBook.Visibility = Visibility.Visible;
                BtnOrders.Visibility = Visibility.Visible;
            }
            else
            {
                BtnAddBook.Visibility = Visibility.Collapsed;
                BtnOrders.Visibility = Visibility.Collapsed;
            }

            if (isGuest == true)
            {
                BtnBasket.Visibility = Visibility.Collapsed;
                ColumnBasket.Visibility = Visibility.Collapsed;
            }
            else
            {
                BtnBasket.Visibility = Visibility.Visible;
                ColumnBasket.Visibility = Visibility.Visible;
            }

            if (isAdmin == true)
            {
                ColumnAction.Visibility = Visibility.Visible;
            }
            else
            {
                ColumnAction.Visibility = Visibility.Collapsed;
            }

            LoadGenres();
            UpdateBooks();
        }

        private void LoadGenres()
        {
            var genres = App.Context.Genres.Select(g => g.Name).ToList();
            genres.Insert(0, "Все жанры");
            ComboGenre.ItemsSource = genres;
            ComboGenre.SelectedIndex = 0;
        }

        private void UpdateBooks()
        {
            var books = App.Context.Books.ToList();

            if (!string.IsNullOrEmpty(TboxSearch.Text))
            {
                books = books.Where(p => p.Name.ToLower().Contains(TboxSearch.Text.ToLower())).ToList();
            }

            if (ComboGenre.SelectedItem != null && ComboGenre.SelectedItem.ToString() != "Все жанры")
            {
                string selectedGenre = ComboGenre.SelectedItem.ToString();
                books = books.Where(p => p.Genres != null && p.Genres.Name == selectedGenre).ToList();
            }

            BooksDataGrid.ItemsSource = books;
        }

        private void TboxSearch_TextChanged(object sender, TextChangedEventArgs e) => UpdateBooks();
        private void ComboGenre_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateBooks();

        private void BtnAddBook_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddBookPage());
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var book = (sender as Button).DataContext as Entities.Books;
            NavigationService.Navigate(new AddBookPage(book));
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            var book = (sender as Button).DataContext as Entities.Books;
            if (MessageBox.Show($"Удалить книгу \"{book.Name}\"?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    App.Context.Books.Remove(book);
                    App.Context.SaveChanges();
                    UpdateBooks();
                }
                catch
                {
                    MessageBox.Show("Нельзя удалить книгу, так как она фигурирует в заказах!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnToBasket_Click(object sender, RoutedEventArgs e)
        {
            if (App.CurrentUser == null)
            {
                MessageBox.Show("Гость не может добавлять товары в корзину. Пожалуйста, войдите в систему.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var book = (sender as Button).DataContext as Entities.Books;
            Basket.AddBook(book.ID_book, book.Name, book.Price);
            MessageBox.Show($"Книга \"{book.Name}\" добавлена в корзину!", "Корзина", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnBasket_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new BasketPage());
        private void BtnOrders_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new OrdersPage());
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentUser = null;
            NavigationService.Navigate(new AuthorizationPage());
        }
    }
}
