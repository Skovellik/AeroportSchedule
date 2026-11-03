using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace AeroportSchedule.ViewModels
{
    public class PassengerListViewModel : ViewModelBase
    {
        private ObservableCollection<Passenger> _passengers;
        private Passenger _selected;

        public PassengerListViewModel()
        {
            RefreshCommand = new RelayCommand(Load);
        }

        public ObservableCollection<Passenger> Passengers
        {
            get => _passengers;
            set => SetProperty(ref _passengers, value);
        }

        public Passenger Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value);
        }

        public ICommand RefreshCommand { get; }

        public void Load()
        {
            using (var ctx = new AirportScheduleDBEntities())
            {
                Passengers = new ObservableCollection<Passenger>(
                    ctx.Passenger.OrderBy(p => p.LastName).ToList());
            }
        }
    }
}
