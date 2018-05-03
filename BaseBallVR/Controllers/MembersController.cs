using System;
using Microsoft.AspNetCore.Mvc;
using BaseBallVR.DB;
using BaseBallVR.Models;
using System.Collections.Generic;
using AutoMapper;
using BaseBallVR.Redis;
using BaseBallVR.Redis.Models;
using StackExchange.Redis.Extensions.Core;

namespace BaseBallVR.Controllers
{
    [Route("api/users/{userId}/members")]
    public class MembersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly ICacheClient _logcache;

        public MembersController(IUserRepository userRepository, IMemberRepository memberRepository, ICacheRedis cacheredis)
        {
            _userRepository = userRepository;
            _memberRepository = memberRepository;
            _logcache = cacheredis.GetCacheClient();
        }
        // 특정 계정의 모든 팀원 조회
        [HttpGet]
        public IActionResult Get(string userId)
        {
            try
            {
                var userDB = _userRepository.GetSingle(userId);
                if (userDB == null)
                    return NotFound("NotFound");

                var memberDB = _memberRepository.FindBy(a=>a.UserId==userId);

              
                IEnumerable<MemberViewModel> member = Mapper.Map< IEnumerable<Member>, IEnumerable< MemberViewModel> >(memberDB);
             
                return new OkObjectResult(member);
            }

            catch(Exception ex)
            {
                return NotFound(ex);
            }
        }


        [HttpGet("{memberId}")]
        public IActionResult Get(string userId, string memberId)
        {
            try
            {
                var member = _memberRepository.GetSingle(memberId);
                if (member == null)
                    return NotFound("NotFound");

                MemberViewModel memberVM = Mapper.Map<Member, MemberViewModel>(member);

                return new OkObjectResult(memberVM);
            }

            catch(Exception ex)
            {
                return NotFound(ex);
            }
        }
        // 팀원 생성
        [HttpPost]
        public IActionResult Create(string userId, [FromBody]MemberViewModel member)
        {
            try
            {
                var userDB = _userRepository.GetSingle(userId);
                if (userDB == null)
                    return NotFound("NotFound");
                var memberDB = _memberRepository.GetSingle(member.Id);
                if (memberDB != null)
                    return NotFound("중복");

                var newMember = new Member
                {
                    Id = member.Id,
                    UserId = userId,
                    reinforce = member.reinforce
                };

                
                _memberRepository.Add(newMember);
                _memberRepository.Commit();
                _userRepository.Commit();

                MemberViewModel memberVM = Mapper.Map<Member, MemberViewModel>(newMember);
                
                return new OkObjectResult(memberVM);
            }

            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }

        // 팀원 강화
        [HttpPut("{memberId}/{num}")]
        public IActionResult Put(string userId, string memberId, int num)
        {
            try
            {
                var userDB = _userRepository.GetSingle(userId);
                if (userDB == null)
                    return NotFound("Not Found");
                var memberDB = _memberRepository.GetSingle(memberId);
                if (memberDB == null)
                    return NotFound("Not Found");

                memberDB.reinforce += num;

                _memberRepository.Update(memberDB);
                _memberRepository.Commit();

                string date = DateTime.UtcNow.ToString("yyyy-MM-dd");

                _logcache.ListAddToLeft<LogData>(date, new LogData
                {
                    UserId = userId,
                    MemberId = memberId,
                    Date = DateTime.UtcNow.ToString(),
                    Num = num,
                    Reinforce = memberDB.reinforce
                });

                MemberViewModel memberVM = Mapper.Map<Member, MemberViewModel>(memberDB);
                return new OkObjectResult(memberVM);
            }
            catch(Exception ex){
                return NotFound(ex);
            }
        }


        // 팀원 삭제.
        [HttpDelete("{memberId}")]
        public IActionResult Delete(string userId, string memberId)
        {
            try
            {
                var userDB = _userRepository.GetSingle(userId);
                if (userDB == null)
                    return NotFound("Not Found");
                var memberDB = _memberRepository.GetSingle(memberId);
                if (memberDB == null)
                    return NotFound("Not Found");

                _memberRepository.Delete(memberDB);
                _memberRepository.Commit();

                return new NoContentResult();

            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }
    }
}