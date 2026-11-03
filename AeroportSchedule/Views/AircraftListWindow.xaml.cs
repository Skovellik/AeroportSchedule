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
    /// Логика взаимодействия для AircraftListWindow.xaml
    /// </summary>
    public partial class AircraftListWindow : Window
    {
        private readonly AircraftListViewModel _vm;
        public AircraftListWindow()
        {
            InitializeComponent();
            _vm = new AircraftListViewModel();
            DataContext = _vm;
            Loaded += (s, e) => _vm.Load();
        }
    }
}
