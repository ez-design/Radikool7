using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using Radikool7.Entities;
using Radikool7.Models;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Radikool7.ViewModels
{
    public class ProgramViewModel : IViewModel
    {
        public ReactiveCollection<RadioProgram> RadikoPrograms { get; } = new ReactiveCollection<RadioProgram>();
        public ReactiveCommand SetTimetableStationCmd { get; } = new ReactiveCommand();
        public ReactiveCommand RefreshProgramCmd { get; } = new ReactiveCommand();

        private MainModel _model;
        
        public void Init(MainModel model, CompositeDisposable disposable)
        {
            _model = model;
            RadikoPrograms.AddTo(disposable);
            // コマンド
            SetTimetableStationCmd.Subscribe(SetTimetableStation).AddTo(disposable);
            RefreshProgramCmd.Subscribe(RefreshProgram).AddTo(disposable);
        }

        /// <summary>
        /// 番組データをサーバから取得
        /// </summary>
        /// <param name="data"></param>
        private async void RefreshProgram(object data)
        {
            if (data is RadioStation station)
            {
                var programs = await _model.RadioProgramModel.Refresh(station);
            }
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

    }
}