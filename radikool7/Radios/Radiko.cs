using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Radikool7.Classes;
using Radikool7.Entities;

namespace Radikool7.Radios
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
        ///  放送局取得
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
                            StationUrl = url,
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

        /// <summary>
        /// 番組情報取得
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        public static async Task<List<RadioProgram>> GetPrograms(RadioStation station)
        {
            var xml = "";
            var url = Define.Radiko.WeeklyTimeTable.Replace("[stationCode]", station.Code);
            try
            {
                xml = await HttpClient.GetStringAsync(url);
            }
            catch (Exception ex)
            {
                ErrorHandler?.Invoke(new Exception($"{url}"));
                return new List<RadioProgram>();
            }

            var doc = XDocument.Parse(xml);

            return doc.Descendants("prog")
                .Select(prog => new RadioProgram()
                {
                    Id = station.Code + prog.Attribute("ft")?.Value + prog.Attribute("to")?.Value,
                    Start = Utility.Text.StringToDate(prog.Attribute("ft")?.Value),
                    End = Utility.Text.StringToDate(prog.Attribute("to")?.Value),
                    Title = prog.Element("title")?.Value.Trim(),
                    Cast = prog.Element("pfm")?.Value.Trim(),
                    Description = prog.Element("info")?.Value.Trim(),
                    StationId = station.Id,
                    TsNg = prog.Element("ts_in_ng")?.Value.Trim()
                })
                .ToList();

        }

        /// <summary>
        /// トークン取得
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetAuthToken()
        {

            // auth token取得
            var request = new HttpRequestMessage(HttpMethod.Get, Define.Radiko.Auth1);
            request.Headers.Add("pragma", "no-cache");
            request.Headers.Add("x-radiko-app", "pc_html5");
            request.Headers.Add("x-radiko-app-version", "0.0.1");
            request.Headers.Add("x-radiko-device", "pc");
            request.Headers.Add("x-radiko-user", "dummy_user");
            
            var res = await HttpClient.SendAsync(request);
            var token = res.Headers.GetValues("X-Radiko-AuthToken").FirstOrDefault();
            int.TryParse(res.Headers.GetValues("X-Radiko-KeyLength").FirstOrDefault(), out var keyLength);
            int.TryParse(res.Headers.GetValues("X-Radiko-KeyOffset").FirstOrDefault(), out var keyOffset);

            // partial keyの元を取得
            var js = await HttpClient.GetStringAsync(Define.Radiko.CommonJs);

            var m = Regex.Match(js, @"new RadikoJSPlayer.*{");
            var key = "";
            if (m.Success)
            {
                key = m.Value.Split(',')[2].Replace("'", "").Trim();
            }

            var partialKey =
                Convert.ToBase64String(Encoding.UTF8.GetBytes(key.Substring(keyOffset, keyLength)));

            // auto tokenを有効可
            request = new HttpRequestMessage(HttpMethod.Get, Define.Radiko.Auth2);
            request.Headers.Add("x-radiko-authtoken", token);
            request.Headers.Add("x-radiko-device", "pc");
            request.Headers.Add("x-radiko-partialkey", partialKey);
            request.Headers.Add("x-radiko-user", "dummy_user");

            res = await HttpClient.SendAsync(request);

            return token;
        }

        /// <summary>
        /// タイムフリーのm3u8取得
        /// </summary>
        /// <param name="program"></param>
        /// <returns></returns>
        public static async Task<string> GetTimeFreeM3U8(RadioProgram program)
        {
            var m3U8 = "";
            var token = await GetAuthToken();
            var url = Define.Radiko.TimeFreeM3U8.Replace("[CH]", program.RadioStation.Code)
                .Replace("[FT]", program.Start.ToString("yyyyMMddHHmmss"))
                .Replace("[TO]", program.End.ToString("yyyyMMddHHmmss"));

            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Headers.Add("pragma", "no-cache");
            request.Headers.Add("X-Radiko-AuthToken", token);
            var res = await HttpClient.SendAsync(request);
            var text = await res.Content.ReadAsStringAsync();

            foreach (var line in text.Split('\n'))
            {
                if (!line.Contains("http")) continue;
                m3U8 = line;
                break;
            }
            return m3U8;

        }

    }


}