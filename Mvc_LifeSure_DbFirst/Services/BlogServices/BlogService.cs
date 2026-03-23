using FluentValidation;
using Mapster;
using Mvc_LifeSure_DbFirst.Data.Entities;
using Mvc_LifeSure_DbFirst.Dtos.BlogDtos;
using Mvc_LifeSure_DbFirst.Repositories.BlogRepositories;
using System;
using System.Collections.Generic;

namespace Mvc_LifeSure_DbFirst.Services.BlogServices
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IValidator<CreateBlogDto> _createValidator;
        private readonly IValidator<UpdateBlogDto> _updateValidator;

        public BlogService(
            IBlogRepository blogRepository,
            IValidator<CreateBlogDto> createValidator,
            IValidator<UpdateBlogDto> updateValidator)
        {
            _blogRepository = blogRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public List<ResultBlogDto> GetAll()
        {
            return _blogRepository.GetAll().Adapt<List<ResultBlogDto>>();
        }

        public UpdateBlogDto GetById(int id)
        {
            var blog = _blogRepository.GetById(id)
                ?? throw new KeyNotFoundException($"Blog kaydı bulunamadı (Id: {id})");
            return blog.Adapt<UpdateBlogDto>();
        }

        public void Create(CreateBlogDto create)
        {
            _createValidator.ValidateAndThrow(create);
            _blogRepository.Create(create.Adapt<Blog>());
        }

        public void Update(UpdateBlogDto update)
        {
            _updateValidator.ValidateAndThrow(update);
            var blog = _blogRepository.GetById(update.Id)
                ?? throw new KeyNotFoundException($"Blog kaydı bulunamadı (Id: {update.Id})");
            update.Adapt(blog);
            _blogRepository.Update(blog);   // BUG FIX: eksik çağrı eklendi
        }

        public void Delete(int id)
        {
            var blog = _blogRepository.GetById(id)
                ?? throw new KeyNotFoundException($"Blog kaydı bulunamadı (Id: {id})");
            _blogRepository.Delete(blog);
        }
    }
}