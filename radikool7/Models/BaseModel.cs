using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Radikool7.Models
{
    public class BaseModel
    {
        /// <summary>
        /// ファイルから読み込み
        /// </summary>
        /// <param name="fileName"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected static Task<T> Read<T>(string fileName)
        {
            return Task.Factory.StartNew(() =>
            {
                var res = default(T);
                if (!File.Exists(fileName))
                {
                    return res;
                }

                var json = File.ReadAllText(fileName);
                try
                {
                    res = JsonConvert.DeserializeObject<T>(json);
                }
                catch
                {
                    // ignored
                }

                return res;
            });

        }

        /// <summary>
        /// ファイルに書き込み
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        protected static Task Write(string fileName, object data)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var json = JsonConvert.SerializeObject(data);
                    File.WriteAllText(fileName, json);
                }
                catch
                {
                    // ignored
                }
            });

        }
    }
}