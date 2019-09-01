using System.Reactive.Disposables;
using Radikool7.Classes;
using Radikool7.Entities;
using Radikool7.Models;
using Radikool7.Radios;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Radikool7.ViewModels
{
    public class StationViewModel : IViewModel
    {
        public ReactiveCollection<RadioStation> RadikoStations { get; } = new ReactiveCollection<RadioStation>();
        public ReactiveCollection<RadioStation> NhkStations { get; } = new ReactiveCollection<RadioStation>();
        public ReactiveCollection<RadioStation> ListenRadioStations { get; } = new ReactiveCollection<RadioStation>();
        private MainModel _model;
        public async void Init(MainModel model, CompositeDisposable disposable)
        {
            _model = model;
            RadikoStations.AddRangeOnScheduler(await Radiko.GetStations());
            RadikoStations.AddTo(disposable);
            
            NhkStations.AddRangeOnScheduler(await Nhk.GetStations());
            NhkStations.AddTo(disposable);
            
            ListenRadioStations.AddRangeOnScheduler(await ListenRadio.GetStations());
            ListenRadioStations.AddTo(disposable);
        }
    }
}