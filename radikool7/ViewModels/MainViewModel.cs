using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Disposables;
using Radikool7.Entities;
using Radikool7.Models;
using Radikool7.Radios;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Radikool7.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged, IDisposable
    {
        public ReactiveCollection<RadioStation> RadikoStations { get; } = new ReactiveCollection<RadioStation>();
        public ReactiveCollection<RadioProgram> RadikoPrograms { get; } = new ReactiveCollection<RadioProgram>();
        

        public ReactiveCommand SetTimetableStationCmd { get; } = new ReactiveCommand();

        public event PropertyChangedEventHandler PropertyChanged;


        private MainModel _model;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        public MainViewModel()
        {
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public async void Init(MainModel model)
        {
            _model = model;
            RadikoStations.AddRangeOnScheduler(await Radiko.GetStations());

            RadikoPrograms.AddTo(_disposable);
            RadikoStations.AddTo(_disposable);

            SetTimetableStationCmd.Subscribe(SetTimetableStation);
        }

        /// <summary>
        /// 番組表表示用放送局指定
        /// </summary>
        /// <param name="data"></param>
        private async void SetTimetableStation(object data)
        {
            if (data is RadioStation station)
            {
                var programs = await _model.RadioProgramModel.Search(new SearchCondition() {StationIds = new List<string>() {station.Id}});
            }
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}