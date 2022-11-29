using API.DTOs;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // [ApiController] 
    // [Route("api/[controller]")]  //api/users
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UsersController(IUserRepository userRepository ,IMapper mapper)
        {
            _userRepository = userRepository;  
            _mapper = mapper;
          
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task <ActionResult<IEnumerable<MemberDto>>> GetUsers(){
            var users = await _userRepository.GetMemberAsync();
            
            return Ok(users);
            //DateOnly deneyelim
            //Async Metod
        }
        [Authorize]
        [HttpGet("{username}")]
        public  async Task<ActionResult<MemberDto>> GetUser(string username){
            
            return await _userRepository.GetMemberAsync(username);
        }
    }
}