using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Radikool7.Test.Radio
{
    public class ListenRadio
    {
        private ITestOutputHelper _output;
        public ListenRadio(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [Fact]
        public async void GetStationsTest()
        {
            var stations = await Radikool7.Radio.ListenRadio.GetStations();
            Assert.NotEmpty(stations);
        }

        [Fact]
        public async void GetProgramTest()
        {
            var stations = await Radikool7.Radio.ListenRadio.GetStations();
            var programs = await Radikool7.Radio.ListenRadio.GetPrograms(stations.First());

            Assert.NotEmpty(programs);
        }
    }
}