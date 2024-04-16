using LibraryManagement.Application.Dtos;
using LibraryManagement.Application.IServices;
using LibraryManagement.Core.Entities;
using LibraryManagement.Infrastructure.Extentions;
using LibraryManagement.Infrastructure.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.Json;
using LibraryManagement.Infrastructure.Constants;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly ILogger<CategoryService> _logger;
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(ILogger<CategoryService> logger, IAuthorRepository authorRepository)
        {
            _logger = logger;
            _authorRepository = authorRepository;
        }

        public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
        {
            var categories = await _authorRepository.GetAllAuthorsAsync();
            return categories;
        }

        public async Task<IEnumerable<Author>> GetAllAuthorsAsync(string searchString)
        {
            var categories = await _authorRepository.GetAllAuthorsAsync(searchString);
            return categories;
        }

        public async Task<Author> GetAuthorByIdAsync(string authorId)
        {
            var author = await _authorRepository.GetAuthorByIdAsync(authorId);
            return author;
        }

        public async Task<Result> RemoveAuthorAsync(string authorId)
        {
            Author author = await _authorRepository.GetAuthorByIdAsync(authorId);
            if (author == null) return new Result(false, "The author does not exist");
            var result = await _authorRepository.RemoveAuthorAsync(author);
            return new Result("The author is removed");
        }

        public async Task<Result> AddAuthorAsync(AuthorDto form)
        {
            Author newAuthor = new Author()
            {
                AuthorId = Guid.NewGuid().ToString(),
                AuthorName = form.AuthorName
            };
            var result = await _authorRepository.AddAuthorAsync(newAuthor);
            if (!result) return new Result(false, "The author already exist!");
            return new Result("The author is added");
        }

        public async Task<Result> UpdateAuthorAsync(Author author)
        {
            Author authorExist = await _authorRepository.GetAuthorByIdAsync(author.AuthorId);
            if (authorExist == null) return new Result(false, "The author does not exist");
            var checkDuplicateResult = await _authorRepository.CheckDuplicateAuthorAsync(authorExist.AuthorId, author.AuthorName);
            if (!checkDuplicateResult) return new Result(false, "The author name already exist");
            var result = await _authorRepository.UpdateAuthorAsync(authorExist, author.AuthorName);
            if (!result) return new Result(false, "Failed to update the author name");
            return new Result("Update category successfully");
        }
    }
}
