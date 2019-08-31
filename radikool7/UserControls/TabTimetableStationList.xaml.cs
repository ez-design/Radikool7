using System.Windows;
using System.Windows.Controls;
using Radikool7.Entities;
using Radikool7.ViewModels;

namespace Radikool7.UserControls
{
    public partial class TabTimetableStationList : UserControl
    {
        private readonly MainViewModel _vModel;
        public TabTimetableStationList()
        {
            InitializeComponent();
            _vModel = DataContext as MainViewModel;
        }

        /// <summary>
        /// 番組表表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonStation_OnClick(object sender, RoutedEventArgs e)
        {
            var station = (sender as Button)?.DataContext as RadioStation;
            _vModel.SetProgramStation(station);
        }
    }
}