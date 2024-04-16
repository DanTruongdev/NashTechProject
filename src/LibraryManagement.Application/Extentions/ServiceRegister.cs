using LibraryManagement.Application.IServices;
using LibraryManagement.Application.Services;
using LibraryManagement.Infrastructure.IRepositories;
using LibraryManagement.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Application.Extentions
{
    public static class ServiceRegister
    {
        public static void Register(IServiceCollection services) 
        {
            //Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IBookService, BookService>();

            //Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IBookRepository, BookRepository>();


        }
    }
}
