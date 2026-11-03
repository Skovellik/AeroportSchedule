using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AeroportSchedule.ViewModels
{
    public class AirlineListViewModel : ViewModelBase
    {
        private ObservableCollection<Airline> _airlines;
        private Airline _selected;

        public AirlineListViewModel()
        {
            RefreshCommand = new RelayCommand(Load);
        }

        public ObservableCollection<Airline> Airlines
        {
            get => _airlines;
            set => SetProperty(ref _airlines, value);
        }

        public Airline Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value);
        }

        public ICommand RefreshCommand { get; }

        public void Load()
        {
            using (var ctx = new AirportScheduleDBEntities())
            {
                Airlines = new ObservableCollection<Airline>(
                    ctx.Airline.OrderBy(a => a.Name).ToList());
            }
        }
    }
}
