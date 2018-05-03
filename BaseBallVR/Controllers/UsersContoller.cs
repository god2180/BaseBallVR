using System;
using Microsoft.AspNetCore.Mvc;
using BaseBallVR.DB;
using BaseBallVR.Models;
using AutoMapper;
using System.Collections.Generic;




// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BaseBallVR.Controllers
{
    [Route("api/users/")]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemberRepository _memberRepository;
        

        public UsersController(IUserRepository userRepository, IMemberRepository memberRepository)
        {
            _userRepository = userRepository;
            _memberRepository = memberRepository;
           
        }
       
        //계정 생성
        [HttpPost]
        public IActionResult Create([FromBody]UserViewModel user)
        {
            try
            {
                var userDB = _userRepository.GetSingle(user.Id);//체크 
                if (userDB != null)
                    return NotFound("key id error");//중복 생성


                var newUser = new User
                {
                    Id = user.Id,
                    TeamName = user.TeamName,
                    MMR = user.MMR,
                };

                _userRepository.Add(newUser);
                _userRepository.Commit();
                

                 return new OkObjectResult(newUser);
            }

            catch(Exception ex)
            {
                return NotFound(ex);
            }
        }


        // 모든 계정 정보 조회
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var userDB = _userRepository.Allincluding(u => u.Members);
                if (userDB == null)
                {
                    return NotFound("Empty");
                }

                IEnumerable<UserViewModel> userVM =  Mapper.Map< IEnumerable<User>, IEnumerable<UserViewModel>>(userDB);
                
                return new OkObjectResult(userVM);
            }
            catch (Exception ex)
            {
                return NotFound(ex);
            }
        }

        // 특정 계정 정보 조회
        [HttpGet("{Id}",Name ="GetUserInfo")]
        public IActionResult Get(string Id)
        {
            try
            {
                var UserDB = _userRepository.GetSingle(u => u.Id == Id, u => u.Members);
                if (UserDB == null)
                    return NotFound("Not Found");

                //UpdateMemberList(UserDB);

                UserViewModel userVM = Mapper.Map<User, UserViewModel>(UserDB);

                

                return new OkObjectResult(userVM);
            }
            catch(Exception ex)
            {
                return NotFound(ex);
            }
        }
        
        // 팀명 변경
        [HttpPut("{Id}/teamname={newTeamName}")]
        public IActionResult Put(string Id, string newTeamName)
        {
            try
            {
                var userDB = _userRepository.GetSingle(u => u.Id == Id, u => u.Members);
                if (userDB == null)
                    return NotFound("NotFound");

                userDB.TeamName = newTeamName;
                _userRepository.Update(userDB);
                _userRepository.Commit();

                UserViewModel userVM = Mapper.Map<User, UserViewModel>(userDB);

                return new OkObjectResult(userVM);
            }

            catch (Exception ex)
            {
                return NotFound(ex);
            }

        }

        //점수 변경

        [HttpPut("{Id}/point={Point}")]
        public IActionResult Put(string Id, int Point)
        {
            try
            {
                var userDB = _userRepository.GetSingle(u => u.Id == Id, u => u.Members);
                if (userDB == null)
                    return NotFound("NotFound");

                userDB.MMR += Point;
                _userRepository.Update(userDB);
                _userRepository.Commit();

                
                UserViewModel userVM = Mapper.Map<User, UserViewModel>(userDB);

                return new OkObjectResult(userVM);
            }

            catch (Exception ex)
            {
                return NotFound(ex);
            }

        }

        // 계정 삭제
        [HttpDelete("{Id}", Name ="DeleteUser")]
        public IActionResult Delete(string Id)
        {
            try
            {
                var userDB = _userRepository.GetSingle(Id);
                if (userDB == null)
                    return NotFound("Not Found");

                _userRepository.Delete(userDB);
                _userRepository.Commit();

                return new NoContentResult();
            }

            catch(Exception ex)
            {
                return NotFound(ex);
            }

        }
        /*
        private void UpdateMemberList(User User)
        {
            try
            {
                var memberDB = _memberRepository.FindBy(u => u.UserId == User.Id);
              
                if (memberDB == null)
                    return;

                foreach (Member member in memberDB)
                {
                //    //User.Members.Add(member);
                }
            }
            catch(Exception ex)
            {
                return;
            }
        }

        private void UpdateMemberList(IEnumerable<User> Users)
        {
            try
            {
                
                foreach( User user in Users)
                {
                    //var memberDB = _memberRepository.FindBy(u => u.UserId == user.Id);
                    UpdateMemberList(user);
                }
            }
            catch(Exception ex)
            {
                return;
            }
        }

    */
    }
}
