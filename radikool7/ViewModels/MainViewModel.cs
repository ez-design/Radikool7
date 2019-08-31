using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using Radikool7.Annotations;
using Radikool7.Classes;
using Radikool7.Entities;
using Radikool7.Radios;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Radikool7.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged, IDisposable
    {
        public ReactiveCollection<RadioStation> RadikoStations { get; } = new ReactiveCollection<RadioStation>();
        public ReactiveCollection<RadioProgram> RadikoPrograms { get; } = new ReactiveCollection<RadioProgram>();
        public event PropertyChangedEventHandler PropertyChanged;
        
        public MainViewModel()
        {
        }

        public async void Init()
        {
            RadikoStations.AddRangeOnScheduler(await Radiko.GetStations());
        }

        public void SetProgramStation(RadioStation station)
        {
            
        }

        public void Dispose()
        {
            RadikoStations?.Dispose();
            RadikoPrograms?.Dispose();
        }
    }
}