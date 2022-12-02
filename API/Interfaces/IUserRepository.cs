using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRepository  //SOLID ı = İNTERAFCE SEGRAGATİON = 
    {
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);     
        Task<PagedList<MemberDto>> GetMemberAsync(UserParams userParams);   
        Task<MemberDto> GetMemberAsync(string username);
    }
}