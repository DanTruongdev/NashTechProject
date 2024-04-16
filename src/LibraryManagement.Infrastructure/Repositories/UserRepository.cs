using LibraryManagement.Core.Entities;
using LibraryManagement.Infrastructure.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
using System.Text;
using System.Collections.Immutable;
using LibraryManagement.Infrastructure.Data;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _dbContext;


        public UserRepository(UserManager<User> userManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }
        public async Task<User> FindUserByEmailAsync(string email)
        {
            string upperEmail = email.ToUpper();
            User userExist = await _userManager.Users.FirstOrDefaultAsync(u => u.Email.ToUpper().Equals(upperEmail));
            if (userExist == null) return null;
            return userExist;
        }

        public async Task<User> CheckUserLoginAsync(string email, string password)
        {
            var userExist = await FindUserByEmailAsync(email);
            var passwordChecker = await _userManager.CheckPasswordAsync(userExist, password);
            if (userExist == null || !passwordChecker) return null;
            if (!userExist.IsActivated) return null;
            return userExist;
        }

        public async Task<User> CreateWithRoleAsync(User newUser,string password, string roleName)
        {
            var addUserResult = await _userManager.CreateAsync(newUser, password);
            if (!addUserResult.Succeeded) return newUser;
            var addUserRoleResult = await _userManager.AddToRoleAsync(newUser, roleName);
            return null;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = await _userManager.Users
                .OrderByDescending(u => u.CreationDate)
                .AsNoTracking().ToListAsync();
            return users;
        }

        public async Task<IEnumerable<User>> SearchUserByName(string searchString)
        {
            
            var users = await _userManager.Users
                .Where(u => u.FirstName.Contains(searchString) || u.LastName.Contains(searchString))
                .OrderByDescending(u => u.CreationDate)
                .AsNoTracking()
                .ToListAsync();
            return users;
        }

        public async Task<bool> ChangeUserStatus(string userId)
        {
            try
            {
                User userExist = await _userManager.FindByIdAsync(userId);
                if (userExist == null) return false;
                userExist.IsActivated = !userExist.IsActivated;
                _dbContext.Update(userExist);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }    
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            try{
                _dbContext.Update(user);
                await _dbContext.SaveChangesAsync();
                return user;
            }
            catch
            {
                return null;
            }

        }
    }
}
