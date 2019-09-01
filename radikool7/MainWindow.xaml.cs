﻿using Radikool7.Models;
using Radikool7.ViewModels;

namespace Radikool7
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            var vModel = new MainViewModel();
            vModel.Init(new MainModel());

            DataContext = vModel;
            InitializeComponent();
        }
    }
}