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
        public StationViewModel Station { get; } = new StationViewModel();
        public ProgramViewModel Program { get; } = new ProgramViewModel();
        public LibraryViewModel Library { get; } = new LibraryViewModel();

        public event PropertyChangedEventHandler PropertyChanged;


        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        public MainViewModel()
        {
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public async void Init(MainModel model)
        {
            Station.Init(model, _disposable);
            Program.Init(model, _disposable);
            Library.Init(model, _disposable);
        }

       

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}