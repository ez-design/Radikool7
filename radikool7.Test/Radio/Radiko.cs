using System.Linq;
using System.Runtime.InteropServices;
using radikool7.Test;
using Xunit;
using Xunit.Abstractions;

namespace Radikool7.Test.Radio
{
    public class Radiko
    {
        private ITestOutputHelper _output;
        public Radiko(ITestOutputHelper output)
        {
            _output = output;
        }
        
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
            Assert.Equal("JP13", result);
        }
        
        [Fact]
        public async void GetStationsTest()
        {
            var stations = await Radikool7.Radio.Radiko.GetStations();
            
            await Radikool7.Radio.Radiko.Login(TestSetting.RadikoEmail, TestSetting.RadikoPassword);
            var allStation = await Radikool7.Radio.Radiko.GetStations();
            
            Assert.NotEqual(stations.Count, allStation.Count);
        }

        [Fact]
        public async void GetProgram()
        {
            var stations = await Radikool7.Radio.Radiko.GetStations();
            var programs = await Radikool7.Radio.Radiko.GetPrograms(stations.First());
            
            Assert.NotEmpty(programs);
        }

        [Fact]
        public async void GetAuthTokenTest()
        {
            var token = await Radikool7.Radio.Radiko.GetAuthToken();
            Assert.NotEqual("", token);
        }

        [Fact]
        public async void GetTimeFree()
        {
           // await Radikool7.Radio.Radiko.Login(TestSetting.RadikoEmail, TestSetting.RadikoPassword);
            var station = (await Radikool7.Radio.Radiko.GetStations()).First();
            var program = (await Radikool7.Radio.Radiko.GetPrograms(station)).First();
            program.RadioStation = station;
            var m3u8 = await Radikool7.Radio.Radiko.GetTimeFreeM3U8(program);
            _output.WriteLine(m3u8);
            Assert.NotEqual("", m3u8);
        }
        
    }
}