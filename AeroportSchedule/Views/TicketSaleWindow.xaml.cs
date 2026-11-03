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
    /// Логика взаимодействия для TicketSaleWindow.xaml
    /// </summary>
    public partial class TicketSaleWindow : Window
    {
        public TicketSaleWindow(int? flightId = null)
        {
            InitializeComponent();
            var vm = new TicketSaleViewModel(new Services.DialogService(), flightId);
            vm.RequestClose += result =>
            {
                DialogResult = result;
                Close();
            };
            DataContext = vm;
        }
    }
}
