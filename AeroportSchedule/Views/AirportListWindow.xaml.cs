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
using AeroportSchedule.Services;
using AeroportSchedule.ViewModels;

namespace AeroportSchedule.Views
{
    /// <summary>
    /// Логика взаимодействия для AirportListWindow.xaml
    /// </summary>
    public partial class AirportListWindow : Window
    {
        private readonly AirportListViewModel _vm;

        public AirportListWindow()
        {
            InitializeComponent();
            _vm = new AirportListViewModel(new DialogService());
            DataContext = _vm;
            Loaded += (s, e) => _vm.Load();
        }
    }
}
