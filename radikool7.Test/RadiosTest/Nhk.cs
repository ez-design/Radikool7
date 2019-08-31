using System;
using System.Linq;
using Radikool7.Radios;
using Xunit;
using Xunit.Abstractions;

namespace Radikool7.Test.RadiosTest
{
    public class NhkTest
    {
        private ITestOutputHelper _output;
        public NhkTest(ITestOutputHelper output)
        {
            _output = output;
        }
        
        
        [Fact]
        public async void GetStationsTest()
        {
            var stations = await Nhk.GetStations();
            Assert.NotEmpty(stations);
        }

        [Fact]
        public async void GetProgramTest()
        {
            var stations = await Nhk.GetStations();
            var programs = await Nhk.GetPrograms(stations.First(), DateTime.Now, DateTime.Now.AddDays(1));
            
            Assert.NotEmpty(programs);
        }

        
        
    }
}