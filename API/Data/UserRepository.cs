using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {

        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {   
            this._context = context;
            this._mapper = mapper;
        }
 
        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await this._context.Users
                            .Include(p => p.Photos)
                            .ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await this._context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await this._context.Users
                            .Include(p => p.Photos)
                            .SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<bool> SaveAllAsync(AppUser user)
        {
            return await this._context.SaveChangesAsync() > 0;
        }

        public Task<bool> SaveAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public void Update(AppUser user)
        {
            this._context.Entry(user).State = EntityState.Modified;
        }

        public async Task<MemberDTO> GetMemberAsync(string username)
        {
            return await this._context.Users
                    .Where(x => x.UserName == username)
                    .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<MemberDTO>> GetMembersAsync()
        {
            return await this._context.Users
                    .ProjectTo<MemberDTO>(this._mapper.ConfigurationProvider)
                    .ToListAsync();
        }
        
    }
}