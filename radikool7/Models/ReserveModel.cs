using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Radikool7.Entities;

namespace Radikool7.Models
{
    public class ReserveModel : BaseModel
    {
        public static string FileName { get; set; } = "reserve.json";

        /// <summary>
        /// 読み込み
        /// </summary>
        /// <returns></returns>
        public async Task<List<Reserve>> Get()
        {
            var res = await Read<List<Reserve>>(FileName);
            return res ?? new List<Reserve>();
        }

        /// <summary>
        /// 予約タスク取得
        /// </summary>
        /// <returns></returns>
        public async Task<List<ReserveTask>> GetTasks(Config config)
        {
            var res = new List<ReserveTask>();
            var reserves = await Get();
            foreach (var reserve in reserves)
            {
                var days = new List<DateTime[]>();
                if (reserve.Repeat.Count == 0)
                {
                    // 単発
                    days.Add(new[]{ reserve.Start, reserve.End});
                }
                else
                {
                    // 曜日指定
                    var date = DateTime.Now;
                    for (var i = 0; i < 7; i++)
                    {
                        if (reserve.Repeat.Contains((int) date.DayOfWeek))
                        {
                            var start = date.Date.AddHours(reserve.Start.Hour).AddMinutes(reserve.Start.Minute)
                                .AddSeconds(reserve.Start.Second);
                            var end = date.Date.AddHours(reserve.End.Hour).AddMinutes(reserve.End.Minute).AddSeconds(reserve.End.Second);
                            if (end < start)
                            {
                                end = end.AddDays(1);
                            }

                            days.Add(new[] {start, end});
                        }

                        date = date.AddDays(1);
                    }
                }
                
                foreach(var day in days)
                {
                    if (reserve.IsTimeFree)
                    {
                        // タイムフリー
                        var task = new ReserveTask()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Start = day[1].AddMinutes(config.TimeFreeMargin),
                            End = day[1].AddMinutes(config.TimeFreeMargin + 10),
                            ProgramStart = day[0],
                            ProgramEnd = day[1],
                            ReserveId = reserve.Id
                        };
                        res.Add(task);
                    
                    }
                    else
                    {
                        // リアルタイム
                        var task = new ReserveTask()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Start = day[0],
                            End = day[1],
                            ReserveId = reserve.Id,
                            ProgramStart = day[0],
                            ProgramEnd = day[1]
                        };
                        res.Add(task);
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// 書き込み
        /// </summary>
        /// <param name="data"></param>
        public async Task Update(Reserve data)
        {
            var reserves = await Get();
            reserves = reserves.Where(x => x.Id != data.Id).ToList();
            reserves.Add(data);
            await Write(FileName, reserves);
        }
    }
}