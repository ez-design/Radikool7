namespace Radikool7.Classes
{
    public static class Define
    {
        public static class Radiko
        {
            /// <summary>
            /// 種別
            /// </summary>
            public const string TypeName = "radiko";

            /// <summary>
            /// ログインURL
            /// </summary>
            public const string Login = "https://radiko.jp/ap/member/login/login";
            
            /// <summary>
            /// ログインチェック
            /// </summary>
            public const string LoginCheck = "https://radiko.jp/ap/member/webapi/member/login/check";
            
            /// <summary>
            /// 地域判定用
            /// </summary>
            public const string AreaCheck = "http://radiko.jp/area/";
            
            /// <summary>
            /// 放送局一覧(すべて)
            /// </summary>
            public const string StationListFull = "http://radiko.jp/v3/station/region/full.xml";
            
            /// <summary>
            /// 放送局一覧(都道府県ごと)
            /// </summary>
            public const string StationListPref = "http://radiko.jp/v3/station/list/[AREA].xml";
            
            /// <summary>
            /// 週刊番組表
            /// </summary>
            public const string WeeklyTimeTable = "http://radiko.jp/v3/program/station/weekly/[stationCode].xml";
            
            /// <summary>
            /// 認証URL1
            /// </summary>
            public const string Auth1 = "https://radiko.jp/v2/api/auth1";

            /// <summary>
            /// 認証URL2
            /// </summary>
            public const string Auth2 = "https://radiko.jp/v2/api/auth2";


            /// <summary>
            /// common.js
            /// </summary>
            public const string CommonJs = "http://radiko.jp/apps/js/playerCommon.js";
            
            /// <summary>
            /// タイムフリー
            /// </summary>
            public const string TimeFreeM3U8 = "https://radiko.jp/v2/api/ts/playlist.m3u8?station_id=[CH]&ft=[FT]&to=[TO]";

        }
    }
}