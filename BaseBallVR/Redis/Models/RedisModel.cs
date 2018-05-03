using System.Collections.Generic;

namespace BaseBallVR.Redis.Models
{
    public class LogData // 선수 로그
    {
        public string Date { get; set; }
        public string UserId { get; set; }
        public string MemberId { get; set; }        
        public int Num { get; set; }
        public int Reinforce { get; set; }
    }
    

    public class LeaderboardData
    {
        public string Id { get; set; }   
        public string TeamName { get; set; }
    }

    
    public class RankingData
    {
        public int Rank { get; set; }
        public LeaderboardData LeaderboardData { get; set; }
    }
}