namespace Radikool7.Entities
{
    public class Config
    {
        /// <summary>
        /// radikoプレミアムメールアドレス
        /// </summary>
        public string RadikoEmail { get; set; } = "";

        /// <summary>
        /// radikoプレミアムパスワード
        /// </summary>
        public string RadikoPassword { get; set; } = "";
        
        public int TimeFreeMargin { get; set; } = 30;

        public string Key { get; set; } = "";

        /// <summary>
        /// コピー
        /// </summary>
        /// <returns></returns>
        public Config Copy()
        {
            return new Config
            {
                RadikoEmail = RadikoEmail,
                RadikoPassword = RadikoEmail,
                TimeFreeMargin = TimeFreeMargin,
                Key = Key
            };
        }
    }
}