using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Radikool7.Test.Radio
{
    public class Nhk
    {
        private ITestOutputHelper _output;
        public Nhk(ITestOutputHelper output)
        {
            _output = output;
        }
        
        
        [Fact]
        public async void GetStationsTest()
        {
            var stations = await Radikool7.Radio.Nhk.GetStations();
            Assert.NotEmpty(stations);
        }

        [Fact]
        public async void GetProgramTest()
        {
            var stations = await Radikool7.Radio.Nhk.GetStations();
            var programs = await Radikool7.Radio.Nhk.GetPrograms(stations.First(), DateTime.Now, DateTime.Now.AddDays(1));
            
            Assert.NotEmpty(programs);
        }

        
        
    }
}