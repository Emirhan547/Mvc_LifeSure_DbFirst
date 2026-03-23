using Microsoft.AspNet.Identity;
using Mvc_LifeSure_DbFirst.Services.AdminLogServices;
using System.Web.Mvc;

namespace Mvc_LifeSure_DbFirst.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public abstract class AdminBaseController : Controller
    {
        protected readonly IAdminLogService _logService;

        protected AdminBaseController(IAdminLogService logService)
        {
            _logService = logService;
        }

        protected void LogAction(string action, string actionType, string tableName, int? recordId = null)
        {
            var userId = User?.Identity?.GetUserId();
            _logService.LogAdminAction(userId, action, actionType, tableName, recordId);
        }

        protected bool IsAjaxRequest => Request.IsAjaxRequest();
    }
}