using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Windows.Input;

namespace AeroportSchedule.ViewModels
{
    public class AirportEditorViewModel : ViewModelBase
    {
        private readonly int? _airportId;
        private Airport _airport;

        public AirportEditorViewModel(int? airportId = null)
        {
            _airportId = airportId;

            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(() => RequestClose?.Invoke(false));

            LoadAirport();
        }

        public event Action<bool> RequestClose;

        public Airport Airport
        {
            get => _airport;
            set => SetProperty(ref _airport, value);
        }

        public string Title => _airportId.HasValue
            ? "Редактирование аэропорта"
            : "Новый аэропорт";

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private void LoadAirport()
        {
            if (_airportId.HasValue)
            {
                using (var ctx = new AirportScheduleDBEntities())
                {
                    var a = ctx.Airport
                        .AsNoTracking()
                        .FirstOrDefault(x => x.AirportID == _airportId.Value);

                    Airport = a ?? new Airport();
                }
            }
            else
            {
                Airport = new Airport();
            }
        }

        private bool CanSave()
        {
            return Airport != null
                && !string.IsNullOrWhiteSpace(Airport.IATACode)
                && Airport.IATACode.Length == 3
                && !string.IsNullOrWhiteSpace(Airport.Name)
                && !string.IsNullOrWhiteSpace(Airport.City)
                && !string.IsNullOrWhiteSpace(Airport.Country);
        }

        private void Save()
        {
            using (var ctx = new AirportScheduleDBEntities())
            {
                // Проверка уникальности IATA-кода
                bool codeExists = ctx.Airport.Any(a =>
                    a.IATACode == Airport.IATACode &&
                    (!_airportId.HasValue || a.AirportID != _airportId.Value));

                if (codeExists)
                {
                    System.Windows.MessageBox.Show(
                        $"Аэропорт с кодом IATA {Airport.IATACode} уже существует.",
                        "Ошибка", System.Windows.MessageBoxButton.OK,
                        System.Windows.MessageBoxImage.Warning);
                    return;
                }

                if (_airportId.HasValue)
                {
                    ctx.Airport.Attach(Airport);
                    ctx.Entry(Airport).State = EntityState.Modified;
                }
                else
                {
                    ctx.Airport.Add(Airport);
                }

                ctx.SaveChanges();
            }

            RequestClose?.Invoke(true);
        }
    }
}
