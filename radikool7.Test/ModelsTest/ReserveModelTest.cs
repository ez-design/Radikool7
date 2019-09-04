using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Radikool7.Classes;
using Radikool7.Entities;
using Radikool7.Models;
using Xunit;
using Xunit.Abstractions;

namespace radikool7.Test.ModelsTest
{
    public class ReserveModelTest : BaseTest
    {
        private readonly MainModel _model = new MainModel();
        private readonly string _fileName;
        
        public ReserveModelTest(ITestOutputHelper output) : base(output)
        {
            _fileName = $"{Guid.NewGuid():N}.json";
            ReserveModel.FileName = _fileName;
        }
        
        [Fact]
        public void GetTest()
        {
            Execute("GetTest", async () =>
            {
                var reserve = new Reserve()
                {
                    Name = "test",
                    Start = DateTime.Now,
                    End = DateTime.Now.AddHours(1),
                    Repeat = new List<int>(){ 1, 2 },
                    IsTimeFree = true
                };
                await _model.ReserveModel.Update(reserve);
                var reserve2 = await _model.ReserveModel.Get();
                Assert.Equal(reserve.Id, reserve2.First().Id);
                
                var tasks = await _model.ReserveModel.GetTasks(new Config());
                Assert.NotEmpty(tasks);
                
                
                File.Delete(_fileName);
            });
        }
    }
}