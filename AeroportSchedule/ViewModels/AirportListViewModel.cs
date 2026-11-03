using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Windows.Input;
using AeroportSchedule.Services;

namespace AeroportSchedule.ViewModels
{
    public class AirportListViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogs;
        private ObservableCollection<Airport> _airports;
        private Airport _selected;

        public AirportListViewModel(IDialogService dialogs)
        {
            _dialogs = dialogs;
            RefreshCommand = new RelayCommand(Load);
            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit, () => Selected != null);
            DeleteCommand = new RelayCommand(Delete, () => Selected != null);
        }

        public ObservableCollection<Airport> Airports
        {
            get => _airports;
            set => SetProperty(ref _airports, value);
        }

        public Airport Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value);
        }

        public ICommand RefreshCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }

        public void Load()
        {
            using (var ctx = new AirportScheduleDBEntities())
            {
                Airports = new ObservableCollection<Airport>(
                    ctx.Airport.OrderBy(a => a.IATACode).ToList());
            }
        }

        private void Add()
        {
            // Диалог редактирования аэропорта (можно сделать отдельное окно
            // AirportEditorWindow — по аналогии с FlightEditorWindow)
            // Для простоты используем простой ввод:
            var wnd = new Views.AirportEditorWindow(null);
            if (wnd.ShowDialog() == true) Load();
        }

        private void Edit()
        {
            if (Selected == null) return;
            var wnd = new Views.AirportEditorWindow(Selected.AirportID);
            if (wnd.ShowDialog() == true) Load();
        }

        private void Delete()
        {
            if (Selected == null) return;
            if (!_dialogs.Confirm($"Удалить аэропорт {Selected.IATACode}?")) return;

            using (var ctx = new AirportScheduleDBEntities())
            {
                // Проверка на использование в рейсах
                bool used = ctx.Flight.Any(f => f.DepartureAirportID == Selected.AirportID
                                             || f.ArrivalAirportID == Selected.AirportID);
                if (used)
                {
                    _dialogs.ShowMessage("Аэропорт используется в рейсах. Удаление невозможно.");
                    return;
                }

                var a = ctx.Airport.Find(Selected.AirportID);
                if (a != null) { ctx.Airport.Remove(a); ctx.SaveChanges(); }
            }
            Load();
        }
    }
}
