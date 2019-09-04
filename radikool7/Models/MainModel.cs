namespace Radikool7.Models
{
    public class MainModel
    {
        public RadioStationModel RadioStationModel { get; } = new RadioStationModel();
        public RadioProgramModel RadioProgramModel { get; } = new RadioProgramModel();
        public ReserveModel ReserveModel { get; } = new ReserveModel();
        
        public ConfigModel ConfigModel { get;  } = new ConfigModel();
    }
}