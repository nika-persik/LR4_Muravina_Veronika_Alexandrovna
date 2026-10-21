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
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        public OrdersPage()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadOrders();
        }

        private void LoadOrders()
        {
            OrdersDataGrid.ItemsSource = App.Context.Orders.OrderByDescending(o => o.Order_date).ToList();
        }

        private void OrdersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedOrder = OrdersDataGrid.SelectedItem as Entities.Orders;
            if (selectedOrder != null)
            {
                OrderItemsDataGrid.ItemsSource = selectedOrder.Order_items.ToList();
            }
            else
            {
                OrderItemsDataGrid.ItemsSource = null;
            }
        }

        private void BtnUpdateStatus_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = OrdersDataGrid.SelectedItem as Entities.Orders;
            var selectedStatus = ComboStatus.SelectedItem as ComboBoxItem;

            if (selectedOrder == null || selectedStatus == null)
            {
                MessageBox.Show("Выберите заказ и новый статус!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            selectedOrder.Status = selectedStatus.Content.ToString();
            App.Context.SaveChanges();

            MessageBox.Show("Статус сохранен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadOrders();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new BooksPage());
    }
}
