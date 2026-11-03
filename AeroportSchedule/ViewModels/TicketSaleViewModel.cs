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
    public class TicketSaleViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogs;
        private readonly int? _preselectedFlightId;

        private ObservableCollection<Flight> _flights;
        private Flight _selectedFlight;
        private ObservableCollection<Passenger> _passengers;
        private Passenger _selectedPassenger;

        private string _ticketNumber;
        private string _seatNumber;
        private string _ticketClass = "Эконом";
        private decimal _price;
        private string _status = "Оплачен";

        public TicketSaleViewModel(IDialogService dialogs, int? preselectedFlightId = null)
        {
            _dialogs = dialogs;
            _preselectedFlightId = preselectedFlightId;

            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(() => RequestClose?.Invoke(false));

            LoadFlights();
            LoadPassengers();

            TicketNumber = GenerateTicketNumber();

            if (preselectedFlightId.HasValue)
                SelectedFlight = Flights.FirstOrDefault(f => f.FlightID == preselectedFlightId.Value);
        }

        public event Action<bool> RequestClose;

        public ObservableCollection<Flight> Flights
        {
            get => _flights;
            set => SetProperty(ref _flights, value);
        }

        public Flight SelectedFlight
        {
            get => _selectedFlight;
            set => SetProperty(ref _selectedFlight, value);
        }

        public ObservableCollection<Passenger> Passengers
        {
            get => _passengers;
            set => SetProperty(ref _passengers, value);
        }

        public Passenger SelectedPassenger
        {
            get => _selectedPassenger;
            set => SetProperty(ref _selectedPassenger, value);
        }

        public string TicketNumber { get => _ticketNumber; set => SetProperty(ref _ticketNumber, value); }
        public string SeatNumber { get => _seatNumber; set => SetProperty(ref _seatNumber, value); }
        public string TicketClass { get => _ticketClass; set => SetProperty(ref _ticketClass, value); }
        public decimal Price { get => _price; set => SetProperty(ref _price, value); }
        public string Status { get => _status; set => SetProperty(ref _status, value); }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private void LoadFlights()
        {
            using (var ctx = new AirportScheduleDBEntities())
            {
                // Грузим с Include, чтобы в ComboBox отображать понятную строку "SU1234 SVO → LED"
                Flights = new ObservableCollection<Flight>(
                    ctx.Flight
                       .Include(f => f.Airline)
                       .Include(f => f.Airport)   // аэропорт вылета
                       .Include(f => f.Airport1)   // аэропорт прилёта
                       .OrderBy(f => f.DepartureTime)
                       .ToList());
            }
        }

        private void LoadPassengers()
        {
            using (var ctx = new AirportScheduleDBEntities())
            {
                Passengers = new ObservableCollection<Passenger>(
                    ctx.Passenger.OrderBy(p => p.LastName).ToList());
            }
        }

        private string GenerateTicketNumber()
        {
            using (var ctx = new AirportScheduleDBEntities())
            {
                int count = ctx.Ticket.Count() + 1;
                return $"TK-{count:D6}";
            }
        }

        private bool CanSave()
        {
            return SelectedFlight != null
                && SelectedPassenger != null
                && !string.IsNullOrWhiteSpace(TicketNumber)
                && Price >= 0;
        }

        private void Save()
        {
            using (var ctx = new AirportScheduleDBEntities())
            {
                var ticket = new Ticket
                {
                    TicketNumber = TicketNumber,
                    FlightID = SelectedFlight.FlightID,
                    PassengerID = SelectedPassenger.PassengerID,
                    SeatNumber = SeatNumber,
                    Class = TicketClass,
                    Price = Price,
                    PurchaseDate = DateTime.Now,
                    Status = Status
                };
                ctx.Ticket.Add(ticket);
                ctx.SaveChanges();
            }

            _dialogs.ShowMessage($"Билет {TicketNumber} успешно оформлен.");
            RequestClose?.Invoke(true);
        }
    }
}
