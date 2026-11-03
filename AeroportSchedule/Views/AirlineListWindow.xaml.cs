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
using AeroportSchedule.ViewModels;

namespace AeroportSchedule.Views
{
    /// <summary>
    /// Логика взаимодействия для AirlineListWindow.xaml
    /// </summary>
    public partial class AirlineListWindow : Window
    {
        private readonly AirlineListViewModel _vm;
        public AirlineListWindow()
        {
            InitializeComponent();
            _vm = new AirlineListViewModel();
            DataContext = _vm;
            Loaded += (s, e) => _vm.Load();
        }
    }
}
