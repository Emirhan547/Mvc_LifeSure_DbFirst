using Mvc_LifeSure_DbFirst.Dtos.BlogDtos;
using Mvc_LifeSure_DbFirst.Services.AdminLogServices;
using Mvc_LifeSure_DbFirst.Services.BlogServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Areas.Admin.Controllers
{
    public class BlogController : AdminBaseController
    {
        private readonly IBlogService _blogService;

        public BlogController(
             IAdminLogService logService,
             IBlogService blogService) : base(logService)
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
        [ValidateAntiForgeryToken]
        public ActionResult CreateBlog(CreateBlogDto create)
        {
            _blogService.Create(create);
            LogAction("Blog kaydı oluşturuldu", "Create", "Blogs");
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public ActionResult UpdateBlog(int id)
        {
            var blogs = _blogService.GetById(id);
            return View(blogs);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateBlog(UpdateBlogDto update)
        {
            _blogService.Update(update);
            LogAction("Blog kaydı güncellendi", "Update", "Blogs", update.Id);
            return RedirectToAction(nameof(Index)); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteBlog(int id)
        {
            _blogService.Delete(id);
            LogAction("Blog kaydı silindi", "Delete", "Blogs", id);
            return RedirectToAction(nameof(Index));
        }
    }
}