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
    /// Логика взаимодействия для FlightEditorWindow.xaml
    /// </summary>
    public partial class FlightEditorWindow : Window
    {


        private readonly FlightEditorViewModel _vm;

        // ↓↓↓ Вот такой конструктор с параметром ↓↓↓
        public FlightEditorWindow(int? flightId = null)
        {
            InitializeComponent();
            _vm = new FlightEditorViewModel(flightId);
            _vm.RequestClose += result =>
            {
                DialogResult = result;
                Close();
            };
            DataContext = _vm;
        }
    }
}
