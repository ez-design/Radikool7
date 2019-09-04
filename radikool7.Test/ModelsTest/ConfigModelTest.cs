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
    public class ConfigModelTest : BaseTest
    {
        private readonly MainModel _model = new MainModel();
        private readonly string _fileName;
        
        public ConfigModelTest(ITestOutputHelper output) : base(output)
        {
            _fileName = $"{Guid.NewGuid():N}.json";
            ConfigModel.FileName = _fileName;
        }
        
        [Fact]
        public void GetTest()
        {
            Execute("GetTest", async () =>
            {
                var config = new Config()
                {
                    RadikoEmail = "test@test.com",
                    Key = $"{Guid.NewGuid():N}"
                };
                await _model.ConfigModel.Update(config);
                var config2 = await _model.ConfigModel.Get();
                
                Assert.Equal(config.RadikoEmail, config2.RadikoEmail);
                
                
                File.Delete(_fileName);
            });
        }
    }
}