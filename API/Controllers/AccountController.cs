using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _token;
        public AccountController(DataContext context , ITokenService token)
        {
            _context = context;
            _token = token;
        }    

        [HttpPost("register")] //api/account/register
        public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto){
            
            if(await UserExist(registerDto.Username)) return BadRequest("Username is taken");
            
            using var hmac = new HMACSHA512();
            var user = new AppUser{
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
           
            return user;
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody]LoginDto loginDto){
            var user = await _context.Users.SingleOrDefaultAsync(x => 
            x.UserName == loginDto.Username);
   
            if (user == null)return Unauthorized("Invalid Username");


            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if(computedHash[i]!=user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }
            return new UserDto{
                Username = user.UserName,
                Token = _token.CreateToken(user)
            };
       
        }


        public async Task<bool> UserExist(string username){

            return await _context.Users.AnyAsync(x=>x.UserName==username.ToLower());
        }

    
    }
}