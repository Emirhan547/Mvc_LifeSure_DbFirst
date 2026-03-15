using Mvc_LifeSure_DbFirst.Dtos.BlogDtos;
using Mvc_LifeSure_DbFirst.Services.BlogServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Areas.Admin.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        public ActionResult Index()
        {
            var blogs=_blogService.GetAll();
            return View(blogs);
        }

        [HttpGet]
        public ActionResult CreateBlog()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateBlog(CreateBlogDto create)
        {
            _blogService.Create(create);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public ActionResult UpdateBlog(int id)
        {
            var blogs = _blogService.GetById(id);
            return View(blogs);
        }
        public ActionResult UpdateBlog(UpdateBlogDto update)
        {
            _blogService.Update(update);
            return RedirectToAction(nameof(Index)); 
        }
        public ActionResult DeleteBlog(int id)
        {
            _blogService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}