using System.Runtime.InteropServices;
using radikool7.Test;
using Xunit;

namespace Radikool7.Test.Radio
{
    public class Radiko
    {
        [Fact]
        public async void LoginTest()
        {
            var result = await Radikool7.Radio.Radiko.Login(TestSetting.RadikoEmail, TestSetting.RadikoPassword);
            Assert.True(result);

            result = await Radikool7.Radio.Radiko.LoginCheck();
            Assert.True(result);
        }
        
        [Fact]
        public async void LoginCheckTest()
        {
            var result = await Radikool7.Radio.Radiko.LoginCheck();
            Assert.False(result);
        }
        
        [Fact]
        public async void GetPrefIdTest()
        {
            var result = await Radikool7.Radio.Radiko.GetPrefId();
            Assert.Equal("JP28", result);
        }
        
        [Fact]
        public async void GetStationsTest()
        {
            var stations = await Radikool7.Radio.Radiko.GetStations();
            
            await Radikool7.Radio.Radiko.Login(TestSetting.RadikoEmail, TestSetting.RadikoPassword);
            var allStation = await Radikool7.Radio.Radiko.GetStations();
            
            Assert.NotEqual(stations.Count, allStation.Count);
        }
    }
}