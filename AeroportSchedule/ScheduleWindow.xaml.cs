using AeroportSchedule.Models;
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
using AeroportSchedule.Services;
using AeroportSchedule.ViewModels;

namespace AeroportSchedule
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class ScheduleWindow : Window
    {
        private readonly ScheduleViewModel _vm;

        public ScheduleWindow()
        {
            InitializeComponent();
            _vm = new ScheduleViewModel(new DialogService());
            DataContext = _vm;
            Loaded += (s, e) => _vm.LoadFlights();
        }
    }
}
