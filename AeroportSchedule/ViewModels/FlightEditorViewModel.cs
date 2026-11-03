using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Windows.Input;

namespace AeroportSchedule.ViewModels
{
    public class FlightEditorViewModel : ViewModelBase
    {
        private readonly int? _flightId;
        private Flight _flight;
        private ObservableCollection<Airline> _airlines;
        private ObservableCollection<Airport> _airports;
        private ObservableCollection<Aircraft> _aircrafts;

        public FlightEditorViewModel(int? flightId)
        {
            _flightId = flightId;
            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(() => RequestClose?.Invoke(false));

            LoadDictionaries();
            LoadFlight();
        }

        public event Action<bool> RequestClose;

        public Flight Flight
        {
            get => _flight;
            set => SetProperty(ref _flight, value);
        }

        public ObservableCollection<Airline> Airlines
        {
            get => _airlines;
            set => SetProperty(ref _airlines, value);
        }

        public ObservableCollection<Airport> Airports
        {
            get => _airports;
            set => SetProperty(ref _airports, value);
        }

        public ObservableCollection<Aircraft> Aircrafts
        {
            get => _aircrafts;
            set => SetProperty(ref _aircrafts, value);
        }

        public string Title => _flightId.HasValue ? "Редактирование рейса" : "Новый рейс";

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private void LoadDictionaries()
        {
            using (var ctx = new AirportScheduleDBEntities())
            {
                Airlines = new ObservableCollection<Airline>(ctx.Airline.OrderBy(a => a.Name).ToList());
                Airports = new ObservableCollection<Airport>(ctx.Airport.OrderBy(a => a.IATACode).ToList());
                Aircrafts = new ObservableCollection<Aircraft>(ctx.Aircraft.OrderBy(a => a.RegistrationNo).ToList());
            }
        }

        private void LoadFlight()
        {
            if (_flightId.HasValue)
            {
                using (var ctx = new AirportScheduleDBEntities())
                {
                    var f = ctx.Flight.AsNoTracking().FirstOrDefault(x => x.FlightID == _flightId.Value);
                    Flight = f ?? new Flight { DepartureTime = DateTime.Now, ArrivalTime = DateTime.Now.AddHours(2) };
                }
            }
            else
            {
                Flight = new Flight
                {
                    DepartureTime = DateTime.Now,
                    ArrivalTime = DateTime.Now.AddHours(2),
                    Status = "По расписанию"
                };
            }
        }

        private bool CanSave()
        {
            return Flight != null
                && !string.IsNullOrWhiteSpace(Flight.FlightNumber)
                && Flight.DepartureAirportID > 0
                && Flight.ArrivalAirportID > 0
                && Flight.DepartureAirportID != Flight.ArrivalAirportID
                && Flight.ArrivalTime > Flight.DepartureTime;
        }

        private void Save()
        {
            using (var ctx = new AirportScheduleDBEntities())
            {
                if (_flightId.HasValue)
                {
                    ctx.Flight.Attach(Flight);
                    ctx.Entry(Flight).State = EntityState.Modified;
                }
                else
                {
                    ctx.Flight.Add(Flight);
                }
                try
                {
                    ctx.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                {
                    var sb = new System.Text.StringBuilder();
                    foreach (var eve in ex.EntityValidationErrors)
                    {
                        sb.AppendLine($"Сущность: {eve.Entry.Entity.GetType().Name} (State: {eve.Entry.State})");
                        foreach (var ve in eve.ValidationErrors)
                        {
                            sb.AppendLine($"  Поле: {ve.PropertyName} — Ошибка: {ve.ErrorMessage}");
                        }
                    }
                    System.Windows.MessageBox.Show(sb.ToString(), "Ошибка валидации");
                    throw;
                }
            }
            RequestClose?.Invoke(true);
        }
    }
}
