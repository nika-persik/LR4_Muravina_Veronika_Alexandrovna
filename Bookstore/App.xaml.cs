using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Bookstore
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Entities.Online_bookstoreEntities Context { get; } = new Entities.Online_bookstoreEntities();
        public static Entities.Users CurrentUser = null;
    }
}
