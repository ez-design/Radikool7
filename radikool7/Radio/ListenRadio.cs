using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Radikool7.Classes;
using Radikool7.Entities;

namespace Radikool7.Radio
{
    public class ListenRadio
    {
        public static Action<Exception> ErrorHandler { private get; set; }
        private static readonly HttpClient HttpClient = new HttpClient(new HttpClientHandler() {UseCookies = true});

        
        private class ListenRadioJson
        {
            public int Result { get; set; }
            public DateTime ServerTime { get; set; }
            public int Count { get; set; }
            public List<ListenRadioStation> Channel { get; set; }
            public List<ListenRadioCategory> Category { get; set; }
            public List<ListenRadioTimetable> ProgramSchedule { get; set; }
        }

        private class ListenRadioCategory
        {
            public string CategoryId { get; set; }
            public string CategoryName { get; set; }
        }

        private class ListenRadioStation
        {
            public string ChannelId { get; set; }
            public string ChannelName { get; set; }
            public string ChannelDetail { get; set; }
            public string ChannelImage { get; set; }
            public string ChannelLogo { get; set; }
            public string ChannelHls { get; set; }

        }

        private class ListenRadioTimetable
        {
            public string ProgramName { get; set; }
            public string ProgramSummary { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
        }

        /// <summary>
        /// 放送局取得
        /// </summary>
        /// <returns></returns>
        public static async Task<List<RadioStation>> GetStations()
        {

            var res = new List<RadioStation>();
            // カテゴリ取得
            var data = JsonConvert.DeserializeObject<ListenRadioJson>(
                await HttpClient.GetStringAsync(Define.ListenRadio.CategoryList));
            var categories = data?.Category.Where(x => x.CategoryId != "99999").ToList();

            if (categories == null) return null;

            var sequence = 1;
            foreach (var category in categories)
            {
                try
                {

                    data = JsonConvert.DeserializeObject<ListenRadioJson>(
                        HttpClient.GetStringAsync(Define.ListenRadio.StationList + category.CategoryId).Result);

                    var total = data.Channel.Count;

                    data.Channel.ForEach(c =>
                    {
                        if (!c.ChannelHls.StartsWith("http")) return;

                        res.Add(new RadioStation
                        {
                            Type = Define.ListenRadio.TypeName,
                            Name = c.ChannelName,
                            Id = $"{Define.ListenRadio.TypeName}_{c.ChannelId}",
                            Code = c.ChannelId,
                            Sequence = sequence++,
                            RegionId = "lr_" + category.CategoryId,
                            RegionName = category.CategoryName,
                            MediaUrl = c.ChannelHls
                        });


                    });

                }
                catch (Exception ex)
                {
                    ErrorHandler?.Invoke(ex);
                }
            }
            return res;

        }

        /// <summary>
        /// 番組表取得
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        public static async Task<List<RadioProgram>> GetPrograms(RadioStation station)
        {
            var res = new List<RadioProgram>();


            try
            {
                using (var client = new HttpClient())
                {
                    var data = JsonConvert.DeserializeObject<ListenRadioJson>(await HttpClient
                        .GetStringAsync(Define.ListenRadio.Timetable + station.Code));

                    foreach (var program in data.ProgramSchedule)
                    {
                        res.Add(new RadioProgram
                        {
                            Id = station.Id + "_" + Utility.Text.StringToDate(program.StartDate + "00")
                                     .ToString("yyyyMMddHHmmss"),
                            StationId = station.Id,
                            Title = program.ProgramName,
                            Description = program.ProgramSummary,
                            Start = Utility.Text.StringToDate(program.StartDate + "00"),
                            End = Utility.Text.StringToDate(program.EndDate + "00"),
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorHandler?.Invoke(ex);
            }

            return res;


        }
    }
}