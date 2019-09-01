using System;
using System.Diagnostics;
using System.IO;
using Radikool7.Classes;
using Radikool7.Models;
using Xunit;
using Xunit.Abstractions;

namespace radikool7.Test.ModelsTest
{
    public class RadioStationModelTest : BaseTest
    {
        private readonly MainModel _model = new MainModel();
        private readonly string _fileName;
        
        public RadioStationModelTest(ITestOutputHelper output) : base(output)
        {
            _fileName = $"{Guid.NewGuid():N}.json";
            RadioStationModel.FileName = _fileName;
        }
        
        [Fact]
        public void GetTest()
        {
            Execute("GetTest", async () =>
            {
                var radiko = await _model.RadioStationModel.Get(Define.Radiko.TypeName);
                Assert.NotEmpty(radiko);
            
                var nhk = await _model.RadioStationModel.Get(Define.Nhk.TypeName);
                Assert.NotEmpty(nhk);
            
                var listenRadio = await _model.RadioStationModel.Get(Define.ListenRadio.TypeName);
                Assert.NotEmpty(listenRadio);

                var sw = new Stopwatch();
                sw.Start();
                var stations = await _model.RadioStationModel.Load();
                sw.Stop();
                Output.WriteLine($"{stations.Count}件: {sw.Elapsed}");
                
                File.Delete(_fileName);
            });
            
        }
    }
}