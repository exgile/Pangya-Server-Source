namespace PangyaAPI.PangyaClient.Data
{
    public class UserInfo
    {
        #region Field
        public uint GetUID { get; set; }
        public byte GetFirstLogin { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string GetLogin = string.Empty;
        public string GetPass = string.Empty;
        public string GetNickname = string.Empty;
        public byte GetSex { get; set; }
        public byte GetCapability { get; set; } = 0;
        public byte GetLevel { get { return UserStatistic.Level; } set { UserStatistic.Level = value; } }
        public uint GetPang { get { return (uint)UserStatistic.Pang; } }
        public uint GetExpPoint { get { return UserStatistic.EXP; } }
        public string GetAuth1 { get; set; } = string.Empty;
        public string GetAuth2 { get; set; } = string.Empty;
        public StatisticInfo UserStatistic;
        public uint Visible { get;  set; }
        public uint GetCookie { get;  set; }

        #endregion

        #region Constructor

        public UserInfo()
        {
            UserStatistic = new StatisticInfo();
        }

        #endregion
    }
}
