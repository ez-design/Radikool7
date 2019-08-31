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
        
        public async Task<IEnumerable<RadioStation>> Get(string type)
        {
            IEnumerable<RadioStation> res;
            var radioStations = Read<IEnumerable<RadioStation>>(FileName) ?? new RadioStation[]{};
            if ((res = radioStations?.Where(x => x.Type == type)).Any())
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

            radioStations = radioStations.Concat(res);
            Write(FileName, radioStations);

            return res;
        }
    }
}