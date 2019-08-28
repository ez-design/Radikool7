using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Radikool7.Classes;
using Radikool7.Entities;

namespace Radikool7.Radio
{
    public static class Radiko
    {
        public static Action<Exception> ErrorHandler { private get; set; }
        private static readonly HttpClient HttpClient = new HttpClient(new HttpClientHandler() {UseCookies = true});

        /// <summary>
        /// ログイン
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public static async Task<bool> Login(string email, string pass)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pass)) return false;

            var result = false;

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"mail", email},
                {"pass", pass}
            });

            var res = await HttpClient.PostAsync(Define.Radiko.Login, content);
            await res.Content.ReadAsStringAsync();

            result = await LoginCheck();
            return result;
        }

        /// <summary>
        /// ログインチェック
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> LoginCheck()
        {
            bool result;
            var response = await HttpClient.GetAsync(Define.Radiko.LoginCheck);
            var json = await response.Content.ReadAsStringAsync();
            try
            {
                var loginResult = JsonConvert.DeserializeObject<RadikoLoginCheckResult>(json);
                result = !string.IsNullOrWhiteSpace(loginResult.UserKey);
            }
            catch (Exception e)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 都道府県取得
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetPrefId()
        {
            var prefId = "";
            var text = await HttpClient.GetStringAsync(Define.Radiko.AreaCheck);
            var m = Regex.Match(text, @"JP[0-9]+");
            if (m.Success)
            {
                prefId = m.Value;
            }

            return prefId;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task<List<RadioStation>> GetStations()
        {
            var xmlUrl = "";
            if (await LoginCheck())
            {
                xmlUrl = Define.Radiko.StationListFull;
            }
            else
            {
                var prefId = await GetPrefId();
                xmlUrl = Define.Radiko.StationListPref.Replace("[AREA]", prefId);
            }


            var xml = await HttpClient.GetStringAsync(xmlUrl);
            var result = new List<RadioStation>();


            try
            {
                var doc = XDocument.Parse(xml);


                // 放送局一覧
                var sequence = 1;
                foreach (var stations in doc.Descendants("stations"))
                {
                    var regionId = stations.Attribute("region_id")?.Value ?? "";
                    var regionName = stations.Attribute("region_name")?.Value ?? "";
                    foreach (var station in stations.Descendants("station"))
                    {
                        var code = station.Descendants("id").First().Value;
                        var name = station.Descendants("name").First().Value;
                        var logo = station.Descendants("logo").FirstOrDefault()?.Value ?? "";
                        var areaId = station.Descendants("area_id").FirstOrDefault()?.Value ?? "";
                        var url = station.Descendants("href").First().Value;
                        result.Add(new RadioStation
                        {
                            Id = $"{Define.Radiko.TypeName}_{code}",
                            RegionId = regionId,
                            RegionName = regionName,
                            Code = code,
                            Name = name,
                            AreaId = areaId,
                            Type = Define.Radiko.TypeName,
                            Sequence = sequence++
                        });
                    }
                }


            }
            catch (Exception ex)
            {
                ErrorHandler?.Invoke(ex);
            }

            return result;


        }
    }
}