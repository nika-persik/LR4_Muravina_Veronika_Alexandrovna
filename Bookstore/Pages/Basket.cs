using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Pages
{
    public static class Basket
    {
        public static ObservableCollection<BasketItem> Items { get; set; } = new ObservableCollection<BasketItem>();

        public static void AddBook(int id, string name, decimal price)
        {
            var existing = Items.FirstOrDefault(i => i.ID_book == id);
            if (existing != null)
            {
                existing.Count++;
            }
            else
            {
                Items.Add(new BasketItem { ID_book = id, BookName = name, Price = price, Count = 1 });
            }
        }
    }

    public class BasketItem
    {
        public int ID_book { get; set; }
        public string BookName { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public decimal Summa => Price * Count;
    }
}
