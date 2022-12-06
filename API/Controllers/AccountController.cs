using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _token;
        private readonly IMapper _mapper;

        private readonly UserManager<AppUser> _userManager;
        public AccountController(UserManager<AppUser> userManager , ITokenService token,IMapper mapper)
        {
            _token = token;
            _mapper = mapper;
            _userManager = userManager;
        }    

        [HttpPost("register")] //api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto){

       
            if(await UserExist(registerDto.Username)) return BadRequest("Username is taken");
    
           var user = _mapper.Map<AppUser>(registerDto);
    
         
          
            user.UserName = registerDto.Username.ToLower();
           
            var result = await _userManager.CreateAsync(user , registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
           
            var roleResult = await _userManager.AddToRoleAsync(user,"Member");
            if (!roleResult.Succeeded)
            {
                return BadRequest(result.Errors);
            }

           return new UserDto{
            Username = user.UserName,
            Token =await _token.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(x=>x.IsMain)?.Url,
            KnownAs = user.KnownAs,
            Gender = user.Gender
           };
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto){
            var user = await _userManager.Users
            .Include(x=> x.Photos) //For navbar image
            .SingleOrDefaultAsync(x =>  x.UserName == loginDto.Username);
   
            if(user == null)    return Unauthorized("Invalid Username");

            var result = await _userManager.CheckPasswordAsync(user , loginDto.Password);

            if (!result)
            {
                return Unauthorized("Invalid password");
            }


            return new UserDto{
                Username = user.UserName,
                Token =await _token.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x=>x.IsMain)?.Url,
                Gender = user.Gender
            };
       
        }


        public async Task<bool> UserExist(string username){

            return await _userManager.Users.AnyAsync(x=>x.UserName==username.ToLower());
        }

    
    }
}