using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]  //api/users
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
            //Dependency Injection
        }

        [HttpGet]
        public async Task <ActionResult<IEnumerable<AppUser>>> GetUsers(){
            var users =await _context.Users.ToListAsync();
            return users;
            //Aync Metod
        }

        [HttpGet("{id}")]
        public  async Task<ActionResult<AppUser>> GetUser(int id){
            return await _context.Users.FindAsync(id);
            
        }

    }
}