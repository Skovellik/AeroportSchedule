using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AeroportSchedule.Services
{
    public interface IDialogService
    {
        bool? ShowFlightEditor(int? flightId = null);
        void ShowAirports();
        void ShowAirlines();
        void ShowAircrafts();
        void ShowPassengers();
        void ShowTicketSale(int? flightId = null);
        void ShowMessage(string text, string caption = "Сообщение");
        bool Confirm(string text, string caption = "Подтверждение");
    }

    public class DialogService : IDialogService
    {
        public bool? ShowFlightEditor(int? flightId = null)
        {
            var wnd = new Views.FlightEditorWindow(flightId);
            return wnd.ShowDialog();
        }

        public void ShowAirports() => new Views.AirportListWindow().ShowDialog();
        public void ShowAirlines() => new Views.AirlineListWindow().ShowDialog();
        public void ShowAircrafts() => new Views.AircraftListWindow().ShowDialog();
        public void ShowPassengers() => new Views.PassengerListWindow().ShowDialog();

        public void ShowTicketSale(int? flightId = null)
        {
            new Views.TicketSaleWindow(flightId).ShowDialog();
        }

        public void ShowMessage(string text, string caption = "Сообщение")
            => MessageBox.Show(text, caption, MessageBoxButton.OK, MessageBoxImage.Information);

        public bool Confirm(string text, string caption = "Подтверждение")
            => MessageBox.Show(text, caption, MessageBoxButton.YesNo,
                               MessageBoxImage.Question) == MessageBoxResult.Yes;
    }
}
