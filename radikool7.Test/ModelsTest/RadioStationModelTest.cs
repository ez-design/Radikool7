using Radikool7.Classes;
using Radikool7.Models;
using Xunit;
using Xunit.Abstractions;

namespace radikool7.Test.ModelsTest
{
    public class RadioStationModelTest
    {
        private ITestOutputHelper _output;
        public RadioStationModelTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async void GetTest()
        {
            var model = new RadioStationModel();
            var radiko = await model.Get(Define.Radiko.TypeName);
            Assert.NotEmpty(radiko);
            
            var nhk = await model.Get(Define.Nhk.TypeName);
            Assert.NotEmpty(nhk);
            
            var listenRadio = await model.Get(Define.ListenRadio.TypeName);
            Assert.NotEmpty(listenRadio);
        }
    }
}