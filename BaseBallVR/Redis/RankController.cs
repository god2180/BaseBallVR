using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using BaseBallVR.Redis.Models;
using BaseBallVR.DB;

using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Newtonsoft;
using AutoMapper;

namespace BaseBallVR.Redis
{
    [Route("api/rank")]
    public class RankController : Controller
    {

        private readonly IUserRepository _userRepository;
        private readonly ICacheClient _leaderboard;

        public RankController(IUserRepository userRepository, ICacheRedis cacheredis)
        {
            _leaderboard = cacheredis.GetCacheClient();
            _userRepository = userRepository;
            
        }

        [HttpGet("ranking/{date}")]
        public IActionResult Get(string date)
        {
            try
            {
                var db = _leaderboard.SortedSetRangeByScore<LeaderboardData>(date + " Ranking", order:Order.Descending);
                int ran = 1;
                IList<RankingData> rDB = new List<RankingData>();

                foreach (var d in db)
                {
                    RankingData ranking = new RankingData
                    {
                        Rank = ran++,
                        LeaderboardData = d
                        
                    };
                    
                    rDB.Add(ranking);
                }

                return new OkObjectResult(rDB);
            }
            catch(Exception ex)
            {
                return NotFound(ex);
            }
        }

        [HttpGet("list/{date}")]
        public IActionResult GetList(string date)
        {
            try
            {
                var list = _leaderboard.HashGetAll <string>(date + " RankList");
                return new OkObjectResult(list);
            }
            catch(Exception ex)
            {
                return NotFound(ex);
            }
        }
        
        [HttpPost]
        public void Post()
        {
            try
            {
                var userDB = _userRepository.FindBy(u => u.MMR > 0);
                string today = DateTime.UtcNow.ToString("yyyy-MM-dd");
                foreach(var rd in userDB)
                {
                    var leaderboardDB = Mapper.Map<BaseBallVR.Models.User, LeaderboardData>(rd);
                    _leaderboard.SortedSetAdd(today+" Ranking", leaderboardDB, rd.MMR);
                    _leaderboard.HashSet(today + " RankList", leaderboardDB.Id, leaderboardDB.TeamName);
                }
            }
        catch(Exception ex)
            {
                NotFound(ex);
                return;
            }
        }

        
    }
}
