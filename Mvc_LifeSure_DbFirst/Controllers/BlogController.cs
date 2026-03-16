using Mvc_LifeSure_DbFirst.Services.BlogServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }
        public PartialViewResult Index()
        {
            var blogs = _blogService.GetAll();
            return PartialView(blogs);
        }
    }
}