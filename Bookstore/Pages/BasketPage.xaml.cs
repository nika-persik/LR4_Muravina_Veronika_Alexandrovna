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
    /// Логика взаимодействия для BasketPage.xaml
    /// </summary>
    public partial class BasketPage : Page
    {
        public BasketPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateBasket();
        }

        private void UpdateBasket()
        {
            BasketDataGrid.ItemsSource = null;
            BasketDataGrid.ItemsSource = Basket.Items;

            decimal total = Basket.Items.Sum(x => x.Summa);
            TotalSumTextBlock.Text = $"Итого к оплате: {total} руб.";
        }

        private void BtnCheckout_Click(object sender, RoutedEventArgs e)
        {
            if (Basket.Items.Count == 0)
            {
                MessageBox.Show("Корзина пуста!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var newOrder = new Entities.Orders
                {
                    ID_user = App.CurrentUser.ID_user,
                    Order_date = DateTime.Now,
                    Status = "Новый"
                };

                App.Context.Orders.Add(newOrder);
                App.Context.SaveChanges();

                foreach (var item in Basket.Items)
                {
                    var orderItem = new Entities.Order_items
                    {
                        ID_order = newOrder.ID_order,
                        ID_book = item.ID_book,
                        Quantity = item.Count,
                        Unit_price = item.Price
                    };
                    App.Context.Order_items.Add(orderItem);

                    var book = App.Context.Books.FirstOrDefault(b => b.ID_book == item.ID_book);
                    if (book != null)
                    {
                        book.Stock_quantity -= item.Count;
                    }
                }

                App.Context.SaveChanges();

                MessageBox.Show("Заказ оформлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                Basket.Items.Clear();
                NavigationService.Navigate(new BooksPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при оформлении заказа: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new BooksPage());
    }
}
