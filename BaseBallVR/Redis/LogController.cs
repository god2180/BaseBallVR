using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BaseBallVR.Redis;
using BaseBallVR.Redis.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis.Extensions.Core;


namespace BaseBallVR.Controllers
{
    [Route("api/logs")]
    public class LogController : Controller
    {
        private readonly ICacheClient _logcache;

        public LogController(ICacheRedis cacheredis)
        {
            _logcache = cacheredis.GetCacheClient();
        }

        [HttpGet("{date}")]
        public IActionResult Get(string date)
        {
            try
            {
                IList<LogData> logs = new List<LogData>();

                for(int i = 0; i<20; i++)
                {
                    var data = _logcache.ListGetFromRight<LogData>(date);
                    if (data == null)
                        break;
                    logs.Add(data);
                }

                return new OkObjectResult(logs); 
            }

            catch(Exception ex)
            {
                return NotFound(ex);
            }
        }
    }
}