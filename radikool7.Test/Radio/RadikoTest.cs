using System.Linq;
using System.Runtime.InteropServices;
using Radikool7.Radio;
using Radikool7.Test.Settings;
using Xunit;
using Xunit.Abstractions;

namespace Radikool7.Test.Radio
{
    public class RadikoTest
    {
        private ITestOutputHelper _output;
        public RadikoTest(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [Fact]
        public async void LoginTest()
        {
            var result = await Radiko.Login(TestSetting.RadikoEmail, TestSetting.RadikoPassword);
            Assert.True(result);

            result = await Radiko.LoginCheck();
            Assert.True(result);
        }
        
        [Fact]
        public async void LoginCheckTest()
        {
            var result = await Radiko.LoginCheck();
            Assert.False(result);
        }
        
        [Fact]
        public async void GetPrefIdTest()
        {
            var result = await Radiko.GetPrefId();
            Assert.Equal("JP28", result);
        }
        
        [Fact]
        public async void GetStationsTest()
        {
            var stations = await Radiko.GetStations();
            
            await Radiko.Login(TestSetting.RadikoEmail, TestSetting.RadikoPassword);
            var allStation = await Radiko.GetStations();
            
            Assert.NotEqual(stations.Count, allStation.Count);
        }

        [Fact]
        public async void GetProgram()
        {
            var stations = await Radiko.GetStations();
            var programs = await Radiko.GetPrograms(stations.First());
            
            Assert.NotEmpty(programs);
        }

        [Fact]
        public async void GetAuthTokenTest()
        {
            var token = await Radiko.GetAuthToken();
            Assert.NotEqual("", token);
        }

        [Fact]
        public async void GetTimeFree()
        {
           // await Radikool7.Radio.Radiko.Login(TestSetting.RadikoEmail, TestSetting.RadikoPassword);
            var station = (await Radiko.GetStations()).First();
            var program = (await Radiko.GetPrograms(station)).First();
            program.RadioStation = station;
            var m3u8 = await Radiko.GetTimeFreeM3U8(program);
            _output.WriteLine(m3u8);
            Assert.NotEqual("", m3u8);
        }
        
    }
}