using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Radikool7.Classes;
using Radikool7.Entities;
using Radikool7.Models;
using Radikool7.Radios;
using Radikool7.ViewModels;
using Xunit;
using Xunit.Abstractions;

namespace radikool7.Test.ModelsTest
{
    public class RadioProgramModelTest : BaseTest
    {
        private readonly MainModel _model;
        public RadioProgramModelTest(ITestOutputHelper output) : base(output)
        {
            _model = new MainModel();
            Radiko.ErrorHandler = (e) =>
            {
                Output.WriteLine(e.Message);
            };
        }

        [Fact]
        public void LoadTest()
        {
            Execute("LoadTest", async () =>
            {
                var programs = await _model.RadioProgramModel.Load();
                Output.WriteLine($"{programs.Count}件");
            });
        }

        [Fact]
        public void UpdateTest()
        {
            Execute("UpdateTest", async () =>
            {
                await Radiko.Login(TestSetting.RadikoEmail, TestSetting.RadikoPassword);
                var stations = await Radiko.GetStations();
                var programs = new List<RadioProgram>();

                var sw = new Stopwatch();
                sw.Start();
                foreach (var station in stations)
                {
                    programs.AddRange(await Radiko.GetPrograms(station));
                }
                sw.Stop();
                Output.WriteLine($"{programs.Count}件：{sw.Elapsed}");
                Assert.NotEmpty(programs);
            
                sw.Restart();
                _model.RadioProgramModel.Update(programs);
                sw.Stop();
                Output.WriteLine($"ファイル書き込み：{sw.Elapsed}");
            });
            
        }

        [Fact]
        public void SearchTest()
        {
            Execute("SearchTest", async () =>
            {
                var station = (await Radiko.GetStations()).First();
                var condition = new SearchCondition()
                {
                    StationIds = new List<string>() {station.Id}
                };
                var programs = await _model.RadioProgramModel.Search(condition);
                Assert.NotEmpty(programs);

                var program = programs.First();
                condition.From = program.Start;
                condition.To = program.End;
                programs = await _model.RadioProgramModel.Search(condition);
            
                Assert.Equal(1, programs.Count);
                Assert.Equal(program.Id, programs.First().Id);
            });
            
        }
    }
}