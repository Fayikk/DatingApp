using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _token;
        private readonly IMapper _mapper;
        public AccountController(DataContext context , ITokenService token,IMapper mapper)
        {
            _context = context;
            _token = token;
            _mapper = mapper;
        }    

        [HttpPost("register")] //api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto){

       
            if(await UserExist(registerDto.Username)) return BadRequest("Username is taken");
    
           var user = _mapper.Map<AppUser>(registerDto);
    
            using var hmac = new HMACSHA512();
         
          
            user.UserName = registerDto.Username.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;
          
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
           
           return new UserDto{
            Username = user.UserName,
            Token = _token.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(x=>x.IsMain)?.Url,
            KnownAs = user.KnownAs,
            Gender = user.Gender
           };
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto){
            var user = await _context.Users
            .Include(x=> x.Photos) //For navbar image
            .SingleOrDefaultAsync(x =>  x.UserName == loginDto.Username);
   
            if(user == null)    return Unauthorized("Invalid Username");


            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if(computedHash[i]!=user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }
            return new UserDto{
                Username = user.UserName,
                Token = _token.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x=>x.IsMain)?.Url,
                Gender = user.Gender
            };
       
        }


        public async Task<bool> UserExist(string username){

            return await _context.Users.AnyAsync(x=>x.UserName==username.ToLower());
        }

    
    }
}