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
    /// Логика взаимодействия для AirportEditorWindow.xaml
    /// </summary>
    public partial class AirportEditorWindow : Window
    {
        private readonly AirportEditorViewModel _vm;

        public AirportEditorWindow(int? airportId = null)
        {
            InitializeComponent();

            _vm = new AirportEditorViewModel(airportId);

            _vm.RequestClose += result =>
            {
                DialogResult = result;
                Close();
            };

            DataContext = _vm;
        }
    }
}
