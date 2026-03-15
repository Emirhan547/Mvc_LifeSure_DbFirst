using FluentValidation;
using Mapster;
using Mvc_LifeSure_DbFirst.Dtos.AboutDtos;
using Mvc_LifeSure_DbFirst.Dtos.BlogDtos;
using Mvc_LifeSure_DbFirst.Models;
using Mvc_LifeSure_DbFirst.Repositories.BlogRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Mvc_LifeSure_DbFirst.Services.BlogServices
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IValidator<CreateBlogDto> _createValidator;
        private readonly IValidator<UpdateBlogDto> _updateValidator;
        public BlogService(IBlogRepository blogRepository, IValidator<CreateBlogDto> createValidator, IValidator<UpdateBlogDto> updateValidator)
        {
            _blogRepository = blogRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public void Create(CreateBlogDto create)
        {
            _createValidator.ValidateAndThrow(create);
            var mappedBlogs = create.Adapt<Blogs>();
            _blogRepository.Create(mappedBlogs);
        }

        public void Delete(int id)
        {
            var abouts=_blogRepository.GetById(id);
            if(abouts == null)
            {
                throw new Exception("Blogs Not Found");
            }
            _blogRepository.Delete(abouts);
        }

        public List<ResultBlogDto> GetAll()
        {
            var blogs = _blogRepository.GetAll();
            return blogs.Adapt<List<ResultBlogDto>>();

        }

        public UpdateBlogDto GetById(int id)
        {
           var blogs=_blogRepository.GetById(id);
            if(blogs == null)
            {
                throw new Exception("Blogs Not Found");
            }
            return blogs.Adapt<UpdateBlogDto>();
        }

        public void Update(UpdateBlogDto update)
        {
            _updateValidator.ValidateAndThrow(update);
            var blogs = _blogRepository.GetById(update.Id);
            if( blogs == null)
            {
                throw new Exception("Blogs Not Fount");
            }
            update.Adapt(blogs);
        }
    }
}