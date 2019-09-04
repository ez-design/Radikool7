using System.Threading.Tasks;
using Radikool7.Classes;
using Radikool7.Entities;

namespace Radikool7.Models
{
    public class ConfigModel : BaseModel
    {
        public static string FileName { get; set; } = "config.json";

        /// <summary>
        /// 読み込み
        /// </summary>
        /// <returns></returns>
        public async Task<Config> Get()
        {
            var res = await Read<Config>(FileName) ?? new Config();
            if (string.IsNullOrWhiteSpace(res.Key)) return res;
            res.RadikoEmail = Utility.Text.Decrypt(res.RadikoEmail, res.Key);
            res.RadikoPassword = Utility.Text.Decrypt(res.RadikoPassword, res.Key);
            return res;
        }


        /// <summary>
        /// 書き込み
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public async Task Update(Config config)
        {
            var data = config.Copy();
            data.RadikoEmail = Utility.Text.Encrypt(data.RadikoEmail, data.Key);
            data.RadikoPassword = Utility.Text.Encrypt(data.RadikoPassword, data.Key);
            await Write(FileName, data);
        }
    }
}