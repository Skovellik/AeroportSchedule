using AeroportSchedule.Models;
using AeroportSchedule.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace AeroportSchedule.ViewModels
{
    public class ScheduleViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogs;
        private ObservableCollection<vw_FlightSchedule> _flights;
        private vw_FlightSchedule _selectedFlight;
        private string _searchText;

        public ScheduleViewModel(IDialogService dialogs)
        {
            _dialogs = dialogs;

            RefreshCommand = new RelayCommand(LoadFlights);
            AddFlightCommand = new RelayCommand(AddFlight);
            EditFlightCommand = new RelayCommand(EditFlight, () => SelectedFlight != null);
            DeleteFlightCommand = new RelayCommand(DeleteFlight, () => SelectedFlight != null);
            AirportsCommand = new RelayCommand(() => _dialogs.ShowAirports());
            AirlinesCommand = new RelayCommand(() => _dialogs.ShowAirlines());
            AircraftsCommand = new RelayCommand(() => _dialogs.ShowAircrafts());
            PassengersCommand = new RelayCommand(() => _dialogs.ShowPassengers());
            SellTicketCommand = new RelayCommand(SellTicket, () => SelectedFlight != null);
        }

        public ObservableCollection<vw_FlightSchedule> Flights
        {
            get => _flights;
            set => SetProperty(ref _flights, value);
        }

        public vw_FlightSchedule SelectedFlight
        {
            get => _selectedFlight;
            set => SetProperty(ref _selectedFlight, value);
        }

        public string SearchText
        {
            get => _searchText;
            set { if (SetProperty(ref _searchText, value)) LoadFlights(); }
        }

        public ICommand RefreshCommand { get; }
        public ICommand AddFlightCommand { get; }
        public ICommand EditFlightCommand { get; }
        public ICommand DeleteFlightCommand { get; }
        public ICommand AirportsCommand { get; }
        public ICommand AirlinesCommand { get; }
        public ICommand AircraftsCommand { get; }
        public ICommand PassengersCommand { get; }
        public ICommand SellTicketCommand { get; }

        public void LoadFlights()
        {
            using (var ctx = new AirportScheduleDBEntities())
            {
                var sql = "SELECT * FROM dbo.vw_FlightSchedule";
                var query = ctx.Database.SqlQuery<vw_FlightSchedule>(sql).AsQueryable();

                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    var t = SearchText.Trim().ToLower();
                    query = query.Where(f =>
                        f.FlightNumber.ToLower().Contains(t) ||
                        f.DepartureCity.ToLower().Contains(t) ||
                        f.ArrivalCity.ToLower().Contains(t) ||
                        f.AirlineName.ToLower().Contains(t));
                }

                Flights = new ObservableCollection<vw_FlightSchedule>(
                    query.OrderBy(f => f.DepartureTime).ToList());
            }
        }

        private void AddFlight()
        {
            if (_dialogs.ShowFlightEditor() == true)
                LoadFlights();
        }

        private void EditFlight()
        {
            if (SelectedFlight == null) return;
            if (_dialogs.ShowFlightEditor(SelectedFlight.FlightID) == true)
                LoadFlights();
        }

        private void DeleteFlight()
        {
            if (SelectedFlight == null) return;
            if (!_dialogs.Confirm($"Удалить рейс {SelectedFlight.FlightNumber}?"))
                return;

            using (var ctx = new AirportScheduleDBEntities())
            {
                var f = ctx.Flight.Find(SelectedFlight.FlightID);
                if (f != null)
                {
                    ctx.Flight.Remove(f);
                    ctx.SaveChanges();
                }
            }
            LoadFlights();
        }

        private void SellTicket()
        {
            if (SelectedFlight == null) return;
            _dialogs.ShowTicketSale(SelectedFlight.FlightID);
        }
    }
}
