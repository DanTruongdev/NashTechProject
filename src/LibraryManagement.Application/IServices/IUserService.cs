using LibraryManagement.Application.Dtos;
using LibraryManagement.Core.Entities;
using Microsoft.AspNetCore.Mvc;
namespace LibraryManagement.Application.IServices
{
    public interface IUserService
    {
        public Task<Result> CreateAsync(SignUpDto form, string roleName);
        public Task<Result> CreateWithGoogleAsync();
        public Task<Result> UpdateUserAsync(UpdateUserDto form);
        public Task<Result> LoginAsync(LoginDto form);
        public Task<User> GetCurrentUserAsync();
        public Task<Result> LogoutAsync();
        public Task<IEnumerable<UserDto>> GetAllUsersAsync();
        public Task<IEnumerable<UserDto>> SearchUserByName(string searchString);
        public Task<string> GetRoleAsync(User user);
        public Task<bool> ChangeUserStatus(string userId);
    }
}
