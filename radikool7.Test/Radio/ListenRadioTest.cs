using System;
using System.Linq;
using Radikool7.Radio;
using Xunit;
using Xunit.Abstractions;

namespace Radikool7.Test.Radio
{
    public class ListenRadioTest
    {
        private ITestOutputHelper _output;
        public ListenRadioTest(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [Fact]
        public async void GetStationsTest()
        {
            var stations = await ListenRadio.GetStations();
            Assert.NotEmpty(stations);
        }

        [Fact]
        public async void GetProgramTest()
        {
            var stations = await ListenRadio.GetStations();
            var programs = await ListenRadio.GetPrograms(stations.First());

            Assert.NotEmpty(programs);
        }
    }
}