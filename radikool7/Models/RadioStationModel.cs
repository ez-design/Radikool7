using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Radikool7.Classes;
using Radikool7.Entities;
using Radikool7.Radios;

namespace Radikool7.Models
{
    public class RadioStationModel : BaseModel
    {
        public static string FileName { get; set; } = "station.json";
        
        public async Task<List<RadioStation>> Get(string type)
        {
            List<RadioStation> res;
            var radioStations = await Load();
            if ((res = radioStations.Where(x => x.Type == type).ToList()).Any())
            {
                return res;
            }
            
            switch (type)
            {
                case Define.Radiko.TypeName:
                    res = await Radiko.GetStations();
                    break;
                case Define.Nhk.TypeName:
                    res = await Nhk.GetStations();
                    break;
                case Define.ListenRadio.TypeName:
                    res = await ListenRadio.GetStations();
                    break;
            }

            radioStations = radioStations.Concat(res).ToList();
            await Write(FileName, radioStations);

            return res;
        }
        
        /// <summary>
        /// ファイル読み込み
        /// </summary>
        /// <returns></returns>
        public async Task<List<RadioStation>> Load()
        {
            return await Read<List<RadioStation>>(FileName) ?? new List<RadioStation>();
        }
    }
}