using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Radikool7.Entities;

namespace Radikool7.Models
{
    public class RadioProgramModel : BaseModel
    {
        public static string FileName { get; set; } = "timetable.json";

        /// <summary>
        /// 番組検索
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public async Task<List<RadioProgram>> Search(SearchCondition condition)
        {
            var data = await Load();
            var q = data.AsEnumerable();
            if (condition.StationIds.Any())
            {
                q = q.Where(x => condition.StationIds.Contains(x.StationId));
            }

            if (condition.From != null)
            {
                q = q.Where(x => x.End > condition.From);
            }

            if (condition.To != null)
            {
                q = q.Where(x => condition.To > x.Start);
            }

            return q.ToList();
        }
        
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="radioPrograms"></param>
        public async void Update(List<RadioProgram> radioPrograms)
        {
            var stationIds = radioPrograms.Select(x => x.StationId).Distinct().ToList();
            var orgData = await Load();
            orgData = orgData.Where(x => !stationIds.Contains(x.StationId)).ToList();
            var data = orgData.Concat(radioPrograms).ToList();
            await Write(FileName, data);
        }

        /// <summary>
        /// ファイル読み込み
        /// </summary>
        /// <returns></returns>
        public async Task<List<RadioProgram>> Load()
        {
            return await Read<List<RadioProgram>>(FileName) ?? new List<RadioProgram>();
        }
        
        
    }
}