using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Data.Entity;

namespace AeroportSchedule.ViewModels
{
    public class AircraftListViewModel : ViewModelBase
    {
        private ObservableCollection<Aircraft> _aircrafts;
        private Aircraft _selected;

        public AircraftListViewModel()
        {
            RefreshCommand = new RelayCommand(Load);
        }

        public ObservableCollection<Aircraft> Aircrafts
        {
            get => _aircrafts;
            set => SetProperty(ref _aircrafts, value);
        }

        public Aircraft Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value);
        }

        public ICommand RefreshCommand { get; }

        public void Load()
        {
            using (var ctx = new AirportScheduleDBEntities())
            {
                Aircrafts = new ObservableCollection<Aircraft>(
                    ctx.Aircraft
                       .Include(a => a.Airline)
                       .Include(a => a.AircraftType)
                       .OrderBy(a => a.RegistrationNo)
                       .ToList());
            }
        }
    }
}
