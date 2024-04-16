using LibraryManagement.Core.Entities;

namespace LibraryManagement.Infrastructure.IRepositories
{
    public interface IUserRepository
    {
      
        public Task<User> FindUserByEmailAsync(string email);
        public Task<User> CreateWithRoleAsync(User newUser, string password, string roleName);
        public Task<User> CheckUserLoginAsync(string email, string password);
        public Task<IEnumerable<User>> GetAllUsersAsync();
        public Task<IEnumerable<User>> SearchUserByName(string searchString);
        public Task<bool> ChangeUserStatus(string userId);
        public Task<User> UpdateUserAsync(User user);


    }
}
